using FluentFTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Client
{
    /// <summary>
    /// Ftp Client
    /// </summary>
    public class Ftp : FtpClient
    {
        public Ftp(string host, int port, string user, string pass) : base(host, port, user, pass)
        {

        }
    }
}
