using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using DataLayer;
using DataLayer.Model;
using BusinessLayer.Contracts;

namespace BusinessLayer
{
    public class Currency
    {
        private HttpClient httpClient;
        private CurrencyDAL _currencyDal;
        private readonly IConfiguration _configuration;

        public Currency()
        {
            if (httpClient is null)
                httpClient = new HttpClient();

            if (_currencyDal is null)
                _currencyDal = new CurrencyDAL();

            if (_configuration is null)
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        private HttpRequestMessage CreateGetRequest(string endpoint)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(Settings.GetSetting("Api:Url") + endpoint);
            request.Method = HttpMethod.Get;
            request.Headers.Add("apikey", Settings.GetSetting("Api:ApiKey"));

            return request;
        }

        private async Task<string> GetResponseAsync(string endpoint)
        {
            var request = CreateGetRequest(endpoint);
            var response = httpClient.Send(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<CurrencyConversionResult> CurrencyConversionForAmountAsync(string fromCurrencyCode, string toCurrencyCode, double amount, DateTime? exchangeRateDate = null)
        {
            CurrencyConversionResult conversionResult;
            try
            {
                var endpoint = exchangeRateDate.HasValue
                                ? $"convert?from={fromCurrencyCode}&to={toCurrencyCode}&amount={amount}&date={exchangeRateDate.Value.Date:yyyy-MM-dd}"
                                : $"convert?from={fromCurrencyCode}&to={toCurrencyCode}&amount={amount}";

                conversionResult = JsonSerializer.Deserialize<CurrencyConversionResult>(await GetResponseAsync(endpoint));
            }
            catch (Exception ex)
            {
                //log error
                conversionResult = new CurrencyConversionResult { success = false, error = new Error { info = ex.Message } };
            }

            return conversionResult;
        }

        public async Task<ExhangeRatesDownloadResultEnum> DownloadCurrencyExchangeRatesAsync()
        {
            try
            {
                var todayDate = DateTime.Now.Date;
                if (!await _currencyDal.IsCurrencyExchangeRateAvailableForDate(todayDate))
                {
                    var exchangeRateDetails = await DownloadLatestCurrencyConversionExchangeRatesAsync();
                    if (exchangeRateDetails is not null && exchangeRateDetails.success)
                    {
                        var exchangeRateHistory = new ExchangeRateHistory
                        {
                            IsDownloadSuccess = exchangeRateDetails.success,
                            DownloadedByUser = "SYS",
                            DownloadTimeStamp = exchangeRateDetails.downloadTimestamp,
                            ExchangeRateDate = exchangeRateDetails.date,
                            BaseCurrency = exchangeRateDetails.@base,
                            ExchangeRateHistoryDetails = exchangeRateDetails.rates.Select(rate => new ExchangeRateHistoryDetail { CurrencyCode = rate.Key, ExchangeRate = rate.Value }).ToList()
                        };

                        await _currencyDal.InsertCurrencyExchangeRateDetails(exchangeRateHistory);
                        return ExhangeRatesDownloadResultEnum.Success;
                    }

                    return ExhangeRatesDownloadResultEnum.Fail;
                }

                return ExhangeRatesDownloadResultEnum.Skip;
            }
            catch
            {
                //log exception
                return ExhangeRatesDownloadResultEnum.Fail;
            }
        }

        private async Task<ExchangeRateDetails> DownloadLatestCurrencyConversionExchangeRatesAsync()
        {
            var endpoint = "latest";
            var result = await GetResponseAsync(endpoint);
            return JsonSerializer.Deserialize<ExchangeRateDetails>(result);
        }

        public async Task<ExchangeRateDetails> GetCurrencyExchangeRatesForDateAsync(DateTime exchangeRateDate)
        {
            var exchangeRateDetails = new ExchangeRateDetails();
            try
            {
                var exchangeRatesForDate = await _currencyDal.GetCurrencyExchangeRateForDate(exchangeRateDate);
                if (exchangeRatesForDate is not null)
                {
                    exchangeRateDetails.date = exchangeRatesForDate.ExchangeRateDate;
                    exchangeRateDetails.success = exchangeRatesForDate.IsDownloadSuccess;
                    exchangeRateDetails.@base = exchangeRatesForDate.BaseCurrency;
                    exchangeRateDetails.rates = exchangeRatesForDate.ExchangeRateHistoryDetails.ToDictionary(erhd => erhd.CurrencyCode, erhd => erhd.ExchangeRate);
                }
            }
            catch
            {
                //log exception
            }

            return exchangeRateDetails;
        }

        public async Task<List<ExchangeRate>> GetCurrencyExchangeRatesForPeriodAsync(string currencyCode, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var exchangeRateForPeriod = await _currencyDal.GetCurrencyExchangeRateForPeriod(currencyCode, fromDate, toDate);
                if (exchangeRateForPeriod.Count > 0)
                {
                    return exchangeRateForPeriod.Select(er => new ExchangeRate
                    {
                        baseCurrencyCode = er.BaseCurrency,
                        currencyCode = currencyCode.ToUpper(),
                        exchangeRateDate = er.ExchangeRateDate,
                        exchangeRate = er.ExchangeRateHistoryDetails.FirstOrDefault()?.ExchangeRate
                    }).OrderBy(er => er.exchangeRateDate).ToList();
                }
            }
            catch
            {
                //log exception
            }

            return null;
        }
    }
}
