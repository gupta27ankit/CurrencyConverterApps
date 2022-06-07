using NUnit.Framework;
using BusinessLayer;
using System.Threading.Tasks;

namespace Test
{
    public class CurrencyConversionTests
    {
        [Test]
        public async Task Test_CurrencyConversionForAmount_WithInvalidCurrencyCode_IsNotSuccessful()
        {
            var currencyBusiness = new Currency();

            var currencyCodeFrom = "EUR-1123";
            var currencyCodeTo = "NOK";
            var amount = 100D;

            var conversionResult = await currencyBusiness.CurrencyConversionForAmountAsync(currencyCodeFrom, currencyCodeTo, amount);
            Assert.IsFalse(conversionResult.success);
        }

        [Test]
        public async Task Test_CurrencyConversionForAmount_WithValidCurrencyCode_IsSuccessful()
        {
            var currencyBusiness = new Currency();

            var currencyCodeFrom = "EUR";
            var currencyCodeTo = "NOK";
            var amount = 100D;

            var conversionResult = await currencyBusiness.CurrencyConversionForAmountAsync(currencyCodeFrom, currencyCodeTo, amount);
            Assert.IsTrue(conversionResult.success);
            Assert.IsTrue(conversionResult.result > 0);
        }
    }
}