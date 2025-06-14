using System.Text.RegularExpressions;

namespace adr_tool.Common;

// _adr_status
internal static class AdrStatus
{
  internal static string GetStatus(string filePath)
  {
    var adrFilePath = AdrFile.GetFile(filePath);
    var lines = File.ReadAllLines(adrFilePath);
    bool inStatusSection = false;
    foreach (var line in lines)
    {
      if (Regex.IsMatch(line, @"^## Status"))
      {
        inStatusSection = true;
        continue;
      }
      if (inStatusSection)
      {
        if (Regex.IsMatch(line, @"^#"))
        {
          inStatusSection = false;
          continue;
        }
        if (!Regex.IsMatch(line, @"^(#|\s*$)"))
        {
          Console.WriteLine(line);
          return line;
        }
      }
    }
    return "No status found.";
  }
}
