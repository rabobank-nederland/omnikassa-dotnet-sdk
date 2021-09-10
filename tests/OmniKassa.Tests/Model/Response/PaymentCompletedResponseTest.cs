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
            PaymentCompletedResponse response = MakeResponse(ORDER_ID, STATUS, SIGNATURE, SIGNING_KEY);

            Assert.Equal(ORDER_ID, response.OrderId);
            Assert.Equal(PaymentStatusParser.GetStatus(STATUS), response.Status);
        }

        [Fact]
        public void ShouldNotAcceptCryptographicallyIncorrectSignature()
        {
            Assert.Throws<IllegalSignatureException>(() => MakeResponse(ORDER_ID, STATUS, "00" + SIGNATURE.Substring(2), SIGNING_KEY));
        }

        [Fact]
        public void ShouldNotAcceptInvalidSigningKey()
        {
            Assert.Throws<IllegalSignatureException>(() => MakeResponse(ORDER_ID, STATUS, SIGNATURE, Encoding.UTF8.GetBytes("wrong")));
        }

        [Fact]
        public void ShouldNotAcceptInvalidOrderIds()
         {
            ShouldRejectInvalidOrderId("123<script>456");
            ShouldRejectInvalidOrderId("123 456");
            ShouldRejectInvalidOrderId("1234567890123456789012345");
            ShouldRejectInvalidOrderId("");
            ShouldRejectInvalidOrderId(null);
        }

        private void ShouldRejectInvalidOrderId(String orderId)
        {
            Assert.Throws<IllegalParameterException>(() => MakeResponse(orderId, STATUS, SIGNATURE, SIGNING_KEY));
        }

        [Fact]
        public void ShouldNotAcceptInvalidStatus()
        {
            ShouldRejectInvalidStatus("IN_<script>PROGRESS");
            ShouldRejectInvalidStatus("IN_P ROGRESS");
            ShouldRejectInvalidStatus("AAAAAAAAAAAAAAAAAAAAA");
            ShouldRejectInvalidStatus("");
            ShouldRejectInvalidStatus(null);
        }

        private void ShouldRejectInvalidStatus(String status) 
        {
            Assert.Throws<IllegalParameterException>(() => MakeResponse(ORDER_ID, status, SIGNATURE, SIGNING_KEY));
        }

        [Fact]
        public void ShouldNotAcceptInvalidSignature()
        {
            ShouldRejectInvalidSignature("6f4911ff3a77e0d5a323f91aad58bef2<script>efc62d2fd4b82ca68d13ce4df5a4020ce390eff5a211551c6134b3fc02c750de9dc7bf04619f8bcfdd12d7c3");
            ShouldRejectInvalidSignature(" f4911ff3a77e0d5a323f91aad58bef2cdb4a8f0efc62d2fd4b82ca68d13ce4df5a4020ce390eff5a211551c6134b3fc02c750de9dc7bf04619f8bcfdd12d7c3");
            ShouldRejectInvalidSignature(SIGNATURE + "f5");
            ShouldRejectInvalidSignature(SIGNATURE.Substring(2));
            ShouldRejectInvalidSignature("");
            ShouldRejectInvalidSignature(null);
        }

        private void ShouldRejectInvalidSignature(String signature)
        {
            Assert.Throws<IllegalParameterException>(() => MakeResponse(ORDER_ID, STATUS, signature, SIGNING_KEY));
        }

        [Fact]
        public void ShouldReturnCorrectSignatureData()
        {
            PaymentCompletedResponse response = MakeResponse(ORDER_ID, STATUS, SIGNATURE, SIGNING_KEY);
            List<String> actual = response.GetSignatureData();
            List<String> expected = new List<String>() {
                ORDER_ID, STATUS
            };

            Assert.Equal(expected, actual);
        }

        private PaymentCompletedResponse MakeResponse(String orderId, String status, String signature, byte[] signingKey)
        {
            return PaymentCompletedResponse.Create(orderId, status, signature, signingKey);
        }
    }
}
