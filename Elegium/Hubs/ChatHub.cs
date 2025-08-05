using Elegium.Data;
using Elegium.Dtos.Chat;
using Elegium.ExtensionMethods;
using Elegium.Models;
using Elegium.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatHub(
            ApplicationDbContext context
            , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HubMethodName("SendPrivateMessage")]
        public async Task SendPrivateMessage(MessageDto message)
        {
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            var sender = await _userManager.GetUserAsync(Context.User);

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
                    ReceiverName = receiver.FirstName + " " + receiver.LastName
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
                    await Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessage",
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
                ReceiverName = sender.FirstName + " " + sender.LastName
            };

            if (sender.Connections.Count > 0)
            {
                foreach (var connection in sender.Connections)
                {
                    await Clients.Client(connection.ConnectionID).SendAsync("MyMessageSent", msgDto);
                }
            }
        }


        public async Task UserIsTypingPrivateChat(MessageDto message)
        {
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            _context.Entry(receiver)
               .Collection(u => u.Connections)
               .Query()
               .Where(c => c.Connected == true)
               .Load();
            var sender = await _userManager.GetUserAsync(Context.User);
            message.SenderId = sender.Id;
            if (receiver.Connections.Count != 0)
            {
                foreach (var connection in receiver.Connections)
                {
                    await Clients.Client(connection.ConnectionID).SendAsync("UpdateUserStatus",
                        new
                        {
                            msgDto = message,
                            openedThreadId = message.ThreadId + "_" + sender.Id,
                            Status = "Typing..."
                        });
                }
            }
        }

        public async Task UserDoneTypingPrivateChat(MessageDto message)
        {
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            _context.Entry(receiver)
               .Collection(u => u.Connections)
               .Query()
               .Where(c => c.Connected == true)
               .Load();
            var sender = await _userManager.GetUserAsync(Context.User);
            message.SenderId = sender.Id;
            if (receiver.Connections.Count != 0)
            {
                foreach (var connection in receiver.Connections)
                {
                    await Clients.Client(connection.ConnectionID).SendAsync("UpdateUserStatus",
                        new
                        {
                            msgDto = message,
                            openedThreadId = message.ThreadId + "_" + sender.Id,
                            Status = "Active Now"
                        });
                }
            }
        }

        public async Task GetLastSeenPrivateChat(MessageDto message)
        {
            try
            {
                var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
                var lastConnection = await _context.Connection
                    .Where(a => a.UserId == receiver.Id)
                    .OrderByDescending(a => a.ConnectionTime)
                    .FirstOrDefaultAsync();
                var sender = await _userManager.GetUserAsync(Context.User);
                message.SenderId = sender.Id;
                if (lastConnection == null)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdateUserStatus",
                    new
                    {
                        msgDto = message,
                        openedThreadId = message.ThreadId + "_" + receiver.Id,
                        Status = "Offline"
                    });
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdateUserStatus",
                    new
                    {
                        msgDto = message,
                        openedThreadId = message.ThreadId + "_" + receiver.Id,
                        Status = lastConnection.Connected ? "Active Now" : string.Format("last seen {0}", lastConnection.ConnectionTime.GetRelativeTime())
                    });
                }
            }
            catch (Exception ex)
            {

            }

        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(Context.User);
                var oneday = DateTime.UtcNow.AddDays(-1);
                var sleepyConnections = _context.Connection.Where(a => a.UserId == user.Id && a.ConnectionTime < oneday && a.Connected);
                _context.RemoveRange(sleepyConnections);
                //await _context.SaveChangesAsync();
                //var oldConnections = _context.Connection.Where(a => a.UserId == user.Id && a.Connected);
                //foreach (var c in oldConnections)
                //    c.Connected = false;
                //var myConnectionsWithThreadUsers = await (from t in _context.Thread
                //                                          join u1 in _context.ThreadUsers on t.ThreadId equals u1.ThreadId
                //                                          join u2 in _context.ThreadUsers on t.ThreadId equals u2.ThreadId
                //                                          where u1.UserId == user.Id && u2.UserId != user.Id
                //                                          join c in _context.Connection on u2.UserId equals c.UserId
                //                                          where c.Connected
                //                                          select new
                //                                          {
                //                                              c.ConnectionID
                //                                          }).ToListAsync();

                var conObj = new Connection
                {
                    ConnectionID = Context.ConnectionId,
                    UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"],
                    Connected = true,
                    User = user
                };
                await _context.Connection.AddAsync(conObj);
                await _context.SaveChangesAsync();
                //foreach (var con in myConnectionsWithThreadUsers)
                //await Clients.Client(con.ConnectionID).SendAsync("UserOnLine", user.Id);

                await Clients.AllExcept(Context.ConnectionId).SendAsync("UserOnLine", user.Id);
            }
            catch (Exception ex)
            {

            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = await _userManager.GetUserAsync(Context.User);
                //var myConnectionsWithThreadUsers = await (from t in _context.Thread
                //                                          join u1 in _context.ThreadUsers on t.ThreadId equals u1.ThreadId
                //                                          join u2 in _context.ThreadUsers on t.ThreadId equals u2.ThreadId
                //                                          where u1.UserId == user.Id && u2.UserId != user.Id
                //                                          join c in _context.Connection on u2.UserId equals c.UserId
                //                                          where c.Connected
                //                                          select new
                //                                          {
                //                                              c.ConnectionID,
                //                                              c.ConnectionTime
                //                                          }).ToListAsync();
                var connection = await _context.Connection.FindAsync(Context.ConnectionId);
                if (connection != null)
                {
                    connection.Connected = false;
                    await _context.SaveChangesAsync();
                }
                //foreach (var con in myConnectionsWithThreadUsers)
                //{
                await Clients.AllExcept(Context.ConnectionId).SendAsync("UserOffLine",
                        new
                        {
                            UserId = user.Id,
                            Status = connection == null ? string.Empty : string.Format("last seen {0}", connection.ConnectionTime.GetRelativeTime())
                        }) ;
               // }
            }
            catch (Exception ex)
            {

            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPhotoIdsToUser(MessageDto message)
        {
            var receiver = await _userManager.FindByIdAsync(message.ReceiverId);
            var sender = await _userManager.FindByIdAsync(message.SenderId);
           await _context.Entry(receiver)
               .Collection(u => u.Connections)
               .Query()
               .Where(c => c.Connected == true)
               .LoadAsync();
            var list = new List<string>();

            try
            {
                // foreach(var file in)
                message.FilesCount = message.Files.Count;
                message.Files = message.Files.Take(4).ToList();
            }
            catch(Exception ex)
            {

            }

            if(receiver.Connections.Count > 0)
            {
                foreach (var connection in receiver.Connections)
                {
                    await Clients.Client(connection.ConnectionID).SendAsync("UpdateMessagePhotos",
                        new
                        {
                            msgDto = message,
                            openedThreadId = message.ThreadId + "_" + sender.Id
                        });
                }
            }
        }
    }
}
