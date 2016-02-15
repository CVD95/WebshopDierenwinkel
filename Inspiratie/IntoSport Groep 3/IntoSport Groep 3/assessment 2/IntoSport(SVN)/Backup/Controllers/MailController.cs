using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace IntoSport.Controllers
{
    public class MailController : Controller
    {
        public void MailUser(string receiver, string textSubject, string textBody)
        {
            //Specify senders gmail address
            string SendersAddress = "intosporttest@gmail.com";
            //Specify The Address You want to sent Email To(can be any valid email address)
            string ReceiversAddress = receiver;
            //Specify The password of gmail account u are using to sent mail(pw of sender@gmail.com)
            string SendersPassword = "testintosport";
            //Write the subject of your mail
            string subject = textSubject;
            //Write the contents of your mail
            string body = textBody;

            try
            {
                //we will use Smtp client which allows us to send email using SMTP Protocol
                //I have specified the properties of SmtpClient smtp within{}
                //gmails smtp server name is smtp.gmail.com and port number is 587
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(SendersAddress, SendersPassword),
                    Timeout = 3000
                };

                //MailMessage represents a mail message
                //it is 4 parameters(From,TO,subject,body)

                MailMessage message = new MailMessage(SendersAddress, ReceiversAddress, subject, body);
                /*WE use smtp sever we specified above to send the message(MailMessage message)*/

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                //als er een fout is dan krijg je de error te zien
                throw ex;
            }
        }

    }
}
