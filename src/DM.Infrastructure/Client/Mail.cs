using DM.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DM.Infrastructure.Client
{
    /// <summary>
    /// Mail Client
    /// </summary>
    public class Mail
    {
        private const int Timeout = 180000;
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _password;
        private readonly bool _ssl;

        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 密送/抄送
        /// </summary>
        public string RecipientCC { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public string AttachmentFile { get; set; }

        public Mail(string host, int port, string user, string possword, bool enableSSL)
        {
            _host = host;
            _port = port;
            _user = user;
            _password = possword;
            _ssl = enableSSL;
        }

        public void Send()
        {
            try
            {
                using (var smtp = new SmtpClient(_host, _port))
                {
                    if (!String.IsNullOrWhiteSpace(_user) && !String.IsNullOrWhiteSpace(_password))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_user, _password);
                        smtp.EnableSsl = _ssl;
                    }

                    using (var message = new MailMessage())
                    {
                        MailAddress fromMailAddress = new MailAddress(Sender);
                        message.From = fromMailAddress;
                        //message.Sender = fromMailAddress;
                        message.Subject = Subject;
                        message.Body = Body;
                        message.IsBodyHtml = true;

                        var list = Recipient.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //多个收件人;隔开
                        foreach (var item in list)
                        {
                            message.To.Add(item);
                        }

                        if (!String.IsNullOrWhiteSpace(RecipientCC))//抄送
                        {
                            list = RecipientCC.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); //多个抄送收件人;隔开
                            foreach (var item in list)
                            {
                                message.Bcc.Add(item);
                            }
                        }

                        if (!String.IsNullOrWhiteSpace(AttachmentFile))//附件
                        {
                            if (File.Exists(AttachmentFile))
                            {
                                var att = new Attachment(AttachmentFile);
                                message.Attachments.Add(att);
                            }
                        }

                        smtp.Send(message);//发送邮件
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("发送邮件出错", ex);
                throw;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send(string strSender, string strReciver, string strSubject, string strBody)
        {
            this.Sender = strSender;
            this.Recipient = strReciver;
            this.Subject = strSubject;
            this.Body = strBody;
            this.Send();
        }
    }
}
