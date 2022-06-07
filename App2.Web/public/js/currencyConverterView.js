const currencyConverterForm = document.querySelector('form')
const currencyCodeFromInput = document.querySelector('#currencyCodeFrom')
const currencyCodeToInput = document.querySelector('#currencyCodeTo')
const amountInput = document.querySelector('#amount')
const errorMessage = document.querySelector('#errorMessage')
const convertedAmountMessage = document.querySelector('#convertedAmountMessage')

currencyConverterForm.addEventListener('submit', (e) => {
    e.preventDefault()

    errorMessage.textContent = ''
    convertedAmountMessage.textContent = ''

    const currencyCodeFrom = currencyCodeFromInput.value
    const currencyCodeTo = currencyCodeToInput.value
    const amount = amountInput.value

    fetch('/currencyConverter?fromCurrencyCode=' + currencyCodeFrom + '&toCurrencyCode=' + currencyCodeTo + '&amount=' + amount).then((response) => {
        response.json().then((data) => {
            
            if (data.error) {
                errorMessage.textContent = data.error
            }
            else {
                console.log(data)
                convertedAmountMessage.textContent = 'Converted amount is ' + data.convertedAmount
            }
        })
    })
})