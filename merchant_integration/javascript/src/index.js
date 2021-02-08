'use strict'

const request = require('request')

const utils = require('./utils')

const pago46_merchant_key = process.env.PAGO46_MERCHANT_KEY
if (!pago46_merchant_key) throw new Error("please set the PAGO46_MERCHANT_KEY environmental variable")

const pago46_merchant_secret  = process.env.PAGO46_MERCHANT_SECRET
if (!pago46_merchant_secret) throw new Error("please set the PAGO46_MERCHANT_SECRET environmental variable")

const pago46_api_host = process.env.PAGO46_API_HOST
if (!pago46_api_host) throw new Error("please set the PAGO46_API_HOST environmental variable")


class Pago46 {
	constructor(){
		this.merchant_key = pago46_merchant_key
		this.merchant_secret = pago46_merchant_secret
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
