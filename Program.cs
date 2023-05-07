using McMaster.Extensions.CommandLineUtils;

namespace Days;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            DataHandler.GetEvents();
        }
        catch (Exception)
        {
            System.Console.WriteLine("Could not find days.csv, creating a new one...");
        }
        CommandLineApplication.Execute<Days>(args);
    }
}
