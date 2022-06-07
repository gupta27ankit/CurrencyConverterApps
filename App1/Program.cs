using BusinessLayer;
using BusinessLayer.Contracts;

await ShowMenu();

async Task ShowMenu()
{
    Console.WriteLine("Press 1 - Convert amount into currency with latest exchange rate");
    Console.WriteLine("Press 2 - Convert amount into currency with exchange rate on a specified date");
    Console.WriteLine("Press 3 - Download currency exchange rates for current date");
    Console.WriteLine("Press 4 - Exit \n");

    var action = Console.ReadLine();

    switch (action)
    {
        case "1":
            await CurrencyConverionForAmount(false);
            await ShowMenu();
            break;
        case "2":
            await CurrencyConverionForAmount(true);
            await ShowMenu();
            break;
        case "3":
            await DownloadCurrecyExchangeRates();
            break;
        case "4":
            return;
        default:
            Console.WriteLine("Unsupported action  \n");
            Console.ReadKey();
            await ShowMenu();
            break;
    }
}

async Task CurrencyConverionForAmount(bool withExchangeRateOnDate)
{
    string currencyCodeFrom, currencyCodeTo;
    double amount;
    DateTime exchangeRateDate;

    var _currencyBusiness = new Currency();
    CurrencyConversionResult convertResult;

    do
    {
        Console.WriteLine("Enter currency code from: ");
        currencyCodeFrom = Console.ReadLine().Trim();
    }
    while (string.IsNullOrEmpty(currencyCodeFrom));

    do
    {
        Console.WriteLine("Enter currency code to: ");
        currencyCodeTo = Console.ReadLine().Trim();
    }
    while (string.IsNullOrEmpty(currencyCodeTo));

    do
    {
        Console.WriteLine("Enter amount: ");
    }
    while (!double.TryParse(Console.ReadLine().Trim(), out amount));

    if (withExchangeRateOnDate)
    {
        do
        {
            Console.WriteLine("Enter date for exchange rate (YYYY-MM-DD): ");
        }
        while (!DateTime.TryParse(Console.ReadLine().Trim(), out exchangeRateDate));

        convertResult = await _currencyBusiness.CurrencyConversionForAmountAsync(currencyCodeFrom, currencyCodeTo, amount, exchangeRateDate);
    }
    else
        convertResult = await _currencyBusiness.CurrencyConversionForAmountAsync(currencyCodeFrom, currencyCodeTo, amount);

    if (IsConverted(convertResult))
    {
        if (withExchangeRateOnDate)
            Console.WriteLine($"{amount} {currencyCodeFrom} = {convertResult.result:N2} {currencyCodeTo} as per conversion rate on {convertResult.date.Value:yyyy-MM-dd}  \n");
        else
            Console.WriteLine($"{amount} {currencyCodeFrom} = {convertResult.result:N2} {currencyCodeTo} as per latest conversion rate \n");
    }

    Console.ReadKey();
}

bool IsConverted(CurrencyConversionResult convertResult)
{
    if (convertResult is null)
        return false;

    if (!convertResult.success)
    {
        Console.WriteLine($"ERROR! ErrorType: {convertResult.error?.type}, ErrorInfo: {convertResult.error?.info} \n");
        return false;
    }

    return true;
}

async Task DownloadCurrecyExchangeRates()
{
    var _currencyBusiness = new Currency();
    var result = await _currencyBusiness.DownloadCurrencyExchangeRatesAsync();
    Console.WriteLine("Download: " + result.ToString());

    Console.ReadKey();
    await ShowMenu();
}