using System;
using System.Globalization;
using System.Linq;
using System.Web;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace dotnet
{
    public  class SMSService
    {
       public Context _db;
         

        public SMSService(Context context)
        {
            _db = context;
        
        }
        static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public void sendMessage(string Message, string usermobile)
        {
          ;
            SendSMS(usermobile, Message);
        }
        public void sendCodeSMS (int code , string usermobile) {
                string sms = "Your OTP code for Kuick Save is  " + code + " For any issues, contact us at 03214658119 or kuicksave.com" ;
                SendSMS(usermobile , sms);
            }

             public void sendOrderCodeSMS (int code , string usermobile) {
                string sms = "Your Order is placed successfully. Order code is" + code + " you have to provide this code while recieving order" ;
                SendSMS(usermobile , sms);
            }

        public void sendShopAddSMS (string usermobile) {
                string sms = "New Shop is Added Login to kuicksave for details";
                SendSMS(usermobile , sms);
            }

            public void sendRiderAddSMS (string usermobile) {
                string sms = "New Rider is registered. Login to kuicksave for details";
                SendSMS(usermobile , sms);
            }

             public void sendShopOwnerAddSMS (string usermobile) {
                string sms = "New Shop Owner is registered. Login to kuicksave for details";
                SendSMS(usermobile , sms);
            }

             public void sendShopApproveSMS (int code , string usermobile) {
                string sms = "Your Shop Approval is successfull login to kuicksave for details " ;
                SendSMS(usermobile , sms);
            }

             public void sendOrderPlacedShopSMS (int code , string usermobile) {
                string sms = "New Order is placed. login to kuicksave for details" ;
                SendSMS(usermobile , sms);
            }
        
        public string SendSMS(string MobileNumber, string text)
        {
            string url = "http://cbs.zong.com.pk/reachrestapi/home/SendQuickSMS";
           var data = new 
           {
           loginId = "923187557283",
           loginPassword="Zong@123",
           Destination = "92" + MobileNumber,
           Mask="KUICK SAVE",
           Message=text,
           UniCode="0",
           ShortCodePrefered="n"
           };
           string json_data = JsonConvert.SerializeObject(data);
           return Controllers.UserController.sendRequest(url , json_data);
        
        }

        
    }
}