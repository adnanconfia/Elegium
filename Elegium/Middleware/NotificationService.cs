using Elegium.Data;
using Elegium.Hubs;
using Elegium.Models;
using Elegium.Models.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elegium.Dtos.Notification;
using Elegium.ExtensionMethods;
using Elegium.Dtos.Chat;
using Elegium.Models.Chat;
using Microsoft.AspNetCore.Http;

namespace Elegium.Middleware
{
    public interface INotificationService
    {
        Task GenerateNotificationAsync(ApplicationUser sender, ApplicationUser receiver, string notificationKind, string url);
        Task SendPrivateMessage(MessageDto message);
    }
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _myHubContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(IHubContext<ChatHub> myHubContext, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _myHubContext = myHubContext;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task GenerateNotificationAsync(ApplicationUser sender, ApplicationUser receiver, string notificationKind, string url)
        {
            var notificationType = await _context.NotificationType.Where(a => a.Type == notificationKind).FirstOrDefaultAsync();


            if (notificationType != null)
            {
                await _context.Entry(receiver)
            .Collection(u => u.Connections)
            .Query()
            .Where(c => c.Connected == true)
            .LoadAsync();
                var notificationBody = notificationType
                    .Template
                    .Replace("{Sender}", sender.FirstName + " " + sender.LastName);

                var notificationObj = new Notification()
                {
                    Receipient = receiver,
                    Sender = sender,
                    Url = url,
                    NotificationText = notificationBody,
                    NotificationType = notificationType,
                    Read = false,
                    Deleted = false
                };

                await _context.Notification.AddAsync(notificationObj);
                await _context.SaveChangesAsync();
                var notificationDto = new NotificationDto()
                {
                    Title = notificationType.Title,
                    Created = notificationObj.Created,
                    When = notificationObj.Created.GetRelativeTime(),
                    Id = notificationObj.Id,
                    NotificationText = notificationBody,
                    Read = notificationObj.Read,
                    SenderId = notificationObj.Sender.Id,
                    Url = url
                };
                foreach (var connection in receiver.Connections)
                {
                    await _myHubContext.Clients.Client(connection.ConnectionID).SendAsync("NotificationHandler", notificationDto);
                }
            }
        }
        public async Task SendPrivateMessage(MessageDto message)
        {
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            var sender = await _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User);
            

            //
            Thread threadObj = await (from thread in _context.Thread
                                      join user1Thread in _context.ThreadUsers
                                      on thread.ThreadId equals user1Thread.ThreadId
                                      join user2Thread in _context.ThreadUsers
                                      on thread.ThreadId equals user2Thread.ThreadId
                                      where user1Thread.UserId == receiver.Id && user2Thread.UserId == sender.Id
                                      select new Thread()
                                      {
                                          ThreadId = thread.ThreadId,
                                          UserId = thread.UserId
                                      }
                                      ).FirstOrDefaultAsync();
            //will be null when user deleted the chat and now starting the conversation again.
            //or starting chat for the first time.

            if (threadObj == null)//don't know why it's null.. so let's try to find out.
            {
                var chattedBeforeWithTheReceiver =

                    await (from thread in _context.Thread
                           join msg in _context.Message on thread.ThreadId equals msg.ThreadId
                           join threadUser in _context.ThreadReadState on msg.Id equals threadUser.MessageId
                           where (msg.SenderId == receiver.Id && threadUser.UserId == sender.Id)
                           ||
                           (msg.SenderId == sender.Id && threadUser.UserId == receiver.Id)
                           select new
                           {
                               thread.ThreadId
                           }).FirstOrDefaultAsync();

                if (chattedBeforeWithTheReceiver == null)//never chatted before
                {
                    threadObj = new Thread()
                    {
                        ThreadId = message.ThreadId.Value,
                        UserId = sender.Id
                    };
                    await _context.Thread.AddAsync(threadObj);

                    var ChatBox = new ChatBoxes()
                    {
                        ThreadId = message.ThreadId.Value,
                        UserId = sender.Id,
                        ReceiverId = receiver.Id
                    };
                    await _context.ChatBoxes.AddAsync(ChatBox);
                }
                else
                {
                    threadObj = await _context.Thread.FindAsync(chattedBeforeWithTheReceiver.ThreadId);
                }
            }

            var newMessageObj = new Message()
            {
                ThreadId = threadObj.ThreadId,
                Text = message.Text,
                SenderId = sender.Id,
                Id = Guid.NewGuid()
            };

            await _context.Message
                .AddAsync(newMessageObj);

            ThreadUsers threadSenderUser =
                await _context.ThreadUsers
                .Where(a => a.ThreadId == threadObj.ThreadId && a.UserId == sender.Id)
                .FirstOrDefaultAsync();

            ThreadUsers threadReceiverUser =
                await
                _context.ThreadUsers
                .Where(a => a.ThreadId == threadObj.ThreadId && a.UserId == receiver.Id)
                .FirstOrDefaultAsync();

            if (threadSenderUser == null)
            {
                threadSenderUser = new ThreadUsers()
                {
                    ThreadId = threadObj.ThreadId,
                    UserId = sender.Id,
                    Id = Guid.NewGuid()
                };
                await _context.ThreadUsers.AddAsync(threadSenderUser);
            }

            if (threadReceiverUser == null)
            {
                threadReceiverUser = new ThreadUsers()
                {
                    ThreadId = threadObj.ThreadId,
                    UserId = receiver.Id,
                    Id = Guid.NewGuid()
                };
                await _context.ThreadUsers.AddAsync(threadReceiverUser);
            }

            var senderReadState = new ThreadReadState()
            {
                Delivered = true,
                Read = true,
                MessageId = newMessageObj.Id,
                Id = Guid.NewGuid(),
                UserId = sender.Id
            };
            await _context.ThreadReadState.AddAsync(senderReadState);

            var receiverReadState = new ThreadReadState()
            {
                MessageId = newMessageObj.Id,
                Id = Guid.NewGuid(),
                UserId = receiver.Id
            };
            await _context.ThreadReadState.AddAsync(receiverReadState);

            await _context.Entry(receiver)
                 .Collection(u => u.Connections)
                 .Query()
                 .Where(c => c.Connected == true)
                 .LoadAsync();

            await _context.Entry(sender)
                .Collection(u => u.Connections)
                .Query()
                .Where(c => c.Connected == true)
                .LoadAsync();
            MessageDto msgDto = null;
            if (receiver.Connections.Count != 0)
            {
                receiverReadState.Delivered = true;
                msgDto = new MessageDto()
                {
                    Delivered = receiverReadState.Delivered,
                    MessageId = newMessageObj.Id,
                    ReceiverId = receiver.Id,
                    SenderId = newMessageObj.SenderId,
                    Text = newMessageObj.Text,
                    Read = receiverReadState.Read,
                    When = newMessageObj.Created,
                    ThreadId = threadObj.ThreadId,
                    FriendlyTime = newMessageObj.Created.ToUserFriendlyTime(),
                    SenderName = sender.FirstName + " " + sender.LastName,
                    ReceiverName = receiver.FirstName + " " + receiver.LastName,
                    Files = message.fileMsgDto != null ? new string[] { message.fileMsgDto.FileObj.FileId }.ToList() : null
                };

                var unreadCount = await (from t in _context.Thread
                                         join m in _context.Message on t.ThreadId equals m.ThreadId
                                         join trs in _context.ThreadReadState on new { m1 = m.Id, r1 = receiver.Id } equals new { m1 = trs.MessageId, r1 = trs.UserId }
                                         where !trs.Read
                                         select new
                                         {
                                             m.Id
                                         }
                                         ).CountAsync();

                foreach (var connection in receiver.Connections)
                {
                    await _myHubContext.Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessage",
                        new
                        {
                            msgDto,
                            openedThreadId = threadObj.ThreadId + "_" + sender.Id,
                            UnreadCount = unreadCount + 1
                        });
                }
            }

            var tr = await _context.Thread.FindAsync(threadObj.ThreadId);
            if (tr == null)
            {
                threadObj.LastMessageId = newMessageObj.Id;
            }
            else
            {
                tr.LastMessageId = newMessageObj.Id;
            }

            if(message.fileMsgDto != null)
            {
                MessageFiles file = new MessageFiles()
                {
                    FileId = message.fileMsgDto.FileObj.FileId,
                    MessageId = newMessageObj.Id,
                    Name = message.fileMsgDto.FileObj.Name,
                    Size = 1234,
                    Type = message.fileMsgDto.FileObj.Type
                };

                await _context.MessageFiles.AddAsync(file);
            }

            await _context.SaveChangesAsync();
            msgDto = new MessageDto()
            {
                Delivered = receiverReadState.Delivered,
                MessageId = newMessageObj.Id,
                ReceiverId = receiver.Id,
                SenderId = newMessageObj.SenderId,
                Text = newMessageObj.Text,
                Read = receiverReadState.Read,
                When = newMessageObj.Created,
                ThreadId = threadObj.ThreadId,
                IsItFromMe = true,
                FriendlyTime = newMessageObj.Created.ToUserFriendlyTime(),
                SenderName = sender.FirstName + " " + sender.LastName,
                ReceiverName = sender.FirstName + " " + sender.LastName,
                Files = message.fileMsgDto != null ? new string[] { message.fileMsgDto.FileObj.FileId }.ToList() : null
            };

            if (sender.Connections.Count > 0)
            {
                foreach (var connection in sender.Connections)
                {
                    await _myHubContext.Clients.Client(connection.ConnectionID).SendAsync("MyMessageSent", msgDto);
                }
            }
        }
    }
}
