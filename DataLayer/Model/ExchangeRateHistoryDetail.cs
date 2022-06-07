using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class ExchangeRateHistoryDetail
    {
        public int Id { get; set; }
        public int ExchangeRateHistoryId { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public decimal ExchangeRate { get; set; }

        public virtual ExchangeRateHistory ExchangeRateHistory { get; set; } = null!;
    }
}
