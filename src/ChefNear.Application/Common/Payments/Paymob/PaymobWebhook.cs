using System.Text.Json.Serialization;

namespace ChefNear.Application.Common.Payments.Paymob;

public class PaymobWebhook
{
    [JsonPropertyName("obj")]
    public Transaction Transaction { get; set; } = default!;
}

public class Transaction
{
    // HMAC Fields

    [JsonPropertyName("amount_cents")]
    public int AmountCents { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = default!;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;

    [JsonPropertyName("error_occured")]
    public bool ErrorOccured { get; set; }

    [JsonPropertyName("has_parent_transaction")]
    public bool HasParentTransaction { get; set; }

    [JsonPropertyName("id")]
    public long TransactionId { get; set; }

    [JsonPropertyName("integration_id")]
    public long IntegrationId { get; set; }

    [JsonPropertyName("is_3d_secure")]
    public bool Is3DSecure { get; set; }

    [JsonPropertyName("is_auth")]
    public bool IsAuth { get; set; }

    [JsonPropertyName("is_capture")]
    public bool IsCapture { get; set; }

    [JsonPropertyName("is_refunded")]
    public bool IsRefunded { get; set; }

    [JsonPropertyName("is_standalone_payment")]
    public bool IsStandalonePayment { get; set; }

    [JsonPropertyName("is_voided")]
    public bool IsVoided { get; set; }

    [JsonPropertyName("owner")]
    public long Owner { get; set; }

    [JsonPropertyName("pending")]
    public bool Pending { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    // Business Fields

    [JsonPropertyName("captured_amount")]
    public decimal CapturedAmount { get; set; }

    [JsonPropertyName("order")]
    public OrderInfo Order { get; set; } = default!;

    [JsonPropertyName("source_data")]
    public SourceData SourceData { get; set; } = default!;

    [JsonPropertyName("data.message")]
    public string? DataMessage { get; set; }

    [JsonPropertyName("txn_response_code")]
    public string? TxnResponseCode { get; set; }
}

public class OrderInfo
{
    // HMAC Field

    [JsonPropertyName("id")]
    public long OrderId { get; set; }

    // Business Field

    [JsonPropertyName("merchant_order_id")]
    public string? MerchantOrderId { get; set; }
}

public class SourceData
{
    [JsonPropertyName("pan")]
    public string Pan { get; set; } = default!;

    [JsonPropertyName("sub_type")]
    public string SubType { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;
}