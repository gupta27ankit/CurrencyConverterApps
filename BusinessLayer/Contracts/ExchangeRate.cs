using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public class ExchangeRate
    {
        public string currencyCode { get; set; }
        public string baseCurrencyCode { get; set; }
        public DateTime exchangeRateDate { get; set; }
        public decimal? exchangeRate { get; set; }
    }
}
