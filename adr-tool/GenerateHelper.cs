using System.Text.RegularExpressions;

namespace adr_tool;

public static class GenerateHelper
{
  public static string GetTitle(string filePath)
  {
    var line1 = File.ReadLines(filePath).First();
    var match = Regex.Match(line1, @"# \d+\. (.+)");
    if (match.Success)
    {
      return match.Groups[1].Value;
    }

    return "No title found";
  }
}
