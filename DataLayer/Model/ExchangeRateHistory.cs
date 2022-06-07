using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class ExchangeRateHistory
    {
        public ExchangeRateHistory()
        {
            ExchangeRateHistoryDetails = new HashSet<ExchangeRateHistoryDetail>();
        }

        public int Id { get; set; }
        public DateTime ExchangeRateDate { get; set; }
        public bool IsDownloadSuccess { get; set; }
        public DateTime DownloadTimeStamp { get; set; }
        public string DownloadedByUser { get; set; } = null!;
        public string BaseCurrency { get; set; } = null!;

        public virtual ICollection<ExchangeRateHistoryDetail> ExchangeRateHistoryDetails { get; set; }
    }
}
