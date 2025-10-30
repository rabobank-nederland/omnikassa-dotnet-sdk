using Newtonsoft.Json;
using OmniKassa.Model.Response;
using OmniKassa.Tests;
using System;
using System.Collections.Generic;
using OmniKassa.Model.Enums;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class ShopperPaymentDetailsResponseTest
    {
        [Fact]
        public void DeserializeFromJson_Should_ReturnCorrectObject()
        {
            // Arrange & Act
            ShopperPaymentDetailsResponse actual = TestHelper.GetObjectFromJsonFile<ShopperPaymentDetailsResponse>("payment-details-list.json");
            ShopperPaymentDetailsResponse expected = CreateExpectedShopperPaymentDetailsResponse();

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.CardOnFileList);
            Assert.Equal(expected.CardOnFileList.Count, actual.CardOnFileList.Count);

            // Verify first card
            Assert.Equal(expected.CardOnFileList[0].Id, actual.CardOnFileList[0].Id);
            Assert.Equal(expected.CardOnFileList[0].Last4Digits, actual.CardOnFileList[0].Last4Digits);
            Assert.Equal(expected.CardOnFileList[0].Brand, actual.CardOnFileList[0].Brand);
            Assert.Equal(expected.CardOnFileList[0].CardExpiry, actual.CardOnFileList[0].CardExpiry);
            Assert.Equal(expected.CardOnFileList[0].TokenExpiry, actual.CardOnFileList[0].TokenExpiry);
            Assert.Equal(expected.CardOnFileList[0].Status, actual.CardOnFileList[0].Status);

            // Verify second card
            Assert.Equal(expected.CardOnFileList[1].Id, actual.CardOnFileList[1].Id);
            Assert.Equal(expected.CardOnFileList[1].Last4Digits, actual.CardOnFileList[1].Last4Digits);
            Assert.Equal(expected.CardOnFileList[1].Brand, actual.CardOnFileList[1].Brand);
            Assert.Equal(expected.CardOnFileList[1].CardExpiry, actual.CardOnFileList[1].CardExpiry);
            Assert.Equal(expected.CardOnFileList[1].TokenExpiry, actual.CardOnFileList[1].TokenExpiry);
            Assert.Equal(expected.CardOnFileList[1].Status, actual.CardOnFileList[1].Status);

            // Verify third card
            Assert.Equal(expected.CardOnFileList[2].Id, actual.CardOnFileList[2].Id);
            Assert.Equal(expected.CardOnFileList[2].Last4Digits, actual.CardOnFileList[2].Last4Digits);
            Assert.Equal(expected.CardOnFileList[2].Brand, actual.CardOnFileList[2].Brand);
            Assert.Equal(expected.CardOnFileList[2].CardExpiry, actual.CardOnFileList[2].CardExpiry);
            Assert.Equal(expected.CardOnFileList[2].TokenExpiry, actual.CardOnFileList[2].TokenExpiry);
            Assert.Equal(expected.CardOnFileList[2].Status, actual.CardOnFileList[2].Status);
        }

        [Fact]
        public void DeserializeFromJson_Should_HandleEmptyList()
        {
            // Arrange
            string emptyJson = "{ \"cardOnFileList\": [] }";

            // Act
            ShopperPaymentDetailsResponse actual = JsonConvert.DeserializeObject<ShopperPaymentDetailsResponse>(emptyJson);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.CardOnFileList);
            Assert.Empty(actual.CardOnFileList);
        }

        [Fact]
        public void Constructor_Should_InitializeEmptyList()
        {
            // Act
            ShopperPaymentDetailsResponse response = new ShopperPaymentDetailsResponse();

            // Assert
            Assert.NotNull(response.CardOnFileList);
            Assert.Empty(response.CardOnFileList);
        }

        private static ShopperPaymentDetailsResponse CreateExpectedShopperPaymentDetailsResponse()
        {
            return new ShopperPaymentDetailsResponse
            {
                CardOnFileList = new List<CardOnFile>
                {
                    new CardOnFile
                    {
                        Id = "a2d4f4d8-32fa-468c-9d14-f996ec4a8bb7",
                        Last4Digits = "1234",
                        Brand = "MASTERCARD",
                        CardExpiry = "2029-12",
                        TokenExpiry = "2027-01",
                        Status = CardStatus.ACTIVE
                    },
                    new CardOnFile
                    {
                        Id = "b3e5g5e9-43gb-579d-a673-d107fd5b9cc8",
                        Last4Digits = "5678",
                        Brand = "VISA",
                        CardExpiry = "2028-06",
                        TokenExpiry = "2026-06",
                        Status = CardStatus.ACTIVE
                    },
                    new CardOnFile
                    {
                        Id = "c4f6h6f0-54hc-680e-b784-e218ge6c0dd9",
                        Last4Digits = "9012",
                        Brand = "MAESTRO",
                        CardExpiry = "2030-03",
                        TokenExpiry = "2028-03",
                        Status = CardStatus.INACTIVE
                    }
                }
            };
        }
    }
}
