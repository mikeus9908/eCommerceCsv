namespace eCommerceCsv;
/// <summary>
/// This model is used for parsing the csv model
/// </summary>
public record CsvRecordModel
{
    public int Id {get; init; }
    public string Name {get; init;} = string.Empty;
    public int Quantity {get; init;}
    public float UnitPrice {get; init;}
    public float PercentageDiscount {get; init;}
    public string Buyer {get; init;} = string.Empty;
}
