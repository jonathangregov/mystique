'use strict'
const nock = require('nock')

const pago46 = require('../src/index').pago46

const url_api_host = 'https://sandboxapi.pago46.com'
// instance the pago46 client.
const client = new pago46(url_api_host)

describe('GET: "/merchant/orders" to get all order of current merchant', () => {
	beforeEach(() => {
		let ordersResponse = [	{
			"id": "7b41ae99-ebdb-4fbc-a1e7-0922d84496f0",
			"price": "1000.00",
			"description": "description testing from Javscript library V1",
			"merchant_order_id": "testJS1",
			"creation_date": "2018-07-19T15:34:38.570499Z",
			"return_url": "http://final.merchant.com",
			"redirect_url": "https://novatest.pago46.com/#UID=7b41ae99-ebdb-4fbc-a1e7-0922d84496f0",
			"status": "successful",
			"timeout": 600,
			"notify_url": "http://merchant.com/app/response"
		}]

		nock('https://sandboxapi.pago46.com')
			.get('/merchant/orders')
			.reply(200, ordersResponse)
	});

	test('should get all order of merchant', async () => {
		return await client.getAllOrders()
		expect(response.statusCode).toBe(200)
		expect(response.body).toBe(ordersResponse)

	})
})

describe('POST: "/merchant/orders" Create a new order', () => {
	const payload = {
		"currency": "CLP",
		"description": "description testing from pythonlibrary from test.Pypi",
		"merchant_order_id": 'testpythonlibrarypypi',
		"notify_url": "http://merchant.com/app/response",
		"price": 1000,
		"return_url": "http://final.merchant.com",
		"timeout": 60
	}

	beforeEach(() => {
		nock('https://sandboxapi.pago46.com')
			.post('/merchant/orders')
			.reply(200, payload)
	})

	test('should create a new order', async () => {
		return await client.createOrder(payload)
		expect(response.statusCode).toBe(200)
		expect(response.body).toBe(payload)

	})
})

describe('POST: "/merchant/complete/" Mark a order as complete', () => {
	const payload = {"order_id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30"}

	beforeEach(() => {
		nock('https://sandboxapi.pago46.com')
			.post('/merchant/complete/')
			.reply(200)
	})

	test('should mark a order as completed', async () => {
		return await client.markOrderAsComplete(payload)
		expect(response.statusCode).toBe(200)

	})
})

describe('GET: "/merchant/order" Retrieve a order by order_id', () => {
	const payload = {"order_id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30"}

	beforeEach(() => {
		const response = {
			"id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30",
			"price": "1500.00",
			"description": "description",
			"merchant_order_id": "000001",
			"creation_date": "2018-05-29T20:42:48.853959Z",
			"return_url": "http://final.merchant.com",
			"redirect_url": "https://novatest.pago46.com/#UID=a6a47f03-d0d3-446b-962d-caf3a7cebb30",
			"status": "expired",
			"timeout": 60,
			"notify_url": "http://notification.merchant.com"
		}
		nock('https://sandboxapi.pago46.com')
			.post('/merchant/order')
			.reply(200, response)
	})

	test('should mark a order as completed', async () => {
		return await client.getOrderById(payload)
		expect(response.statusCode).toBe(200)
		expect(response.body).toBe(payload)
	})
})


describe('GET: "/merchant/notification" Retrieve a order by notification ID', () => {
	const payload = {"order_id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30"}

	beforeEach(() => {
		const response = {
			"id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30",
			"price": "1500.00",
			"description": "description",
			"merchant_order_id": "000001",
			"creation_date": "2018-05-29T20:42:48.853959Z",
			"return_url": "http://final.merchant.com",
			"redirect_url": "https://novatest.pago46.com/#UID=a6a47f03-d0d3-446b-962d-caf3a7cebb30",
			"status": "expired",
			"timeout": 60,
			"notify_url": "http://notification.merchant.com"
		}
		nock('https://sandboxapi.pago46.com')
			.post('/merchant/notification')
			.reply(200, response)
	})

	test('should mark a order as completed', async () => {
		const notification_id = 'a6a47f03-d0d3-446b-962d-caf3a7cebb30'

		return await client.getOrderByNotificationId(notification_id)
		expect(response.statusCode).toBe(200)
		expect(response.body).toBe(payload)
	})
})

describe('GET: "/merchant/order" Retrieve a order DETAIL by order_id', () => {
	const payload = {"order_id": "a6a47f03-d0d3-446b-962d-caf3a7cebb30"}

	beforeEach(() => {
		nock('https://sandboxapi.pago46.com')
			.post('/merchant/123/detail')
			.reply(200)
	})

	test('should mark a order as completed', async () => {
		const notification_id = 'a6a47f03-d0d3-446b-962d-caf3a7cebb30'

		return await client.getOrderDetailsByOrderId(notification_id)
		expect(response.statusCode).toBe(200)
	})
})

