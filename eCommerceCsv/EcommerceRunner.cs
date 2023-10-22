namespace eCommerceCsv;

public class EcommerceRunner
{
    public static void Main (string[] args)
    {
        Console.WriteLine(" Starting eCommerceConsoleApp ".PadLeft(50, '=').PadRight(75,'='));
        // reading filename as an argument
        if(args.Length != 1){
            LogError("Not enough args: You need to provide a filename.");
            return;
        }

        FileInfo fileInfo = new FileInfo(args[0]);
        if(fileInfo.Exists == false){
            LogError($"Invalid filename {fileInfo.FullName}, please enter a valid one");
            return;
        }

    }

    private static void LogError(string message){
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
