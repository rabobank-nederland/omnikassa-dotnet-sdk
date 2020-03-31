using System;
using Newtonsoft.Json;
using OmniKassa.Model.Response;

namespace OmniKassa.Tests.Model.Response
{
    public class MerchantOrderStatusResponseBuilder
    {
        private bool moreOrderResultsAvailable = false;
        private String orderResults = InitializeOrderResults();
        private String signature = "eadd65e96b347c006e8e4e95cec30b9ab988226a7bb1cbdc9da9a521f245dd553cdb665f9cd5677fe782af3c0efee3a30025084b999a0bca795ce45eec23a2e7";

        private static String InitializeOrderResults()
        {
            return "[ " +
                    "{ poiId:'1', totalAmount:{ amount:'599', currency:'EUR'}, errorCode:'', paidAmount:{ amount:'0', currency:'EUR'}, merchantOrderId:'MYSHOP0001', orderStatusDateTime:'2016-07-28T12:51:15.574+02:00', orderStatus:'CANCELLED', omnikassaOrderId:'aec58605-edcf-4886-b12d-594a8a8eea60'}, " +
                    "{ poiId:'1', totalAmount:{ amount:'599', currency:'EUR'}, errorCode:'', paidAmount:{ amount:'599', currency:'EUR'}, merchantOrderId:'MYSHOP0002', orderStatusDateTime:'2016-07-28T13:58:50.205+02:00', orderStatus:'COMPLETED', omnikassaOrderId:'e516e630-9713-4cfa-ae88-c5fbc4b06744'}" +
                    " ]";
        }

        public MerchantOrderStatusResponseBuilder WithMoreOrderResultsAvailable(bool moreOrderResultsAvailable)
        {
            this.moreOrderResultsAvailable = moreOrderResultsAvailable;
            return this;
        }

        public MerchantOrderStatusResponseBuilder WithOrderResults(String orderResults)
        {
            this.orderResults = orderResults;
            return this;
        }

        public MerchantOrderStatusResponseBuilder WithSignature(String signature)
        {
            this.signature = signature;
            return this;
        }

        public MerchantOrderStatusResponse Build()
        {
            String builder = "{ " +
                    "orderResults: " + orderResults + ", " +
                    "signature: '" + signature + "', " +
                    "moreOrderResultsAvailable:" + Convert.ToString(moreOrderResultsAvailable).ToLower() + "}";
            
            return JsonConvert.DeserializeObject<MerchantOrderStatusResponse>(builder);
        }
    }
}
