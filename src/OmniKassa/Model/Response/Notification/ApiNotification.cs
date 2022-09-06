using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OmniKassa.Model.Response.Notification
{
    /// <summary>
    /// Order status notification
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiNotification : SignedResponse
    {
        /// <summary>
        /// The poi id for which this Notification is meant (only relevant of multiple poi share one webhook)
        /// </summary>
        /// <value>The poi identifier.</value>
        [JsonProperty(PropertyName = "poiId")]
        public int PoiId { get; set; }

        /// <summary>
        /// Authentication string which the SDK uses to retrieve the status updates from the API
        /// </summary>
        /// <value>The authentication.</value>
        [JsonProperty(PropertyName = "authentication")]
        public String Authentication { get; set; }

        /// <summary>
        /// A date string till when this Notification can be used to retrieve data from the remote API
        /// </summary>
        /// <value>Expiry date.</value>
        [JsonProperty(PropertyName = "expiry")]
        public String Expiry { get; set; }

        /// <summary>
        /// The type of event which is retrieved
        /// </summary>
        /// <value>The name of the event.</value>
        [JsonProperty(PropertyName = "eventName")]
        public String EventName { get; set; }

        /// <summary>
        /// Initializes an empty OrderStatusNotification
        /// </summary>
        public ApiNotification() :
            base(null)
        {

        }

        /// <summary>
        /// Initializes an OrderStatusNotification from the individual fields
        /// </summary>
        /// <param name="poiId">Poi id for which this Notification is meant (only relevant of multiple poi share one webhook)</param>
        /// <param name="authentication">Authentication string which the SDK uses to retrieve the status updates from the API</param>
        /// <param name="expiry">Date string till when this notification can be used to retrieve data from the remote API</param>
        /// <param name="eventName">Type of event which is retrieved</param>
        /// <param name="signature">Signature of this notification, this is validated by the SDK when retrieving the details</param>
        public ApiNotification(int poiId, String authentication, String expiry, String eventName, String signature) :
            base(signature)
        {
            this.PoiId = poiId;
            this.Authentication = authentication;
            this.Expiry = expiry;
            this.EventName = eventName;
        }

        /// <summary>
        /// Gets the signature data
        /// </summary>
        /// <returns>Signature data</returns>
        public override List<String> GetSignatureData()
        {
            return new List<String>(new String[] {
                    Authentication,
                    Expiry,
                    EventName,
                    Convert.ToString(PoiId)
            });
        }
    }
}
