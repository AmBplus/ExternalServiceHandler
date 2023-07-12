using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ExternalHandler.Settings
{



#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
    public class VerifyAccountSettings

    {
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
        public static readonly string Configuration = "VerifyAccountSettings";
        private string _address;
        private string _clientId;
        private string _clientSecret;

        private string _scopes;
        private string _nid;



     
        public string Address
        {
            get { return _address; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _address = value;
            }
        } 


        public string ClientId
        {
            get { return _clientId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _clientId = value;
            }
        }


        public string ClientSecret
        {
            get { return _clientSecret; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientSecret cannot be null, empty, or whitespace.");
                }
                _clientSecret = value;
            }
        }



        public string Scopes
        {
            get { return _scopes; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Scopes cannot be null, empty, or whitespace.");
                }
                _scopes = value;
            }
        }


        public string Nid
        {
            get { return _nid; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Nid cannot be null, empty, or whitespace.");
                }
                _nid = value;
            }
        }
    }

}
