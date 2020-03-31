using Newtonsoft.Json;
using OmniKassa.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class PaymentBrandInfoTest
    {
        private static readonly String IDEAL = "IDEAL";
        private static readonly String ACTIVE = "Active";
        private static readonly String INACTIVE = "Inactive";


        [Fact]
        public void IsActiveTrueOK()
        {
            String json = "{ 'name': '" + IDEAL + "', 'status': '" + ACTIVE + "' }";
            PaymentBrandInfo info = JsonConvert.DeserializeObject<PaymentBrandInfo>(json);

            Assert.Equal(info.Name, IDEAL);
            Assert.True(info.IsActive);
        }

        [Fact]
        public void IsActiveFalseOK()
        {
            String json = "{ 'name': '" + IDEAL + "', 'status': '" + INACTIVE + "' }";
            PaymentBrandInfo info = JsonConvert.DeserializeObject<PaymentBrandInfo>(json);

            Assert.Equal(info.Name, IDEAL);
            Assert.False(info.IsActive);
        }
    }
}
