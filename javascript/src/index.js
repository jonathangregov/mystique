'use strict'

const request = require('request')

const utils = require('./utils')


class Pago46 {
	constructor(pago46_api_host){
		this.merchant_key = process.env.MERCHANT_KEY
		this.merchant_secret = process.env.MERCHANT_SECRET
		this.pago46_api_host = pago46_api_host
		// urls
		this.url_merchant_orders = '/merchant/orders'
		this.url_merchant_order = '/merchant/order'
		this.url_merchant_complete = '/merchant/complete/'
		this.url_merchant_notification = '/merchant/notification'
	}

	getAllOrders() {
		const method = 'GET'
		const payload = {}
		const sing_result = utils.sign_request(method, this.url_merchant_orders, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url =  `${this.pago46_api_host}${this.url_merchant_orders}`

		const options = {
			url: url,
			method: method,
			headers: headers
		}
		request(options, (error, response) =>{
			return response

		})
	}
	createOrder(payload){
		const method= "POST"
		const sing_result = utils.sign_request(method, this.url_merchant_orders, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url =  `${this.pago46_api_host}${this.url_merchant_orders}`
		const options = {
			url: url,
			method: method,
			headers: headers,
            form: payload
		}

		request(options, (error, response) =>{
			return response
		})

	}
	markOrderAsComplete(payload){
		const method = 'POST'
		const sing_result = utils.sign_request(method, this.url_merchant_complete, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url =  `${this.pago46_api_host}${this.url_merchant_complete}`
		const options = {
			url: url,
			method: method,
			headers: headers,
			form: payload
		}
		request(options, (error, response) =>{
			return response
		})

	}
	getOrderById(order_id){
		const method= 'GET'
		const path = `${this.url_merchant_order}/${order_id}`
        const payload = {}
		const sing_result = utils.sign_request(method, path, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url = `${this.pago46_api_host}${this.url_merchant_order}/${order_id}`
		const options = {
			url: url,
			method: method,
			headers: headers,
		}
		request(options, (error, response) =>{
				return response
		})
	}
	getOrderByNotificationId(notification_id){
		const method = 'GET'
		const path = `${this.url_merchant_notification}/${notification_id}`
		const payload = {}
		const sing_result = utils.sign_request(method, path, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url = `${this.pago46_api_host}${this.url_merchant_notification}/${notification_id}`
		const options = {
			url: url,
			method: method,
			headers: headers,
		}
		request(options, (error, response) =>{
				return response
		})
	}
	getOrderDetailsByOrderId(order_id){
		const method = "GET"
		const path = `${this.url_merchant_order}/${order_id}/detail`
		const payload = {}
		const sing_result = utils.sign_request(method, path, this.merchant_key, this.merchant_secret, payload)
		const hash = sing_result['hash']
		const date = sing_result['date']
		const headers = {
			'merchant-key': this.merchant_key,
			'message-hash': hash,
			'message-date': date
		}
		const url = `${this.pago46_api_host}${this.url_merchant_order}/${order_id}/detail`
		const options = {
			url: url,
			method: method,
			headers: headers,
		}

		request(options, (error, response) =>{
				return response
		})
	}

}

module.exports = {
	pago46: Pago46
}
