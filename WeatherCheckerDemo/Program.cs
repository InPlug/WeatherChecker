using NetEti.Globals;
using System;
using Vishnu.Interchange;

namespace Vishnu_UserModules
{
    /// <summary>
    /// Demoprogramm für den WeatherChecker.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            TreeEvent treeEvent = null;

            // Der nachfolgende Teil ist zur Demonstration des Zugriffs auf das Vishnu-Environment und 
            // der Übergabe dort gespeicherter Results von möglichen Vorläufern des aktuellen Checkers.
            // Checker benötigen normalerweise keinen Zugriff auf die Ergebnisse von Vorläuferknoten.
            // Dann reicht hier eine Übergabe von null als TreeEvent.
            WeatherChecker_ReturnObject demoReturnObject = new WeatherChecker_ReturnObject();
            treeEvent = new TreeEvent("True", "Predecessor", "Sender", "SenderName", "./SenderPath",
                                                true, NodeLogicalState.Done,
              new ResultDictionary() { { "Predecessor",
                                   new Result("Main", true, NodeState.Finished, NodeLogicalState.Done, demoReturnObject) }  },
              new ResultDictionary() { { "Predecessor",
                                   new Result("Main", true, NodeState.Finished, NodeLogicalState.Done, demoReturnObject) } });

            // Der nachfolgende Teil reicht für den Aufruf eines einfachen Checkers:
            WeatherChecker demoChecker = new WeatherChecker();
            demoChecker.NodeProgressChanged += SubNodeProgressChanged;
            bool? logicalResult = demoChecker.Run(@"UserParameter", new TreeParameters("MainTree", null), treeEvent);
            string logicalResultString;
            switch (logicalResult)
            {
                case true: logicalResultString = "true"; break;
                case false: logicalResultString = "false"; break;
                default: logicalResultString = "null"; break;
            }
            Console.WriteLine("logical result: {0}, DataPoints: {1}, Result: {2}",
                logicalResultString, (demoChecker.ReturnObject as WeatherChecker_ReturnObject).RecordCount,
                demoChecker.ReturnObject.ToString());
            demoChecker.Dispose();
            Console.ReadLine();
        }

        // Wird vom UserChecker bei Veränderung des Verarbeitungsfortschritts aufgerufen.
        // Wann und wie oft der Aufruf erfolgen soll, wird im UserChecker festgelegt.
        static void SubNodeProgressChanged(object sender, CommonProgressChangedEventArgs args)
        {
            Console.WriteLine("{0} of {1}", args.CountSucceeded, args.CountAll);
        }
    }
}
