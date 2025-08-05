using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Elegium.Models;
using Elegium.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Http;
using System.Text;
using tusdotnet.Stores;
using System.IO;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Interfaces;
using Elegium.Hubs;
using Newtonsoft.Json.Serialization;
using tusdotnet.Models.Configuration;
using System.Net;
using Elegium.Middleware;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Elegium
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorPages();

            services.AddSignalR(hubOpts =>
            {
                hubOpts.EnableDetailedErrors = true;
                hubOpts.KeepAliveInterval = TimeSpan.FromMinutes(1);
                hubOpts.ClientTimeoutInterval = TimeSpan.FromMinutes(3);
            })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddOptions();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<INotificationService, NotificationService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.RequestCultureProviders.Clear();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseGleamTech();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSignalR(route => 
            //{ 
            //    route.MapHub<ChatHub>("/chathub")

            //    }

            //);

            //app.UseSignalR(routes => { routes.MapHub<ChatHub>("/chatHub"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.UseTus(context => new DefaultTusConfiguration
            {
                UrlPath = "/files",
                Store = new TusDiskStore(Path.Combine(env.ContentRootPath, @"uploads\tusio")),
                Events = new Events
                {
                    OnAuthorizeAsync = eventContext =>
                    {
                        if (!eventContext.HttpContext.User.Identity.IsAuthenticated)
                        {
                            eventContext.FailRequest(HttpStatusCode.Unauthorized);
                            return Task.CompletedTask;
                        }

                        // Do other verification on the user; claims, roles, etc. In this case, check the username.
                        //if (eventContext.HttpContext.User.Identity.Name != "test")
                        //{
                        //    eventContext.FailRequest(HttpStatusCode.Forbidden, "'test' is the only allowed user");
                        //    return Task.CompletedTask;
                        //}

                        // Verify different things depending on the intent of the request.
                        // E.g.:
                        //   Does the file about to be written belong to this user?
                        //   Is the current user allowed to create new files or have they reached their quota?
                        //   etc etc
                        //switch (ctx.Intent)
                        //{
                        //    case IntentType.CreateFile:
                        //        break;
                        //    case IntentType.ConcatenateFiles:
                        //        break;
                        //    case IntentType.WriteFile:
                        //        break;
                        //    case IntentType.DeleteFile:
                        //        break;
                        //    case IntentType.GetFileInfo:
                        //        break;
                        //    case IntentType.GetOptions:
                        //        break;
                        //    default:
                        //        break;
                        //}

                        return Task.CompletedTask;
                    }
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments(new PathString("/files"), StringComparison.Ordinal,
                        out PathString remaining))
                {
                    // Try to get a file id e.g. /files/<fileId>
                    string fileId = remaining.Value.TrimStart('/');
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        var store = new TusDiskStore(Path.Combine(env.ContentRootPath, @"uploads\tusio"));
                        var file = await store.GetFileAsync(fileId, context.RequestAborted);

                        if (file == null)
                        {
                            context.Response.StatusCode = 404;
                            await context.Response.WriteAsync($"File with id {fileId} was not found.", context.RequestAborted);
                            return;
                        }
                        var fileStream = await file.GetContentAsync(context.RequestAborted);
                        var metadata = await file.GetMetadataAsync(context.RequestAborted);

                        // The tus protocol does not specify any required metadata.
                        // "contentType" is metadata that is specific to this domain and is not required.
                        context.Response.ContentType = metadata.ContainsKey("contentType")
                                  ? metadata["contentType"].GetString(Encoding.UTF8)
                                  : "application/octet-stream";

                        if (metadata.ContainsKey("name"))
                        {
                            var name = metadata["name"].GetString(Encoding.UTF8);
                            context.Response.Headers.Add("Content-Disposition", new[] { $"attachment; filename=\"{name}\"" });
                        }

                        using (fileStream = await file.GetContentAsync(context.RequestAborted))
                        {
                            await fileStream.CopyToAsync(context.Response.Body, context.RequestAborted);
                        }
                    }
                    else
                    {
                        // Call next handler in pipeline if it's something else
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
