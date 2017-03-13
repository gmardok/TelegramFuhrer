using System;
using System.IO;
using TLSharp.Core;

namespace TelegramFuhrer.BL
{
    public class ServiceSessionStore : ISessionStore
    {
        public void Save(Session session)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.dat");

            using (FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate))
            {
                byte[] bytes = session.ToBytes();
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public Session Load(string sessionUserId)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.dat");

            if (!File.Exists(file))
                return null;

            var buffer = File.ReadAllBytes(file);
            return Session.FromBytes(buffer, this, sessionUserId);
        }
    }
}
