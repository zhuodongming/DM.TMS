using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// SFtp Helper
    /// </summary>
    public class SFtp : IDisposable
    {
        private SftpClient client = null;

        public SFtp(string host, int port, string username, string password)
        {
            client = new SftpClient(host, port, username, password);
            client.Connect();
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
