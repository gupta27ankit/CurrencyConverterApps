using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class Settings
    {
        private static readonly IConfiguration _configuration;

        static Settings()
        {
            if (_configuration is null)
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public static string GetSetting(string settingKey)
        {
            return _configuration.GetSection(settingKey).Value;
        }
    }
}
