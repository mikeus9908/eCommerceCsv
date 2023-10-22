namespace eCommerceCsv;

public class EcommerceRunner
{
    public static void Main (string[] args)
    {
        Console.WriteLine(" Starting eCommerceConsoleApp ".PadLeft(50, '=').PadRight(75,'='));
        // reading filename as an argument
        /*if(args.Length != 1){
            LogError("Not enough args: You need to provide a filename.");
            return;
        }*/

        FileInfo fileInfo = new FileInfo("C:\\Users\\mikeu\\Documents\\progettiIncompleti\\eCommerceCsv\\eCommerceCsv\\eCommerceList.csv");
        if(fileInfo.Exists == false){
            LogError($"Invalid filename {fileInfo.FullName}, please enter a valid one");
            return;
        }

        TryReadCsvFile(fileInfo, out IEnumerable<CsvRecordModel> records);

        // the whole file has been read. Now I can extract any information
        CsvRecordModel maximumPriceRecord = records.OrderByDescending(record => record.UnitPrice * record.Quantity).First();
        CsvRecordModel maximumQuantity = records.OrderByDescending(record => record.Quantity).First();
        CsvRecordModel maximumDiscount = records.Where(record => record.PercentageDiscount > 0).OrderByDescending(record => (record.UnitPrice * record.Quantity) - record.UnitPrice * record.Quantity * record.PercentageDiscount).First();
        Log("Results:");
        Log($"Maximum price found = {maximumPriceRecord.UnitPrice * maximumPriceRecord.Quantity:C2}");
        LogRecord(maximumPriceRecord);
        Log($"Maximum quantity found = {maximumQuantity.Quantity}");
        LogRecord(maximumQuantity);
        Log($"Maximum discount found = {(maximumDiscount.UnitPrice * maximumDiscount.Quantity) - maximumDiscount.UnitPrice * maximumDiscount.Quantity * maximumDiscount.PercentageDiscount:C2}");
        LogRecord(maximumDiscount);

    }

#region CSV PARSING
private static bool TryReadCsvFile(FileInfo csvFile, out IEnumerable<CsvRecordModel> ecommerceRecords)
{
    bool result = false;
    List<CsvRecordModel> recordList = new List<CsvRecordModel>();

    try
    {
        // reading file lines
        using StreamReader reader = csvFile.OpenText();
        // skip the header
        reader.ReadLine();
        // reading the file line by line
        while (reader.ReadLine() is string line)
        {
            if(TryParseLine(line, out CsvRecordModel newRecord) == false)
            {
                Log("Skipping line", ConsoleColor.DarkYellow);
                continue;
            }

            recordList.Add(newRecord);
        }

        result = true;
    }
    catch (IOException)
    {
        LogError("Cannot read the requested file. Exiting...");
        result = false;
    }
    catch (Exception ex)
    {
        LogError($"Unexpected exception: {ex.Message}");
        result = false;
    }
    finally
    {
        ecommerceRecords = recordList.AsEnumerable();
    }

    return result;
}

/// <summary>
/// Parsing a single Line and generating a csvRecord in case of success
/// </summary>
/// <param name="line"></param>
/// <returns></returns>
private static bool TryParseLine(string line, out CsvRecordModel record)
{
    // empty record in case parsing is not completed
    record = new CsvRecordModel();
    // the line should be splitted in 6 comma separated fields
    string[] fields = line.Split(',');
    if (fields.Length != 6)
    {
        Log($"Found a line that cannot be parsed: {line}", ConsoleColor.DarkYellow);
        return false;
    }

    string id = fields[0].Trim();
    string name = fields[1].Trim();

    if (int.TryParse(fields[2], out int quantity) == false)
    {
        Log("The quantity is not an integer", ConsoleColor.DarkYellow);
        return false;
    }

    if (float.TryParse(fields[3], out float unitPrice) == false)
    {
        Log("The unitPrice cannot be parsed", ConsoleColor.DarkYellow);
        return false;
    }

    if (float.TryParse(fields[4], out float percentageDiscount) == false)
    {
        Log("The discount cannot be parsed", ConsoleColor.DarkYellow);
        return false;
    }

    string buyer = fields[5].Trim();

    record = new CsvRecordModel(){
        Id = id,
        Name = name,
        Quantity = quantity,
        UnitPrice = unitPrice,
        PercentageDiscount = percentageDiscount/100,
        Buyer = buyer
    };
    
    return true;
}

#endregion

#region UTILS

    private static void LogRecord(CsvRecordModel record)
    {
        Log($"{record.Id, 4} {record.Name} {record.Quantity} {record.UnitPrice:C2} {record.PercentageDiscount:P} {record.Buyer}");
    }

    /// <summary>
    /// Useful Log wrapper specific for errors
    /// </summary>
    /// <param name="message"></param>
    private static void LogError(string message) => Log(message, ConsoleColor.DarkRed);
    private static void Log(string message, ConsoleColor? color = null){

        if (color is ConsoleColor newForeground)
            Console.ForegroundColor = newForeground;
        Console.WriteLine(message);
        Console.ResetColor();
    }
#endregion
}
