using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;

namespace DataLayer
{
    public class CurrencyDAL
    {
        CurrencyContext _context;

        public CurrencyDAL()
        {
            if (_context is null)
                _context = new CurrencyContext();
        }

        public async Task<bool> IsCurrencyExchangeRateAvailableForDate(DateTime date)
            => await _context.ExchangeRateHistories.Where(erh => erh.ExchangeRateDate.Date == date.Date && erh.IsDownloadSuccess)
                                                   .AnyAsync();

        public async Task InsertCurrencyExchangeRateDetails(ExchangeRateHistory exchangeRateHistory)
        {
            _context.ExchangeRateHistories.Add(exchangeRateHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<ExchangeRateHistory> GetCurrencyExchangeRateForDate(DateTime exchangeRateDate)
            => await _context.ExchangeRateHistories.Where(erh => erh.ExchangeRateDate.Date == exchangeRateDate)
                                                   .Include(erh => erh.ExchangeRateHistoryDetails)
                                                   .FirstOrDefaultAsync();

        public async Task<List<ExchangeRateHistory>> GetCurrencyExchangeRateForPeriod(string currencyCode, DateTime fromDate, DateTime toDate)
            => await (from erh in _context.ExchangeRateHistories
                      where erh.ExchangeRateDate.Date >= fromDate.Date
                      && erh.ExchangeRateDate <= toDate.Date
                      && erh.IsDownloadSuccess
                      select erh)
            .Include(erh => erh.ExchangeRateHistoryDetails.Where(erhd => erhd.CurrencyCode == currencyCode))
            .ToListAsync();
    }
}
