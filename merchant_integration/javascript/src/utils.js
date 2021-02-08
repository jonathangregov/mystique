'use strict'

const crypto = require('crypto')

let get_concatenated_payload = (payload) => {
    let concatenatedParams = ''
    for (let key in payload) {
        concatenatedParams += '&'+ key + '=' + encodeURIComponent(payload[key]);
    }
    return concatenatedParams
}

const sign_request = (method, path, merchant_key, merchant_secret, payload) => {
    let date = + new Date()
    let concatenatedParams = get_concatenated_payload(payload)
	let requestPath = encodeURIComponent(path)
    let encrypt_base = `${merchant_key}&${String(date)}&${method}&${requestPath}${concatenatedParams}`
    const hash = crypto.createHmac('sha256', merchant_secret)
                        .update(encrypt_base)
                        .digest('hex')

    return {
        hash: hash,
        date: String(date)
    }
}

module.exports = {
    sign_request: sign_request
}