using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TLSharp.Core.Network;

namespace TelegramFuhrer.BL
{
    public class TelegramClientEx : TelegramClient
    {
        private static readonly ConcurrentQueue<Guid> Queue = new ConcurrentQueue<Guid>();

        public TelegramClientEx(int apiId, string apiHash, ISessionStore store = null, string sessionUserId = "session",
            TcpClientConnectionHandler handler = null) : base(apiId, apiHash, store, sessionUserId, handler)
        {
        }

        public new async Task<T> SendRequestAsync<T>(TLMethod methodToExecute)
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.SendRequestAsync<T>(methodToExecute);
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await SendRequestAsync<T>(methodToExecute);
            }
        }

        public new async Task<TLContacts> GetContactsAsync()
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.GetContactsAsync();
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await GetContactsAsync();
            }
        }

        public new async Task<TLAbsUpdates> SendMessageAsync(TLAbsInputPeer peer, string message)
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.SendMessageAsync(peer, message);
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await SendMessageAsync(peer, message);
            }
        }

        public new async Task<Boolean> SendTypingAsync(TLAbsInputPeer peer)
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.SendTypingAsync(peer);
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await SendTypingAsync(peer);
            }
        }

        public new async Task<TLAbsDialogs> GetUserDialogsAsync()
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.GetUserDialogsAsync();
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await GetUserDialogsAsync();
            }
        }

        public new async Task<TLFound> SearchUserAsync(string q, int limit = 10)
        {
            try
            {
                WaitForQueue(Guid.NewGuid());
                return await base.SearchUserAsync(q, limit);
            }
            catch (FloodException ex)
            {
                Thread.Sleep(ex.TimeToWait);
                return await SearchUserAsync(q, limit);
            }
        }

        private static void WaitForQueue(Guid id)
        {
            Queue.Enqueue(id);
            Guid queueGuid;
            Queue.TryPeek(out queueGuid);
            while (queueGuid != id)
            {
                Thread.Sleep(100);
                Queue.TryPeek(out queueGuid);
            }

            Queue.TryDequeue(out queueGuid);
        }
    }
}