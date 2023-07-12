using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExternalHandler.Settings
{
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
    public class AppSettings
    {
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
        public static readonly string Configuration = "AppSettings";
        
        public string Token { get; set; } 
        public string BaseTelegramUrl { get; set; } 
        public string UrlValidBasketWithTrackId { get; set; } 
        public string ZibalVerifyPaymentUrl { get; set; } 
        public string ZibalInquiryPaymentUrl { get; set; } 

    }
}
