using FluentFTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Ftp Helper
    /// </summary>
    public class Ftp : IDisposable
    {
        private FtpClient client = null;

        public Ftp(string host, int port, string user, string pass)
        {
            client = new FtpClient(host, port, user, pass);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
