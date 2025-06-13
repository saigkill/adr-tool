using System.Text.RegularExpressions;

namespace adr_tool.Common;

// _adr_remove_status
public static class AdrRemoveStatus
{
  internal static void Remove(string currentStatus, string filePath)
  {
    string tempFile = filePath + ".tmp";
    bool inStatusSection = false;
    bool afterBlank = false;
    using (var reader = new StreamReader(filePath))
    using (var writer = new StreamWriter(tempFile))
    {
      string line;
      while ((line = reader.ReadLine()) != null)
      {
        if (Regex.IsMatch(line, @"^##"))
        {
          inStatusSection = false;
        }
        if (line == "## Status")
        {
          inStatusSection = true;
        }
        if (inStatusSection && string.IsNullOrWhiteSpace(line))
        {
          if (!afterBlank)
          {
            writer.WriteLine(line);
          }
          afterBlank = true;
          continue;
        }
        if (inStatusSection && line == currentStatus)
        {
          continue;
        }
        if (inStatusSection && !string.IsNullOrWhiteSpace(line))
        {
          afterBlank = false;
        }
        writer.WriteLine(line);
      }
    }
    File.Replace(tempFile, filePath, null);
  }
}
