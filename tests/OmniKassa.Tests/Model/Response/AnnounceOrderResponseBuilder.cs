using System;
using Newtonsoft.Json;
using OmniKassa.Model.Response;

namespace OmniKassa.Tests.Model.Response
{
    public class AnnounceOrderResponseBuilder
    {
        private String redirectUrl = "http://returnAdress";
        private String omnikassaOrderId = "";

        public AnnounceOrderResponseBuilder WithRedirectUrl(String redirectUrl)
        {
            this.redirectUrl = redirectUrl;
            return this;
        }

        public AnnounceOrderResponseBuilder WithOmnikassaOrderId(String omnikassaOrderId)
        {
            this.omnikassaOrderId = omnikassaOrderId;
            return this;
        }

        public MerchantOrderResponse Build()
        {
            String json = "{ 'redirectUrl': " + redirectUrl + ", 'omnikassaOrderId': " + omnikassaOrderId + " }";
            return JsonConvert.DeserializeObject<MerchantOrderResponse>(json);
        }
    }
}
