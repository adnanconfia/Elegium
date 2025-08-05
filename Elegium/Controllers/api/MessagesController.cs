using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models.Chat;
using System.Drawing.Text;
using Microsoft.AspNetCore.Identity;
using Elegium.Models;
using Microsoft.VisualBasic;
using Elegium.Dtos.Chat;
using Microsoft.AspNetCore.Authorization;
using Elegium.Configuration;
using Elegium.ExtensionMethods;
using System.Drawing;
using System.Text;
using tusdotnet.Stores;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LazZiya.ImageResize;
using System.Drawing.Imaging;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }

        // GET: api/GetConversationThreads
        [HttpGet]
        public async Task<ActionResult> GetConversationThreads()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var dbList = await _context.ConversationThreadsDto.FromSqlRaw(string.Format(@"
                                                        SELECT M.Created as ""When"",
                                                            m.Id as MessageId,
                                                            IIF(y.UnreadMsgs > 0, y.UnreadMsgs, 0) UnreadMsgs,
                                                            y.UserId,
                                                            y.ThreadId,
                                                            m.Text,
                                                            u.FirstName + ' ' + u.LastName as Name,
                                                            IIF(y.UnreadMsgs > 0, CAST('FALSE' as bit), CAST('TRUE' as bit)) ""Read""
                                                              FROM(SELECT t.ThreadId,
                                                                           t.UserId,
                                                                           T.LASTMESSAGEiD,
                                                                           SUM(UnreadMsgs) AS UnreadMsgs
                                                                      FROM(select t.ThreadId,
                                                                                   u2.UserId,
                                                                                   IIF(S.""READ"" = 0, 1, 0) UnreadMsgs,
                                                                                   T.LASTMESSAGEiD
                                                                              from thread t
                                                                              join ThreadUsers u1
                                                                                on u1.ThreadId = t.ThreadId
                                                                              join ThreadUsers u2
                                                                                on u2.ThreadId = t.ThreadId
                                                                              join Message m
                                                                                on m.ThreadId = t.ThreadId
                                                                              join ThreadReadState s
                                                                                on s.MessageId = m.Id
                                                                             where u1.UserId = '{0}'
                                                                               and u2.UserId <> '{0}'
                                                                               and s.UserId = u1.UserId
                                                                               and s.deleted = 0) T


                                                                     group by t.ThreadId, t.UserId, LASTMESSAGEiD

                                                                    ) Y
                                                              JOIN MESSAGE M
                                                                ON Y.LastMessageId = M.Id

                                                                join aspnetusers u

                                                                on y.UserId = u.Id

                                                                order by m.created desc
            ", loggedInUser.Id)).ToListAsync();

            var myThreads = from msg in dbList
                            select new ConversationThreadsDto()
                            {

                                When = msg.When,
                                MessageId = msg.MessageId,
                                Read = msg.Read,
                                Text = msg.Text,
                                ThreadId = msg.ThreadId,
                                Name = msg.Name,
                                UserId = msg.UserId,
                                Online = _context.Connection.Where(a => a.UserId == msg.UserId).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                                Opened = false,
                                Delivered = msg.Delivered,
                                Deleted = msg.Deleted,
                                FriendlyTime = msg.When.ToUserFriendlyTime(),
                                UnreadMsgs = msg.UnreadMsgs

                            };

            //var myThreads1 = await (from thread in _context.Thread
            //                       join user1Thread in _context.ThreadUsers on thread.ThreadId equals user1Thread.ThreadId
            //                       join user2Thread in _context.ThreadUsers on thread.ThreadId equals user2Thread.ThreadId
            //                       where user1Thread.UserId == loggedInUser.Id && user2Thread.UserId != loggedInUser.Id
            //                       join msg in _context.Message on thread.LastMessageId equals msg.Id
            //                       join user in _context.Users on user2Thread.UserId equals user.Id
            //                       join threadReadState in _context.ThreadReadState on msg.Id equals threadReadState.MessageId
            //                       where threadReadState.UserId == user2Thread.UserId
            //                       select new ConversationThreadsDto
            //                       {
            //                           When = msg.Created,
            //                           MessageId = msg.Id,
            //                           //Delivered = msg.SenderId != loggedInUser.Id ? _context.ThreadReadState.Where(a => a.MessageId == msg.Id && a.UserId != loggedInUser.Id).FirstOrDefault().Delivered : true,
            //                           Read = threadReadState.Read,
            //                           IsItFromMe = msg.SenderId == loggedInUser.Id ? true : false,
            //                           Text = msg.Text,
            //                           ThreadId = msg.ThreadId,
            //                           Name = user.FirstName + " " + user.LastName,
            //                           UserId = user.Id,
            //                           Online = _context.Connection.Where(a => a.Connected && a.UserId == user2Thread.UserId).Count() > 0 ? true : false,
            //                           Opened = false,
            //                           Delivered = threadReadState.Delivered,
            //                           Deleted = threadReadState.Deleted,
            //                           FriendlyTime = msg.Created.ToUserFriendlyTime(),
            //                           UnreadMsgs = _context.ThreadReadState.Where(a => a.MessageId == msg.Id && a.UserId == user1Thread.UserId && !a.Read).Count()

            //                       }).OrderByDescending(a => a.When)
            //                .ToListAsync();
            return Ok(new { myThreads });

        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversationMessages([FromBody] ConversationThreadsDto dto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            //var list = await (from msgs in _context.Message
            //                  join thread in _context.Thread on msgs.ThreadId equals thread.ThreadId
            //                  where thread.ThreadId == dto.ThreadId
            //                  join threadUser in _context.ThreadUsers on thread.ThreadId equals threadUser.ThreadId
            //                  where threadUser.UserId == loggedInUser.Id
            //                  join threadReadState in _context.ThreadReadState on msgs.Id equals threadReadState.MessageId
            //                  where threadReadState.UserId == threadUser.UserId
            //                  && !threadReadState.Deleted
            //                  join user in _context.Users on msgs.SenderId equals user.Id
            //                  select new MessageDto()
            //                  {
            //                      MessageId = msgs.Id,
            //                      IsItFromMe = loggedInUser.Id == msgs.SenderId ? true : false,
            //                      Text = msgs.Text,
            //                      Delivered = true,
            //                      Read = threadReadState.Read,
            //                      When = msgs.Created,
            //                      ThreadId = thread.ThreadId,
            //                      SenderId = msgs.SenderId,
            //                      ReceiverId = loggedInUser.Id

            //                  }).OrderBy(a => a.When).ToListAsync();


            var list = await (from msgs in _context.Message
                              join thread in _context.Thread on msgs.ThreadId equals thread.ThreadId
                              join threadUser1 in _context.ThreadUsers on thread.ThreadId equals threadUser1.ThreadId
                              where threadUser1.UserId == loggedInUser.Id
                              join threadUser2 in _context.ThreadUsers on thread.ThreadId equals threadUser2.ThreadId
                              where threadUser2.UserId == dto.UserId
                              join threadReadState in _context.ThreadReadState on msgs.Id equals threadReadState.MessageId
                              where threadReadState.UserId == threadUser1.UserId && !threadReadState.Deleted
                              join user in _context.Users on msgs.SenderId equals user.Id
                              select new MessageDto()
                              {
                                  MessageId = msgs.Id,
                                  IsItFromMe = loggedInUser.Id == msgs.SenderId,
                                  Text = msgs.Text,
                                  Delivered = threadReadState.Delivered,
                                  Read = _context.ThreadReadState.Count(a => !a.Read && a.MessageId == msgs.Id && a.UserId == threadUser2.UserId) == 0,
                                  When = msgs.Created,
                                  ThreadId = thread.ThreadId,
                                  SenderId = msgs.SenderId,
                                  ReceiverId = loggedInUser.Id,
                                  FriendlyTime = msgs.Created.ToUserFriendlyTime(),
                                  SenderName = user.FirstName + " " + user.LastName,
                                  Files = _context.MessageFiles.Where(a => a.MessageId == msgs.Id).OrderByDescending(a => a.CreatedAtTicks).Take(4).Select(a => a.FileId).ToList(),
                                  FilesCount = _context.MessageFiles.Count(a => a.MessageId == msgs.Id)
                              }).OrderByDescending(a => a.When).GetPaged(dto.PageIndex, 20);

            var threadReadStateUpdate =
                _context
                .ThreadReadState
                .Where(a => !a.Read && a.UserId == loggedInUser.Id && list.Select(a => a.MessageId).Contains(a.MessageId));

            foreach (var i in threadReadStateUpdate)
                i.Read = true;
            await _context.SaveChangesAsync();
            return list.ToList();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(Guid id)
        {
            var message = await _context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(Guid id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return message;
        }

        private bool MessageExists(Guid id)
        {
            return _context.Message.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> GetConversationWithUser([FromBody] ConversationThreadsDto dto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var receiver = await _userManager.FindByIdAsync(dto.UserId);
            var list = await (from msgs in _context.Message
                              join thread in _context.Thread on msgs.ThreadId equals thread.ThreadId
                              join threadUser1 in _context.ThreadUsers on thread.ThreadId equals threadUser1.ThreadId
                              where threadUser1.UserId == loggedInUser.Id
                              join threadUser2 in _context.ThreadUsers on thread.ThreadId equals threadUser2.ThreadId
                              where threadUser2.UserId == dto.UserId
                              join threadReadState in _context.ThreadReadState on msgs.Id equals threadReadState.MessageId
                              where threadReadState.UserId == threadUser1.UserId && !threadReadState.Deleted
                              join user in _context.Users on msgs.SenderId equals user.Id
                              select new MessageDto()
                              {
                                  MessageId = msgs.Id,
                                  IsItFromMe = loggedInUser.Id == msgs.SenderId,
                                  Text = msgs.Text,
                                  Delivered = true,
                                  Read = _context.ThreadReadState.Count(a => !a.Read && a.MessageId == msgs.Id && a.UserId == threadUser2.UserId) == 0,
                                  When = msgs.Created,
                                  ThreadId = thread.ThreadId,
                                  SenderId = msgs.SenderId,
                                  ReceiverId = loggedInUser.Id,
                                  SenderName = user.FirstName + " " + user.LastName,
                                  FriendlyTime = msgs.Created.ToUserFriendlyTime(),
                                  Files = _context.MessageFiles.Where(a => a.MessageId == msgs.Id).OrderByDescending(a => a.CreatedAtTicks).Take(4).Select(a => a.FileId).ToList(),
                                  FilesCount = _context.MessageFiles.Count(a => a.MessageId == msgs.Id)

                              }).OrderByDescending(a => a.When).GetPaged(dto.PageIndex, 12);

            var threadReadStateUpdate =
                _context
                .ThreadReadState
                .Where(a => !a.Read && a.UserId == loggedInUser.Id && list.Select(a => a.MessageId).Contains(a.MessageId));

            foreach (var i in threadReadStateUpdate)
                i.Read = true;
            await _context.SaveChangesAsync();


            var lastConnection = await _context.Connection
                .Where(a => a.UserId == receiver.Id)
                .OrderByDescending(a => a.ConnectionTime)
                .FirstOrDefaultAsync();

            string status;
            if (lastConnection == null)
            {
                status = "Offline";
            }
            else
            {
                status = lastConnection.Connected ? "Active Now" : $"last seen {lastConnection.ConnectionTime.GetRelativeTime()}";
            }


            if (list.Count > 0)
            {

                var chatBox = await _context.ChatBoxes.Where(a => a.UserId == loggedInUser.Id && a.ThreadId == (list.Count == 0 ? Guid.NewGuid() : list.FirstOrDefault().ThreadId)).FirstOrDefaultAsync();
                if (chatBox == null)
                {
                    chatBox = new ChatBoxes()
                    {
                        ThreadId = list.FirstOrDefault()?.ThreadId,
                        UserId = loggedInUser.Id,
                        ReceiverId = dto.UserId
                    };
                    await _context.ChatBoxes.AddAsync(chatBox);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new
            {
                list = list.Count == 0 ? list: list.OrderBy(a => a.When).ToList(),
                Box = new
                {
                    Name = receiver?.FirstName + " " + receiver?.LastName,
                    Online = lastConnection?.Connected,
                    status,
                    ThreadId = list.Count == 0 ? Guid.NewGuid() : list.FirstOrDefault()?.ThreadId,
                    //there's no chat so we will create this threadId when user sends a message
                    dto.UserId,
                    Text = string.Empty,
                    Opened = true,
                    NewChat = list.Count == 0,
                    LoggedInUser = loggedInUser.Id,
                    Closed = false
                }
            }); ;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConversation([FromBody] ConversationThreadsDto dto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    string.Format(@"update ThreadReadState
                                set  ThreadReadState.Deleted = 1
                                where exists (
                                select  1 from Message m,
                                Thread t,
                                ThreadUsers u
                                where m.Id = ThreadReadState.MessageId
                                and m.ThreadId = t.ThreadId
                                and u.ThreadId = t.ThreadId
                                and u.UserId = ThreadReadState.UserId
                                and t.ThreadId = '{0}'
                                )
                                and ThreadReadState.UserId = '{1}'", dto.ThreadId, loggedInUser.Id));

                var chatBox = await _context.ChatBoxes.Where(a => a.ThreadId == dto.ThreadId && a.UserId == loggedInUser.Id).FirstOrDefaultAsync();
                _context.Remove(chatBox);
                await _context.SaveChangesAsync();

                //await _context.Database.ExecuteSqlRawAsync(
                //    string.Format(
                //        @"delete from ThreadUsers where ThreadUsers.ThreadId = '{0}' and ThreadUsers.UserId = '{1}'", dto.ThreadId, loggedInUser.Id));
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Msg = ex.Message + (ex.InnerException?.Message) });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTop5Conversations()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var dbList = await _context.ConversationThreadsDto.FromSqlRaw(string.Format(@"
                                                        SELECT top 5 M.Created as ""When"",
                                                            m.Id as MessageId,
                                                            IIF(y.UnreadMsgs > 0, y.UnreadMsgs, 0) UnreadMsgs,
                                                            y.UserId,
                                                            y.ThreadId,
                                                            m.Text,
                                                            u.FirstName + ' ' + u.LastName as Name,
                                                            IIF(y.UnreadMsgs > 0, CAST('FALSE' as bit), CAST('TRUE' as bit)) ""Read""
                                                              FROM(SELECT t.ThreadId,
                                                                           t.UserId,
                                                                           T.LASTMESSAGEiD,
                                                                           SUM(UnreadMsgs) AS UnreadMsgs
                                                                      FROM(select t.ThreadId,
                                                                                   u2.UserId,
                                                                                   IIF(S.""READ"" = 0, 1, 0) UnreadMsgs,
                                                                                   T.LASTMESSAGEiD
                                                                              from thread t
                                                                              join ThreadUsers u1
                                                                                on u1.ThreadId = t.ThreadId
                                                                              join ThreadUsers u2
                                                                                on u2.ThreadId = t.ThreadId
                                                                              join Message m
                                                                                on m.ThreadId = t.ThreadId
                                                                              join ThreadReadState s
                                                                                on s.MessageId = m.Id
                                                                             where u1.UserId = '{0}'
                                                                               and u2.UserId <> '{0}'
                                                                               and s.UserId = u1.UserId
                                                                               and s.deleted = 0) T


                                                                     group by t.ThreadId, t.UserId, LASTMESSAGEiD

                                                                    ) Y
                                                              JOIN MESSAGE M
                                                                ON Y.LastMessageId = M.Id

                                                                join aspnetusers u

                                                                on y.UserId = u.Id

                                                                order by m.created desc
            ", loggedInUser.Id)).ToListAsync();

            var myThreads = from msg in dbList
                            select new ConversationThreadsDto()
                            {
                                When = msg.When,
                                MessageId = msg.MessageId,
                                Read = msg.Read,
                                Text = msg.Text,
                                ThreadId = msg.ThreadId,
                                Name = msg.Name,
                                UserId = msg.UserId,
                                Online = _context.Connection.Where(a => a.UserId == msg.UserId).OrderByDescending(a => a.ConnectionTime).FirstOrDefault()?.Connected,
                                Opened = false,
                                Delivered = msg.Delivered,
                                Deleted = msg.Deleted,
                                FriendlyTime = msg.When.ToUserFriendlyTime(),
                                UnreadMsgs = msg.UnreadMsgs
                            };

            var dict = new Dictionary<string, ConversationThreadsDto>();
            foreach (var t in myThreads)
                dict.Add(t.UserId, t);

            var openChatBoxes = await _context.ChatBoxes.Where(a => a.UserId == loggedInUser.Id).ToListAsync();
            return Ok(new
            {
                myThreads,
                UnreadCount = myThreads.Where(a => !a.Read).Count(),
                dict,
                openChatBoxes
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessageFiles([FromBody] MessageFiles[] messageFiles)
        {
            try
            {
                if (messageFiles != null && messageFiles.Length > 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    foreach (var item in messageFiles)
                    {
                        item.MessageId = item.MessageId;
                    }
                    await _context.MessageFiles.AddRangeAsync(messageFiles);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("{id}/{width}/{height}")]
        public async Task<IActionResult> GetFileThumbnail(string id, int width = 348, int height = 218)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var store = new TusDiskStore(Path.Combine(_env.ContentRootPath, @"uploads\tusio"));
                    var file = await store.GetFileAsync(id, HttpContext.RequestAborted);

                    if (file == null)
                    {
                        HttpContext.Response.StatusCode = 404;
                        await HttpContext.Response.WriteAsync($"File with id {id} was not found.", HttpContext.RequestAborted);
                        return NotFound();
                    }
                    var fileStream = await file.GetContentAsync(HttpContext.RequestAborted);
                    var metadata = await file.GetMetadataAsync(HttpContext.RequestAborted);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.FromStream(fileStream))
                    {
                        MemoryStream ms = new MemoryStream();
                        //await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        var img = ImageResize.ScaleByHeight(imgPhoto, height);
                        img.Save(ms, ImageFormat.Jpeg);
                        fileStream.Close();
                        return File(ms.ToArray(), "image/jpeg");
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                // }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMsg([FromBody] MessageDto msg)
        {
            var sender = await _userManager.GetUserAsync(User);
            var userMsg = await _context.ThreadReadState.Where(a => a.UserId == sender.Id && a.MessageId == msg.MessageId).FirstOrDefaultAsync();
            userMsg.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet("{id}/{flag}/{threadId}/{messageId}")]
        public async Task<IActionResult> NextPrevFileId(string id, string flag, string threadId, string messageId)
        {
            var currentFileObj = await _context.MessageFiles.Where(a => a.FileId == id).FirstOrDefaultAsync();

            var currentFileUploadTime = currentFileObj.CreatedAtTicks;
            var user = await _userManager.GetUserAsync(User);
            MediaFile nextPrevFileId = null;

            if (flag.Equals("P"))
            {
                var nextFileObj = await (from t in _context.Thread
                                         where t.ThreadId == Guid.Parse(threadId)
                                         join m in _context.Message on t.ThreadId equals m.ThreadId
                                         join s in _context.ThreadReadState on m.Id equals s.MessageId
                                         where s.UserId == user.Id && !s.Deleted
                                         join f in _context.MessageFiles on m.Id equals f.MessageId
                                         where f.CreatedAtTicks > currentFileUploadTime
                                         orderby f.CreatedAtTicks
                                         select new MediaFile()
                                         {
                                             FileId = f.FileId,
                                             Name = f.Name
                                         }).FirstOrDefaultAsync();

                if (nextFileObj == null)
                {
                    nextFileObj = await (from t in _context.Thread
                                         where t.ThreadId == Guid.Parse(threadId)
                                         join m in _context.Message on t.ThreadId equals m.ThreadId
                                         join s in _context.ThreadReadState on m.Id equals s.MessageId
                                         where s.UserId == user.Id && !s.Deleted
                                         join f in _context.MessageFiles on m.Id equals f.MessageId
                                         // where f.CreatedAtTicks > currentFileUploadTime
                                         orderby f.CreatedAtTicks
                                         select new MediaFile()
                                         {
                                             FileId = f.FileId,
                                             Name = f.Name
                                         }).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }
            else
            {
                var nextFileObj = await (from t in _context.Thread
                                         where t.ThreadId == Guid.Parse(threadId)
                                         join m in _context.Message on t.ThreadId equals m.ThreadId
                                         join s in _context.ThreadReadState on m.Id equals s.MessageId
                                         where s.UserId == user.Id && !s.Deleted
                                         join f in _context.MessageFiles on m.Id equals f.MessageId
                                         where f.CreatedAtTicks < currentFileUploadTime
                                         orderby f.CreatedAtTicks descending
                                         select new MediaFile()
                                         {
                                             FileId = f.FileId,
                                             Name = f.Name
                                         }).FirstOrDefaultAsync();
                if (nextFileObj == null)
                {
                    nextFileObj = await (from t in _context.Thread
                                         where t.ThreadId == Guid.Parse(threadId)
                                         join m in _context.Message on t.ThreadId equals m.ThreadId
                                         join s in _context.ThreadReadState on m.Id equals s.MessageId
                                         where s.UserId == user.Id && !s.Deleted
                                         join f in _context.MessageFiles on m.Id equals f.MessageId
                                         //where f.CreatedAtTicks < currentFileUploadTime
                                         orderby f.CreatedAtTicks descending
                                         select new MediaFile()
                                         {
                                             FileId = f.FileId,
                                             Name = f.Name
                                         }).FirstOrDefaultAsync();
                }
                nextPrevFileId = nextFileObj;
            }

            return Ok(new
            {
                Record = new
                {
                    nextPrevFileId.FileId,
                    nextPrevFileId.Name,
                    ThreadId = threadId,
                    MessageId = messageId
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> CloseChatBox([FromBody] ConversationThreadsDto dto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var chatBox = await _context.ChatBoxes.Where(a => a.ThreadId == dto.ThreadId && a.UserId == loggedInUser.Id).FirstOrDefaultAsync();
            if (chatBox != null)
            {
                _context.ChatBoxes.Remove(chatBox);
                await _context.SaveChangesAsync();
            }
            return Ok(new { success = true, deleted = chatBox != null });
        }
    }
}
