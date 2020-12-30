using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace dotnet
{

    public class EmailService 
    {

          private readonly Context _db;

        public EmailService(Context context)
        {
            _db = context;
        }


   // send email function 
        public  void sendCodeEmail (int code , string useremail) {
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage ("multistore199@gmail.com", useremail)) {
                mm.Subject = "multistore account verification";
                string body = "Your Account verification code is  " + code ;
                mm.Body = body;
                mm.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                // SmtpClient smtp = new SmtpClient ();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                // System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("multistore199@gmail.com", "Love@Pakistan@123");
                NetworkCredential NetworkCred = new NetworkCredential ("multistore199@gmail.com", "Love@Pakistan@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send (mm);

            }

        }
//
         public  void sendOrderCode (int code , string useremail) {
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage ("multistore199@gmail.com", useremail)) {
                mm.Subject = "kuickSave Order Code";
                string body = "Your Order is placed successfully. Order code is" + code + " you have to provide this code while recieving order" ;
                mm.Body = body;
                mm.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                // SmtpClient smtp = new SmtpClient ();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                // System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("multistore199@gmail.com", "Love@Pakistan@123");
                NetworkCredential NetworkCred = new NetworkCredential ("multistore199@gmail.com", "Love@Pakistan@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send (mm);

            }

        }


         // send email function 
        public  void sendShopEmail (long userId , string adminemail) {
            var shopOwner = _db.Users.Where(x=>x.Id == userId).FirstOrDefault();
            
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage ("multistore199@gmail.com", adminemail)) {
                mm.Subject = "multistore account verification";
                string body = "A new Shop is Added from " + shopOwner.FirstName+ " " + shopOwner.LastName;
                mm.Body = body;
                mm.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                // SmtpClient smtp = new SmtpClient ();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                // System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("multistore199@gmail.com", "Love@Pakistan@123");
                NetworkCredential NetworkCred = new NetworkCredential ("multistore199@gmail.com", "Love@Pakistan@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send (mm);

            }

        }
       
    }
}
