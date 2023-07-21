using System.ComponentModel;
using Vishnu.Interchange;
using WeatherChecker;

namespace WeatherCheckerDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WeatherChecker.WeatherChecker demoChecker = new();
            demoChecker.NodeProgressChanged += SubNodeProgressChanged;
            bool? logicalResult = demoChecker.Run(@"UserParameter", new TreeParameters("MainTree", null), TreeEvent.UndefinedTreeEvent);
            string logicalResultString;
            switch (logicalResult)
            {
                case true: logicalResultString = "true"; break;
                case false: logicalResultString = "false"; break;
                default: logicalResultString = "null"; break;
            }
            Console.WriteLine("logical result: {0}, DataPoints: {1}, Result: {2}",
                logicalResultString, ((WeatherChecker_ReturnObject?)demoChecker.ReturnObject)?.RecordCount,
                demoChecker.ReturnObject?.ToString());
            demoChecker.Dispose();
            Console.ReadLine();
        }

        // Wird vom UserChecker bei Veränderung des Verarbeitungsfortschritts aufgerufen.
        // Wann und wie oft der Aufruf erfolgen soll, wird im UserChecker festgelegt.
        static void SubNodeProgressChanged(object? sender, ProgressChangedEventArgs args)
        {
            Console.WriteLine(args.ProgressPercentage);
        }
    }
}