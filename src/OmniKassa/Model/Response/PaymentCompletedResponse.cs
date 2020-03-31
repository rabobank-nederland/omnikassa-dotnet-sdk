using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using OmniKassa.Exceptions;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response given by the OmniKassa API when a payment is completed and user returns to the webshop
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentCompletedResponse : SignedResponse
    {
        /// <summary>
        /// Order ID parameter key
        /// </summary>
        public static String ORDER_ID_KEY = "order_id";

        /// <summary>
        /// Status parameter key
        /// </summary>
        public static String STATUS_KEY = "status";

        /// <summary>
        /// Signature parameter key
        /// </summary>
        public static String SIGNATURE_KEY = "signature";

        [JsonProperty(PropertyName = "order_id")]
        private readonly String orderId;

        /// <summary>
        /// Order ID
        /// </summary>
        public String OrderId
        { 
            get 
            {
                if (isSignatureValid)
                {
                    return orderId;
                }
                else
                {
                    throw new SignatureNotValidatedException();
                }
            }
        }

        [JsonProperty(PropertyName = "status")]
        private readonly String status;

        /// <summary>
        /// Currently known state of the order
        /// </summary>
        public PaymentStatus? Status
        { 
            get
            {
                if (isSignatureValid)
                {
                    return PaymentStatusParser.GetStatus(status);
                }
                else
                {
                    throw new SignatureNotValidatedException();
                }
            }
        }

        /// <summary>
        /// Initializes an empty PaymentCompletedResponse
        /// </summary>
        public PaymentCompletedResponse() {
            
        }

        /// <summary>
        /// Initializes a PaymentCompletedResponse with given signature
        /// </summary>
        /// <param name="signature"></param>
        protected PaymentCompletedResponse(String signature) :
            base(signature)
        {
            
        }

        private PaymentCompletedResponse(String orderId, String status, String signature) :
            base(signature)
        {
            this.orderId = orderId;
            this.status = status;
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="orderId">Order ID as received by OmniKassa</param>
        /// <param name="status">Order status as received by the OmniKassa</param>
        /// <param name="signature">Signature of the response as received by OmniKassa</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(String orderId,
                                                      String status,
                                                      String signature,
                                                      String signingKey)
        {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            return Create(orderId, status, signature, signingKeyBytes);
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="orderId">Order ID as received by OmniKassa</param>
        /// <param name="status">Order status as received by the OmniKassa</param>
        /// <param name="signature">Signature of the response as received by OmniKassa</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(String orderId,
                                                      String status,
                                                      String signature,
                                                      byte[] signingKey)
        {
            PaymentCompletedResponse response = new PaymentCompletedResponse(orderId, status, signature);
            response.ValidateSignature(signingKey);
            return response;
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="dictionary">Key-value pairs with the response parameters</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(Dictionary<string, string> dictionary, String signingKey)
        {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            return Create(dictionary, signingKeyBytes);
        }
        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="dictionary">Key-value pairs with the response parameters</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(Dictionary<string, string> dictionary, byte[] signingKey)
        {
            PaymentCompletedResponse response = Create(dictionary);
            response.ValidateSignature(signingKey);
            return response;
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance
        /// </summary>
        /// <param name="dictionary">Key-value pairs with the response parameters</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(Dictionary<string, string> dictionary)
        {
            String orderId = dictionary[ORDER_ID_KEY];
            String status = dictionary[STATUS_KEY];
            String signature = dictionary[SIGNATURE_KEY];

            return new PaymentCompletedResponse(orderId, status, signature);
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="collection">Collection with the response parameters</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(NameValueCollection collection, String signingKey)
        {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            return Create(collection, signingKeyBytes);
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance and validates the signature.
        /// </summary>
        /// <param name="collection">Collection with the response parameters</param>
        /// <param name="signingKey">Signing key to validate the signature</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(NameValueCollection collection, byte[] signingKey)
        {
            PaymentCompletedResponse response = Create(collection);
            response.ValidateSignature(signingKey);
            return response;
        }

        /// <summary>
        /// Initializes a new PaymentCompletedResponse instance
        /// </summary>
        /// <param name="collection">>Collection with the response parameters</param>
        /// <returns>Payment completed response</returns>
        public static PaymentCompletedResponse Create(NameValueCollection collection)
        {
            String orderId = collection.Get(ORDER_ID_KEY);
            String status = collection.Get(STATUS_KEY);
            String signature = collection.Get(SIGNATURE_KEY);

            return new PaymentCompletedResponse(orderId, status, signature);
        }

        /// <summary>
        /// Gets the signature data
        /// </summary>
        /// <returns>Signature data</returns>
        public override List<String> GetSignatureData()
        {
            return new List<String>(new String[] {
                orderId, status
            });
        }
    }
}
