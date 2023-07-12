

namespace ExternalHandler.Settings
{



#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable RCS1110 // Declare type inside namespace.
    public class ZibalPaymentSettings

    {
#pragma warning restore RCS1110 // Declare type inside namespace.
#pragma warning restore CA1050 // Declare types in namespaces
        public static readonly string Configuration = "ZibalPaymentSettings";
        private string _merchant;
        private string _urlStartPaymentRequest;
        private string _baseCallbackUrl;
        private string _urlVerifyPaymentRequest;
        private string _urlInquiryPaymentRequest;


        

        public string Merchant
        {
            get { return _merchant; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _merchant = value;
            }
        }


        public string BaseCallbackUrl
        {
            get { return _baseCallbackUrl; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _baseCallbackUrl = value;
            }
        }
        public string UrlPaymentRequest
        {
            get { return _urlStartPaymentRequest; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _urlStartPaymentRequest = value;
            }
        }

        public string UrlVerifyPaymentRequest
        {
            get { return _urlVerifyPaymentRequest; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _urlVerifyPaymentRequest = value;
            }
        }
        public string UrlToPayZibal { get; set; }
        public string UrlInquiryPaymentRequest
        {
            get { return _urlInquiryPaymentRequest; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ClientId cannot be null, empty, or whitespace.");
                }
                _urlInquiryPaymentRequest = value;
            }
        }


    }

}
