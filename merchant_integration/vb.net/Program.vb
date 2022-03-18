Imports System.Net
Imports System.Net.Http
Imports System.Net.Security
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports Newtonsoft

Class Program
    Structure HashResult
        Public hash As String
        Public pdate As Long
    End Structure

    Private Shared merchantKey As String
    Private Shared merchantSecret As String
    Private Shared providerKey As String
    Private Shared providerSecret As String

    Public Shared Sub Main(ByVal args As String())
        merchantKey = Environment.GetEnvironmentVariable("MERCHANT_KEY")
        merchantSecret = Environment.GetEnvironmentVariable("MERCHANT_SECRET")
        providerKey = Environment.GetEnvironmentVariable("PROVIDER_KEY")
        providerSecret = Environment.GetEnvironmentVariable("PROVIDER_SECRET")

        Console.WriteLine("Enter order number...")
        Dim orderNumber As String = Console.ReadLine()

        Try
            Console.WriteLine("Creating order...")
            Dim order As CreateOrderResponse = CreateOrderAsync(orderNumber).GetAwaiter().GetResult()
            Console.WriteLine(Json.JsonConvert.SerializeObject(order))
            Console.WriteLine("Press any key")
            Console.ReadKey()

            Try
                Console.WriteLine("Getting order by notification id...")
                Dim orderByNotification As GetOrderByNotificationIdResponse = GetOrderByNotificationIdAsync().GetAwaiter().GetResult()
                Console.WriteLine(Json.JsonConvert.SerializeObject(orderByNotification))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Console.WriteLine("Press any key")
            Console.ReadKey()

            Try
                Console.WriteLine("Getting order by id...")
                Dim orderById = GetOrderByIdAsync(order.id).GetAwaiter().GetResult()
                Console.WriteLine(Json.JsonConvert.SerializeObject(orderById))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Console.WriteLine("Press any key")
            Console.ReadKey()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine("Press any key")
            Console.ReadKey()
        End Try

        Console.WriteLine("Enter cashout order number...")
        Dim cashOutOrderNumber As String = Console.ReadLine()

        Try
            Console.WriteLine("Creating cashout order...")
            Dim cashOutOrder As CreateCashOutOrderResponse = CreateCashOutOrderAsync(cashOutOrderNumber).GetAwaiter().GetResult()
            Console.WriteLine(Json.JsonConvert.SerializeObject(cashOutOrder))
            Console.WriteLine("Press any key")
            Console.ReadKey()

            Try
                Console.WriteLine("Getting cashout order by id...")
                Dim cashOutOrderById = GetCashOutOrderByIdAsync(cashOutOrder.id).GetAwaiter().GetResult()
                Console.WriteLine(Json.JsonConvert.SerializeObject(cashOutOrderById))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Console.WriteLine("Press any key")
            Console.ReadKey()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine("Press any key")
            Console.ReadKey()
        End Try

        Console.WriteLine("Enter debt code...")
        Dim debtCode As String = Console.ReadLine()

        Try
            Console.WriteLine("Getting debt verification...")
            Dim debtVerification As GetDebtVerificationResponse = GetDebtVerificationAsync(debtCode).GetAwaiter().GetResult()
            Console.WriteLine(Json.JsonConvert.SerializeObject(debtVerification))

            Try
                Console.WriteLine("Confirming payment...")
                Dim confirmPayment As ConfirmPaymentResponse = ConfirmPaymentAsync(debtVerification.code).GetAwaiter().GetResult()
                Console.WriteLine(Json.JsonConvert.SerializeObject(confirmPayment))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine("Press any key")
            Console.ReadKey()
        End Try

        Try
            Console.WriteLine("Checking user...")
            Dim checkUser As CheckUserResponse = CheckUserAsync().GetAwaiter().GetResult()
            Console.WriteLine(Json.JsonConvert.SerializeObject(checkUser))

            Try
                Console.WriteLine("Notifying transaction...")
                Dim notifyTransaction As NotifyTransactionResponse = NotifyTransactionAsync(checkUser.message.transaction_uuid).GetAwaiter().GetResult()
                Console.WriteLine(Json.JsonConvert.SerializeObject(notifyTransaction))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine("Press any key")
            Console.ReadKey()
        End Try

        Console.WriteLine("Press any key")
        Console.ReadKey()
    End Sub

    Private Shared Function ValidateServerCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Shared Async Function NotifyTransactionAsync(uuid As String) As Task(Of NotifyTransactionResponse)
        Dim payload As KeyValuePair(Of String, String)() = {
            New KeyValuePair(Of String, String)("amount", "100.00"),
            New KeyValuePair(Of String, String)("bank_country_currency_id", "599d801a-8c6b-4c87-9c7a-917a9b8d59c6"),
            New KeyValuePair(Of String, String)("bank_transaction_id", "1234567890"),
            New KeyValuePair(Of String, String)("external_contract", ""),
            New KeyValuePair(Of String, String)("status", "processed")
        }
        Dim formContent = New FormUrlEncodedContent(payload)
        Dim hashResult As HashResult = GenerateProviderHash("POST", "/notify/" + uuid, payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.PostAsync("notify/" + uuid, formContent)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of NotifyTransactionResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function CheckUserAsync() As Task(Of CheckUserResponse)
        Dim payload As KeyValuePair(Of String, String)() = {
            New KeyValuePair(Of String, String)("amount", "100.00"),
            New KeyValuePair(Of String, String)("bank_country_currency_id", "599d801a-8c6b-4c87-9c7a-917a9b8d59c6"),
            New KeyValuePair(Of String, String)("bank_transaction_id", "1234567890"),
            New KeyValuePair(Of String, String)("beneficiary_email", "beneficiary@email.com"),
            New KeyValuePair(Of String, String)("beneficiary_name", "beneficiary_name"),
            New KeyValuePair(Of String, String)("creation_date", DateTime.Now.ToString("Ymd")),
            New KeyValuePair(Of String, String)("description", ""),
            New KeyValuePair(Of String, String)("document_id", "11111111"),
            New KeyValuePair(Of String, String)("external_contract", ""),
            New KeyValuePair(Of String, String)("extra_document_id", "11111111"),
            New KeyValuePair(Of String, String)("payer_email", "payer@email.com"),
            New KeyValuePair(Of String, String)("external_contract", ""),
            New KeyValuePair(Of String, String)("payer_name", "payer_name"),
            New KeyValuePair(Of String, String)("payment_method", "bank_transfer"),
            New KeyValuePair(Of String, String)("tax_amount", "")
        }
        Dim formContent = New FormUrlEncodedContent(payload)
        Dim hashResult As HashResult = GenerateProviderHash("POST", "/check", payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.PostAsync("check", formContent)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of CheckUserResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function ConfirmPaymentAsync(code As String) As Task(Of ConfirmPaymentResponse)
        Dim payload As KeyValuePair(Of String, String)() = {
            New KeyValuePair(Of String, String)("status", "complete")
        }
        Dim formContent = New FormUrlEncodedContent(payload)
        Dim hashResult As HashResult = GenerateProviderHash("PUT", "/payments/provider/notify/" + code, payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("provider-key", providerKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.PutAsync("payments/provider/notify/" + code, formContent)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of ConfirmPaymentResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function GetDebtVerificationAsync(code As String) As Task(Of GetDebtVerificationResponse)
        Dim payload As KeyValuePair(Of String, String)() = {}
        Dim hashResult As HashResult = GenerateProviderHash("GET", "/payments/provider/check/" + code, payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("provider-key", providerKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.GetAsync("payments/provider/check/" + code)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of GetDebtVerificationResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function GetCashOutOrderByIdAsync(id As String) As Task(Of GetCashOutOrderByIdResponse)
        Dim payload As KeyValuePair(Of String, String)() = {}
        Dim hashResult As HashResult = GenerateMerchatHash("GET", "/cashout/order/" + id, payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.GetAsync("cashout/order/" + id)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of GetCashOutOrderByIdResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function CreateCashOutOrderAsync(orderNumber As String) As Task(Of CreateCashOutOrderResponse)
        Dim payload As KeyValuePair(Of String, String)() = {
            New KeyValuePair(Of String, String)("amount", "1500"),
            New KeyValuePair(Of String, String)("currency", "ARS"),
            New KeyValuePair(Of String, String)("country_code", "ARG"),
            New KeyValuePair(Of String, String)("description", "description"),
            New KeyValuePair(Of String, String)("merchant_order_id", orderNumber),
            New KeyValuePair(Of String, String)("notify_url", "http://notification.merchant.com"),
            New KeyValuePair(Of String, String)("return_url", "http://final.merchant.com"),
            New KeyValuePair(Of String, String)("timeout", "60")
        }
        Dim formContent = New FormUrlEncodedContent(payload)
        Dim hashResult As HashResult = GenerateMerchatHash("POST", "/cashout/order/", payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.PostAsync("cashout/order/", formContent)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of CreateCashOutOrderResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function GetOrderByIdAsync(id As String) As Task(Of GetOrderByIdResponse)
        Dim payload As KeyValuePair(Of String, String)() = {}
        Dim hashResult As HashResult = GenerateMerchatHash("GET", "/merchant/order/" + id, payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.GetAsync("merchant/order/" + id)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of GetOrderByIdResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function GetOrderByNotificationIdAsync() As Task(Of GetOrderByNotificationIdResponse)
        Dim payload As KeyValuePair(Of String, String)() = {}
        Dim hashResult As HashResult = GenerateMerchatHash("GET", "/merchant/notification/6cbddcac-67c4-451e-b414-aaa8b5d26117", payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.GetAsync("merchant/notification/6cbddcac-67c4-451e-b414-aaa8b5d26117")
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of GetOrderByNotificationIdResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Async Function CreateOrderAsync(orderNumber As String) As Task(Of CreateOrderResponse)
        Dim payload As KeyValuePair(Of String, String)() = {
            New KeyValuePair(Of String, String)("currency", "CLP"),
            New KeyValuePair(Of String, String)("description", "description"),
            New KeyValuePair(Of String, String)("merchant_order_id", orderNumber),
            New KeyValuePair(Of String, String)("notify_url", "http://notification.merchant.com"),
            New KeyValuePair(Of String, String)("price", "1500"),
            New KeyValuePair(Of String, String)("return_url", "http://final.merchant.com"),
            New KeyValuePair(Of String, String)("timeout", "60")
        }
        Dim formContent = New FormUrlEncodedContent(payload)
        Dim hashResult As HashResult = GenerateMerchatHash("POST", "/merchant/orders/", payload)

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri("https://sandboxapi.pago46.com/")
        client.DefaultRequestHeaders.Add("merchant-key", merchantKey)
        client.DefaultRequestHeaders.Add("message-hash", hashResult.hash)
        client.DefaultRequestHeaders.Add("message-date", hashResult.pdate.ToString())

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim response As HttpResponseMessage = Await client.PostAsync("merchant/orders/", formContent)
        Dim response_contents As String = Await response.Content.ReadAsStringAsync()
        If (response.IsSuccessStatusCode) Then
            Return Json.JsonConvert.DeserializeObject(Of CreateOrderResponse)(response_contents)
        Else
            Throw New Exception(response_contents)
        End If
    End Function

    Private Shared Function GenerateMerchatHash(ByVal requestMethod As String, ByVal requestPath As String, ByVal payload As KeyValuePair(Of String, String)()) As HashResult
        payload.OrderBy(Function(x) x)
        Dim concatenatedParams As String = ""

        For Each data As KeyValuePair(Of String, String) In payload
            Dim escapedValue As String = Uri.EscapeDataString(data.Value)
            concatenatedParams += "&"c & data.Key & "="c & escapedValue
        Next

        Dim pdate As Long = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        Dim encrypt_base As String = merchantKey & "&"c & pdate & "&"c & requestMethod & "&"c & Uri.EscapeDataString(requestPath) & concatenatedParams
        Dim encoding As ASCIIEncoding = New ASCIIEncoding()
        Dim key As Byte() = encoding.GetBytes(merchantSecret)
        Dim encrypt_base_bytes As Byte() = encoding.GetBytes(encrypt_base)

        Using hmacSHA256 = New HMACSHA256(key)
            Dim hashmessage As Byte() = hmacSHA256.ComputeHash(encrypt_base_bytes)
            Dim computed_hash As String = BitConverter.ToString(hashmessage).Replace("-", "").ToLower()
            Return New HashResult With {
                .hash = computed_hash,
                .pdate = pdate
            }
        End Using
    End Function

    Private Shared Function GenerateProviderHash(ByVal requestMethod As String, ByVal requestPath As String, ByVal payload As KeyValuePair(Of String, String)()) As HashResult
        payload.OrderBy(Function(x) x)
        Dim concatenatedParams As String = ""

        For Each data As KeyValuePair(Of String, String) In payload
            Dim escapedValue As String = Uri.EscapeDataString(data.Value)
            concatenatedParams += "&"c & data.Key & "="c & escapedValue
        Next

        Dim pdate As Long = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        Dim encrypt_base As String = providerKey & "&"c & pdate & "&"c & requestMethod & "&"c & Uri.EscapeDataString(requestPath) & concatenatedParams
        Dim encoding As ASCIIEncoding = New ASCIIEncoding()
        Dim key As Byte() = encoding.GetBytes(providerSecret)
        Dim encrypt_base_bytes As Byte() = encoding.GetBytes(encrypt_base)

        Using hmacSHA256 = New HMACSHA256(key)
            Dim hashmessage As Byte() = hmacSHA256.ComputeHash(encrypt_base_bytes)
            Dim computed_hash As String = BitConverter.ToString(hashmessage).Replace("-", "").ToLower()
            Return New HashResult With {
                .hash = computed_hash,
                .pdate = pdate
            }
        End Using
    End Function
End Class
