using System;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;

namespace OmniKassa.Tests.Model.Response
{
    public class OrderStatusNotificationBuilder
    {
        private int poiId = 1;
        private String authentication = "authentication";
        private String expiry = "2000-01-01T00:00:00.000-0200";
        private String eventName = "event";
        private String signature = "2ef8975ecd1425ba5f3117e797047a1bd15c0a1e4a605656a69fbf49fb767281ec6b4e24a194bcc975285ebe978cfc0b662e530ff34f5090a4abb6626376f4ff";

        public OrderStatusNotificationBuilder WithPoiId(int poiId)
        {
            this.poiId = poiId;
            return this;
        }

        public OrderStatusNotificationBuilder WithAuthentication(String authentication)
        {
            this.authentication = authentication;
            return this;
        }

        public OrderStatusNotificationBuilder WithExpiry(String expiry)
        {
            this.expiry = expiry;
            return this;
        }

        public OrderStatusNotificationBuilder WithEventName(String eventName)
        {
            this.eventName = eventName;
            return this;
        }

        public OrderStatusNotificationBuilder WithSignature(String signature)
        {
            this.signature = signature;
            return this;
        }

        public ApiNotification Build()
        {
            return new ApiNotification(poiId, authentication, expiry, eventName, signature);
        }
    }
}
