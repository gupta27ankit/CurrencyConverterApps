const request = require('request')

const currencyConverter = (fromCurrencyCode, toCurrencyCode, amount, callback) => {

    const url = 'https://localhost:7142/api/Currency/GetCurrencyConversionForAmount?fromCurrencyCode=' + fromCurrencyCode + '&toCurrencyCode=' + toCurrencyCode + '&amount=' + amount

    request({ url, json: true, rejectUnauthorized: false }, (error, response) => {

        if (error) {
            callback('Unable to connect with currency converter service', undefined)
        }
        else if (!response.body.success) {
            var message = 'Unable to convert currencies'

            if (response.body.error != null) {
                message = message + ', ' + response.body.error.info
            }
            
            callback(message, undefined)
        }
        else {
            callback(undefined, {
                convertedAmount: response.body.result
            })
        }
    })
}

module.exports = currencyConverter