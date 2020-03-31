using System;
using System.Collections.Generic;
using System.Text;
using OmniKassa.Exceptions;
using OmniKassa.Model.Response;
using OmniKassa.Model.Enums;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class PaymentCompletedResponseTest
    {
        private static readonly String ORDER_ID = "ORDER1";
        private static readonly String STATUS = "COMPLETED";
        private static readonly String SIGNATURE = "6f4911ff3a77e0d5a323f91aad58bef2cdb4a8f0efc62d2fd4b82ca68d13ce4df5a4020ce390eff5a211551c6134b3fc02c750de9dc7bf04619f8bcfdd12d7c3";
        private static readonly byte[] SIGNING_KEY = Encoding.UTF8.GetBytes("secret");

        [Fact]
        public void ShouldBeAbleToCreateNewInstance()
        {
            PaymentCompletedResponse response = MakeResponse(SIGNATURE, SIGNING_KEY);

            Assert.Equal(ORDER_ID, response.OrderId);
            Assert.Equal(PaymentStatusParser.GetStatus(STATUS), response.Status);
        }

        [Fact]
        public void ShouldNotAcceptInvalidSignature()
        {
            Assert.Throws<IllegalSignatureException>(() => MakeResponse("wrong", SIGNING_KEY));
        }

        [Fact]
        public void ShouldNotAcceptInvalidSigningKey()
        {
            Assert.Throws<IllegalSignatureException>(() => MakeResponse(SIGNATURE, Encoding.UTF8.GetBytes("wrong")));
        }

        [Fact]
        public void ShouldReturnCorrectSignatureData()
        {
            PaymentCompletedResponse response = MakeResponse(SIGNATURE, SIGNING_KEY);
            List<String> actual = response.GetSignatureData();
            List<String> expected = new List<String>() {
                ORDER_ID, STATUS
            };

            Assert.Equal(expected, actual);
        }

        private PaymentCompletedResponse MakeResponse(String signature, byte[] signingKey)
        {
            return PaymentCompletedResponse.Create(ORDER_ID, STATUS, signature, signingKey);
        }
    }
}
