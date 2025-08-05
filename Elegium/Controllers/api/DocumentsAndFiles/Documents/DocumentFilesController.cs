using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Elegium.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Elegium.Dtos;
using Elegium.Dtos.Chat;
using Elegium.Models.Chat;
using Elegium.Middleware;

namespace Elegium.Controllers.api.DocumentsAndFiles.Documents
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentFilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        public DocumentFilesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: api/DocumentFiles
        [HttpGet("{docCatId}/{page}/{size}")]
        public async Task<ActionResult> GetDocumentFiles(int docCatId, int page, int size)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.DocumentCategoryId == docCatId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    DocumentCategoryId = d.DocumentCategoryId.Value,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default
                                }).GetPaged(page, size);

            var defaultImageId = await _context.DocumentFiles.Where(a => a.Default && a.DocumentCategoryId == docCatId).FirstOrDefaultAsync();

            var list = from d in dbList
                       let latestVersion = (from v in _context.VersionFiles
                                            where d.Id == v.DocumentFileId
                                            //orderby d.CreateAtTicks ascending

                                            select v

                                                   ).OrderByDescending(a => a.CreateAtTicks).FirstOrDefault()


                       select new DocumentFilesDto()
                       {
                           ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                           DocumentCategoryId = d.DocumentCategoryId,
                           Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                           FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                           Id = d.Id,
                           MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                           Name = latestVersion == null ? d.Name : latestVersion.Name,
                           Type = latestVersion == null ? d.Type : latestVersion.Type,
                           UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                           UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                           UserName = latestVersion == null ? d.UserName : d.UserName,
                           RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                           UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                           Default = d.Default,
                           LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                       };

            return Ok(new { list, defaultImageId?.FileId });
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentFile(int id)
        {
            var nextPrevFileId = await _context.DocumentFiles.FindAsync(id);
            DocumentFilesDto dto = null;
            if (nextPrevFileId != null)
            {
                //var docCatName = await _context.DocumentCategory.FindAsync(nextPrevFileId.DocumentCategoryId);


                var d = new DocumentFilesDto()
                {
                    ContentType = nextPrevFileId.ContentType,
                    DocumentCategoryId = nextPrevFileId.DocumentCategoryId,
                    Extension = nextPrevFileId.Extension,
                    FileId = nextPrevFileId.FileId,
                    Id = nextPrevFileId.Id,
                    MimeType = nextPrevFileId.MimeType,
                    Name = nextPrevFileId.Name,
                    Type = nextPrevFileId.Type,
                    UserFriendlySize = nextPrevFileId.UserFriendlySize,
                    UserId = nextPrevFileId.UserId,
                    UserName = _context.Users.Find(nextPrevFileId.UserId).GetUserName(),
                    RelativeTime = nextPrevFileId.Created.GetRelativeTime(),
                    UserFriendlyTime = nextPrevFileId.Created.ToUserFriendlyTime()
                };


                var latestVersion = await _context.VersionFiles.Where(a => a.DocumentFileId == d.Id).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();

                dto = new DocumentFilesDto()
                {
                    ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                    DocumentCategoryId = d.DocumentCategoryId,
                    Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                    FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                    Id = d.Id,
                    MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                    Name = latestVersion == null ? d.Name : latestVersion.Name,
                    Type = latestVersion == null ? d.Type : latestVersion.Type,
                    UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                    UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                    UserName = latestVersion == null ? d.UserName : d.UserName,
                    RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                    UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                    Default = d.Default,
                    LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
                };

                return Ok(new { dto });
            }
            return Ok();
        }

        // PUT: api/DocumentFiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentFiles(int id, DocumentFiles documentFiles)
        {
            if (id != documentFiles.Id)
            {
                return BadRequest();
            }

            _context.Entry(documentFiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentFilesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DocumentFiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostCastDocumentFiles(List<DocumentFiles> documentFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var actorDefault = _context.DocumentFiles.Where(a => a.Default && a.ActorId == documentFiles.Select(a => a.ActorId ?? -1).DefaultIfEmpty(-1).First());
                var talentDefault = _context.DocumentFiles.Where(a => a.Default && a.TalentId == documentFiles.Select(a => a.TalentId ?? -1).DefaultIfEmpty(-1).FirstOrDefault());
                var charDefault = _context.DocumentFiles.Where(a => a.Default && a.CharId == documentFiles.Select(a => a.CharId ?? -1).DefaultIfEmpty(-1).FirstOrDefault());
                var extraDefault = _context.DocumentFiles.Where(a => a.Default && a.ExtraId == documentFiles.Select(a => a.ExtraId ?? -1).DefaultIfEmpty(-1).FirstOrDefault());
                var agencyDefault = _context.DocumentFiles.Where(a => a.Default && a.AgencyID == documentFiles.Select(a => a.AgencyID ?? -1).DefaultIfEmpty(-1).FirstOrDefault());
                if (documentFiles.Count(a => a.Default) > 0)
                {
                    if (actorDefault != null)
                        foreach (var a in actorDefault)
                            a.Default = false;
                    if (talentDefault != null)
                        foreach (var a in talentDefault)
                            a.Default = false;
                    if (charDefault != null)
                        foreach (var a in charDefault)
                            a.Default = false;
                    if (extraDefault != null)
                        foreach (var a in extraDefault)
                            a.Default = false;
                    if (agencyDefault != null)
                        foreach (var a in agencyDefault)
                            a.Default = false;
                }
                foreach (var d in documentFiles)
                {
                    d.UserFriendlySize = d.Size.GetBytesReadable();
                    d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                    FileInfo fi = new FileInfo(d.Name);
                    d.Type = fi.Extension.GetFileType();
                    d.UserId = user.Id;
                    d.Extension = fi.Extension;
                }
                await _context.DocumentFiles.AddRangeAsync(documentFiles);
                await _context.SaveChangesAsync();
                return Ok(documentFiles);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DocumentFiles>>> PostDocumentFiles(List<DocumentFiles> documentFiles)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var alreadyDefault = await _context.DocumentFiles.Where(a => a.Default && a.DocumentCategoryId == documentFiles.Select(a => a.DocumentCategoryId).FirstOrDefault()).FirstOrDefaultAsync();
                if (alreadyDefault != null && documentFiles.Count(a => a.Default) > 0)
                    alreadyDefault.Default = false;
                foreach (var d in documentFiles)
                {
                    d.UserFriendlySize = d.Size.GetBytesReadable();
                    d.MimeType = MimeMapping.MimeUtility.GetMimeMapping(d.Name);
                    FileInfo fi = new FileInfo(d.Name);
                    d.Type = fi.Extension.GetFileType();
                    d.UserId = user.Id;
                    d.Extension = fi.Extension;
                }
                await _context.DocumentFiles.AddRangeAsync(documentFiles);
                await _context.SaveChangesAsync();
                return Ok(documentFiles);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // DELETE: api/DocumentFiles/5
        [HttpPost("{id}")]
        public async Task<ActionResult<DocumentFiles>> DeleteDocumentFiles(int id)
        {
            var documentFiles = await _context.DocumentFiles.FindAsync(id);
            if (documentFiles == null)
            {
                return NotFound();
            }

            _context.DocumentFiles.Remove(documentFiles);
            await _context.SaveChangesAsync();

            return documentFiles;
        }

        private bool DocumentFilesExists(int id)
        {
            return _context.DocumentFiles.Any(e => e.Id == id);
        }

        [HttpGet("{id}/{flag}/{docCatId}")]
        public async Task<DocumentFilesDto> NextPrevFileId(int id, string flag, int docCatId)
        {
            var currentFileObj = await _context.DocumentFiles.Where(a => a.Id == id && a.DocumentCategoryId == docCatId).FirstOrDefaultAsync();
            var currentFileUploadTime = currentFileObj.CreateAtTicks;
            var user = await _userManager.GetUserAsync(User);
            DocumentFiles nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.DocumentCategoryId == docCatId && a.CreateAtTicks > currentFileUploadTime).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.DocumentCategoryId == docCatId).OrderBy(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.DocumentCategoryId == docCatId && a.CreateAtTicks < currentFileUploadTime).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await _context.DocumentFiles.Where(a => a.UserId.Equals(user.Id) && a.DocumentCategoryId == docCatId).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            var d = new DocumentFilesDto()
            {
                ContentType = nextPrevFileId.ContentType,
                DocumentCategoryId = nextPrevFileId.DocumentCategoryId.Value,
                Extension = nextPrevFileId.Extension,
                FileId = nextPrevFileId.FileId,
                Id = nextPrevFileId.Id,
                MimeType = nextPrevFileId.MimeType,
                Name = nextPrevFileId.Name,
                Type = nextPrevFileId.Type,
                UserFriendlySize = nextPrevFileId.UserFriendlySize,
                UserId = nextPrevFileId.UserId,
                UserName = _context.Users.Find(nextPrevFileId.UserId).GetUserName(),
                RelativeTime = nextPrevFileId.Created.GetRelativeTime(),
                UserFriendlyTime = nextPrevFileId.Created.ToUserFriendlyTime()
            };

            var latestVersion = await _context.VersionFiles.Where(a => a.DocumentFileId == d.Id).OrderByDescending(a => a.CreateAtTicks).FirstOrDefaultAsync();

            var dto = new DocumentFilesDto()
            {
                ContentType = latestVersion == null ? d.ContentType : latestVersion.ContentType,
                DocumentCategoryId = d.DocumentCategoryId,
                Extension = latestVersion == null ? d.Extension : latestVersion.Extension,
                FileId = latestVersion == null ? d.FileId : latestVersion.FileId,
                Id = d.Id,
                MimeType = latestVersion == null ? d.MimeType : latestVersion.MimeType,
                Name = latestVersion == null ? d.Name : latestVersion.Name,
                Type = latestVersion == null ? d.Type : latestVersion.Type,
                UserFriendlySize = latestVersion == null ? d.UserFriendlySize : latestVersion.UserFriendlySize,
                UserId = latestVersion == null ? d.UserId : latestVersion.UserId,
                UserName = latestVersion == null ? d.UserName : d.UserName,
                RelativeTime = latestVersion == null ? d.RelativeTime : latestVersion.Created.GetRelativeTime(),
                UserFriendlyTime = latestVersion == null ? d.UserFriendlyTime : latestVersion.Created.ToUserFriendlyTime(),
                Default = d.Default,
                LatestVersion = latestVersion == null ? string.Empty : latestVersion.Version
            };

            return dto;
        }

        [HttpPost]
        public async Task<DocumentFilesDto> UpdateFileName(DocumentFilesDto dto)
        {
            var fileObj = await _context.DocumentFiles.FindAsync(dto.Id);
            if (fileObj != null)
            {
                fileObj.Name = dto.Name;
                _context.Entry(fileObj).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return dto;
        }

        [HttpPost("{id}/{docCatId}")]
        public async Task<ActionResult<DocumentFiles>> MakeDefault(int id,
            int docCatId)
        {
            var documentFiles = await _context.DocumentFiles.Where(a => a.DocumentCategoryId == docCatId && a.Default).FirstOrDefaultAsync();
            if (documentFiles != null)
                documentFiles.Default = false;

            var docFile = await _context.DocumentFiles.FindAsync(id);
            if (docFile == null)
            {
                return NotFound();
            }
            docFile.Default = true;
            _context.Entry(docFile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return docFile;
        }

        [HttpPost]
        public async Task SendPrivateMessage(FileMessageDto dto)
        {
            //var sender = await _userManager.GetUserAsync(User);

            foreach (var u in dto.SendTo)
            {
                MessageDto message = new MessageDto()
                {
                    ReceiverId = u,
                    Text = dto.Message,
                    ThreadId = Guid.NewGuid(),
                    fileMsgDto = dto
                };
                await _notificationService.SendPrivateMessage(message);
                //var receiver = await _userManager.FindByIdAsync(message.ReceiverId);

                //if (receiver == null)
                //    continue;

                ////
                //Thread threadObj = await (from thread in _context.Thread
                //                          join user1Thread in _context.ThreadUsers
                //                          on thread.ThreadId equals user1Thread.ThreadId
                //                          join user2Thread in _context.ThreadUsers
                //                          on thread.ThreadId equals user2Thread.ThreadId
                //                          where user1Thread.UserId == receiver.Id && user2Thread.UserId == sender.Id
                //                          select new Thread()
                //                          {
                //                              ThreadId = thread.ThreadId,
                //                              UserId = thread.UserId
                //                          }
                //                          ).FirstOrDefaultAsync();
                ////will be null when user deleted the chat and now starting the conversation again.
                ////or starting chat for the first time.

                //if (threadObj == null)//don't know why it's null.. so let's try to find out.
                //{
                //    var chattedBeforeWithTheReceiver =

                //        await (from thread in _context.Thread
                //               join msg in _context.Message on thread.ThreadId equals msg.ThreadId
                //               join threadUser in _context.ThreadReadState on msg.Id equals threadUser.MessageId
                //               where (msg.SenderId == receiver.Id && threadUser.UserId == sender.Id)
                //               ||
                //               (msg.SenderId == sender.Id && threadUser.UserId == receiver.Id)
                //               select new
                //               {
                //                   thread.ThreadId
                //               }).FirstOrDefaultAsync();

                //    if (chattedBeforeWithTheReceiver == null)//never chatted before
                //    {
                //        threadObj = new Thread()
                //        {
                //            ThreadId = message.ThreadId.Value,
                //            UserId = sender.Id
                //        };
                //        await _context.Thread.AddAsync(threadObj);

                //        var ChatBox = new ChatBoxes()
                //        {
                //            ThreadId = message.ThreadId.Value,
                //            UserId = sender.Id,
                //            ReceiverId = receiver.Id
                //        };
                //        await _context.ChatBoxes.AddAsync(ChatBox);
                //    }
                //    else
                //    {
                //        threadObj = await _context.Thread.FindAsync(chattedBeforeWithTheReceiver.ThreadId);
                //    }
                //}

                //var newMessageObj = new Message()
                //{
                //    ThreadId = threadObj.ThreadId,
                //    Text = message.Text,
                //    SenderId = sender.Id,
                //    Id = Guid.NewGuid()
                //};

                //await _context.Message
                //    .AddAsync(newMessageObj);

                //ThreadUsers threadSenderUser =
                //    await _context.ThreadUsers
                //    .Where(a => a.ThreadId == threadObj.ThreadId && a.UserId == sender.Id)
                //    .FirstOrDefaultAsync();

                //ThreadUsers threadReceiverUser =
                //    await
                //    _context.ThreadUsers
                //    .Where(a => a.ThreadId == threadObj.ThreadId && a.UserId == receiver.Id)
                //    .FirstOrDefaultAsync();

                //if (threadSenderUser == null)
                //{
                //    threadSenderUser = new ThreadUsers()
                //    {
                //        ThreadId = threadObj.ThreadId,
                //        UserId = sender.Id,
                //        Id = Guid.NewGuid()
                //    };
                //    await _context.ThreadUsers.AddAsync(threadSenderUser);
                //}

                //if (threadReceiverUser == null)
                //{
                //    threadReceiverUser = new ThreadUsers()
                //    {
                //        ThreadId = threadObj.ThreadId,
                //        UserId = receiver.Id,
                //        Id = Guid.NewGuid()
                //    };
                //    await _context.ThreadUsers.AddAsync(threadReceiverUser);
                //}

                //var senderReadState = new ThreadReadState()
                //{
                //    Delivered = true,
                //    Read = true,
                //    MessageId = newMessageObj.Id,
                //    Id = Guid.NewGuid(),
                //    UserId = sender.Id
                //};
                //await _context.ThreadReadState.AddAsync(senderReadState);

                //var receiverReadState = new ThreadReadState()
                //{
                //    MessageId = newMessageObj.Id,
                //    Id = Guid.NewGuid(),
                //    UserId = receiver.Id
                //};
                //await _context.ThreadReadState.AddAsync(receiverReadState);

                //var tr = await _context.Thread.FindAsync(threadObj.ThreadId);
                //if (tr == null)
                //{
                //    threadObj.LastMessageId = newMessageObj.Id;
                //}
                //else
                //{
                //    tr.LastMessageId = newMessageObj.Id;
                //}

                //MessageFiles file = new MessageFiles()
                //{
                //    FileId = dto.FileObj.FileId,
                //    MessageId = newMessageObj.Id,
                //    Name = dto.FileObj.Name,
                //    Size = 1234,
                //    Type = dto.FileObj.Type
                //};

                //await _context.MessageFiles.AddAsync(file);
                //await _context.SaveChangesAsync();
            }
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFileDetailSummary(int fileId)
        {
            var fileVersionsCount = await _context.VersionFiles.CountAsync(a => a.DocumentFileId == fileId);
            var fileTasksCount = await _context.ProjectTasks.CountAsync(a => a.DocumentFilesId == fileId && !a.Deleted);
            var fileCommentsCount = await _context.Comments.CountAsync(a => a.DocumentFileId == fileId);
            return Ok(new
            {
                fileVersionsCount,
                fileTasksCount,
                fileCommentsCount
            });
        }


        #region Event related get requests
        [HttpGet("{eventId}")]
        public async Task<ActionResult> GetEventDocumentFiles(int eventId)
        {
            var dbList = await (from d in _context.DocumentFiles
                                join user in _context.Users
                                on d.UserId equals user.Id
                                where d.EventId == eventId
                                select new DocumentFilesDto()
                                {
                                    ContentType = d.ContentType,
                                    Extension = d.Extension,
                                    FileId = d.FileId,
                                    Id = d.Id,
                                    MimeType = d.MimeType,
                                    Name = d.Name,
                                    Type = d.Type,
                                    UserFriendlySize = d.UserFriendlySize,
                                    UserId = d.UserId,
                                    UserName = user.GetUserName(),
                                    RelativeTime = d.Created.GetRelativeTime(),
                                    UserFriendlyTime = d.Created.ToUserFriendlyTime(),
                                    Default = d.Default
                                }).ToListAsync();


            return Ok(dbList);
        }
        #endregion
    }
}
