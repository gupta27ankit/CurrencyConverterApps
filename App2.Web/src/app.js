const path = require('path')
const express = require('express')
const hbs = require('hbs')

const currencyConverter = require('./utils/currencyConverter')

const app = express()
const port = process.env.port || 1337

const publicDirectoryPath = path.join(__dirname, '../public')
const viewsPath = path.join(__dirname, '../templates/views')
const partialsPath = path.join(__dirname, '../templates/partials')

app.set('view engine', 'hbs')
app.set('views', viewsPath)
hbs.registerPartials(partialsPath)

app.use(express.static(publicDirectoryPath))

app.get('', (req, res) => {
    res.render('index', {})
})

app.get('/currencyConverter', (req, res) => {

    if (!req.query.fromCurrencyCode) {
        return res.send({
            error: 'enter from currency code'
        })
    }

    if (!req.query.toCurrencyCode) {
        return res.send({
            error: 'enter to currency code'
        })
    }

    if (!req.query.amount) {
        return res.send({
            error: 'enter amount'
        })
    }

    currencyConverter(req.query.fromCurrencyCode, req.query.toCurrencyCode, req.query.amount, (error, conversionResult) => {
        if (error) {
            return res.send({ error })
        }

        res.send({ convertedAmount: conversionResult.convertedAmount })
    })
})

app.listen(port, () => {
    console.log('Server is up on port ' + port)
})