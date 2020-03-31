using System;
using System.Collections.Generic;
using System.Text;
using OmniKassa.Exceptions;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class OrderStatusNotificationTest
    {
        private static readonly byte[] SIGNING_KEY = Encoding.UTF8.GetBytes("secret");

        [Fact]
        public void BuildFromValues()
        {
            ApiNotification apiNotification = new OrderStatusNotificationBuilder().Build();

            AssertNotification(apiNotification);
        }

        private void AssertNotification(ApiNotification notification)
        {
            Assert.Equal(1, notification.PoiId);
            Assert.Equal("authentication", notification.Authentication);
            Assert.Equal("2000-01-01T00:00:00.000-0200", notification.Expiry);
            Assert.Equal("event", notification.EventName);

            notification.ValidateSignature(SIGNING_KEY);
        }

        [Fact]
        public void BuildFromJson_InvalidSignature()
        {
            ApiNotification notification = new OrderStatusNotificationBuilder().WithSignature("wrong").Build();

            Assert.Throws<IllegalSignatureException>(() => notification.ValidateSignature(SIGNING_KEY));
        }

        [Fact]
        public void GetSignatureData_Should_ReturnCorrectSignatureData()
        {
            String expectedSignatureData = "authentication,2000-01-01T00:00:00.000-0200,event,1";
            List<String> signatureData = new OrderStatusNotificationBuilder().Build().GetSignatureData();
            String actualSignatureData = String.Join(",", signatureData);
            Assert.Equal(expectedSignatureData, actualSignatureData);
        }
    }
}
