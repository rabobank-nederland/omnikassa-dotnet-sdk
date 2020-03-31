using System;
using System.Collections.Generic;
using OmniKassa;
using OmniKassa.Model;
using Xunit;

namespace OmniKassa.Tests.Model
{
    public class SignableTest
    {
        private readonly byte[] signingKey;

        public SignableTest()
        {
            String base64EncodedSigningKey = "jOo9qaeEnQVM";
            signingKey = GetSigningKey(base64EncodedSigningKey);
        }

        [Fact]
        public void HappyFlow()
        {
            String signature = Signable.CalculateSignature(AsList("foo", "bar"), signingKey);
            String signature1 = Signable.CalculateSignature(AsList("foo", "bar"), signingKey);
            Assert.Equal(signature, signature1);
        }

        [Fact]
        public void CaseCapital()
        {
            String signature = Signable.CalculateSignature(AsList("foo", "bar"), signingKey);
            String signature1 = Signable.CalculateSignature(AsList("Foo", "bar"), signingKey);
            Assert.NotEqual(signature, signature1);
        }

        [Fact]
        public void CaseDifferentOrder()
        {
            String signature = Signable.CalculateSignature(AsList("foo", "bar"), signingKey);
            String signature1 = Signable.CalculateSignature(AsList("bar", "foo"), signingKey);
            Assert.NotEqual(signature, signature1);
        }

        private List<String> AsList(String a, String b)
        {
            return new List<String>() { a, b };
        }

        private byte[] GetSigningKey(String base64EncodedSigningKey)
        {
            Endpoint endpoint = Endpoint.Create("http://localhost/", base64EncodedSigningKey, "");
            return endpoint.SigningKey;
        }
    }
}
