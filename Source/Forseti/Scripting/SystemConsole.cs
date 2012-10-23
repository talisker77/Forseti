using System;

namespace Forseti.Scripting
{
  public class SystemConsole
  {
    public static bool LoggingEnabled = true;

    public static void Print(string message)
    {
      if (!LoggingEnabled)
        return;

      Console.WriteLine(message);
    }

    public static void ReportFailedCase(string description, string message)
    {
      if (!LoggingEnabled)
        return;

      Console.Write(" Spec( {0} ) ", description);
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write("FAILED "); // added space here to avoid cut of D char in some fonts in the console. 
      Console.ResetColor();
      Console.WriteLine("with message : {0}", message);
    }

    public static void ReportPassedCase(string description)
    {
      if (!LoggingEnabled)
        return;

      Console.Write(" Spec( {0} ) ", description); 
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("PASSED");
      Console.ResetColor();
    }
  }
}
