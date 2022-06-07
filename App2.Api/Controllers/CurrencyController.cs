using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using BusinessLayer.Contracts;

namespace App2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CurrencyController : Controller
    {
        Currency _currencyBusiness;
        public CurrencyController()
        {
            if (_currencyBusiness is null)
                _currencyBusiness = new Currency();
        }

        [HttpGet(Name = "CurrencyConversionForAmount")]
        public async Task<CurrencyConversionResult> GetCurrencyConversionForAmountAsync(string fromCurrencyCode, string toCurrencyCode, double amount)
        {
            return await _currencyBusiness.CurrencyConversionForAmountAsync(fromCurrencyCode, toCurrencyCode, amount);
        }

        [HttpGet(Name = "CurrencyConversionForAmountOnExchangeRateDate")]
        public async Task<CurrencyConversionResult> GetCurrencyConversionForAmountOnExchangeRateDateAsync(string fromCurrencyCode, string toCurrencyCode, double amount, DateTime date)
        {
            return await _currencyBusiness.CurrencyConversionForAmountAsync(fromCurrencyCode, toCurrencyCode, amount, date);
        }

        [HttpGet(Name = "CurrencyExchangeRatesForDate")]
        public async Task<ExchangeRateDetails> GetCurrencyExchangeRatesForDateAsync(DateTime exchangeRateDate)
        {
            return await _currencyBusiness.GetCurrencyExchangeRatesForDateAsync(exchangeRateDate);
        }

        [HttpGet(Name = "CurrencyExchangeRatesForPeriod")]
        public async Task<List<ExchangeRate>> GetCurrencyExchangeRatesForPeriodAsync(string currencyCode, DateTime fromDate, DateTime toDate)
        {
            return await _currencyBusiness.GetCurrencyExchangeRatesForPeriodAsync(currencyCode, fromDate, toDate);
        }
    }
}
