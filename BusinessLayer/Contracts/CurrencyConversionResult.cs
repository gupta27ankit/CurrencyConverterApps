using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public class CurrencyConversionResult
    {
        public bool success { get; set; }
        public Error error { get; set; }
        public DateTime? date { get; set; }
        public decimal result { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }
}
