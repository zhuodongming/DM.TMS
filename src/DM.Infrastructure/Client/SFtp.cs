using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Client
{
    /// <summary>
    /// SFtp Client
    /// </summary>
    public class SFtp : SftpClient
    {
        public SFtp(string host, int port, string username, string password) : base(host, port, username, password)
        {
            this.Connect();
        }
    }
}
