namespace eCommerceCsv;
/// <summary>
/// This model is used for parsing the csv model
/// </summary>
public record CsvRecordModel
{
    public string Id {get; init; } = string.Empty;
    public string Name {get; init;} = string.Empty;
    public int Quantity {get; init;}
    public float UnitPrice {get; init;}
    public float PercentageDiscount {get; init;}
    public string Buyer {get; init;} = string.Empty;
}
