using System.Text.RegularExpressions;

namespace adr_tool.Common;

internal static class AdrLinks
{
  internal static void GetLinks(string searchstatus)
  {
    var search = AdrStatus.GetStatus(searchstatus);
    var regex = new Regex(@"^(.+) \[.*\]\(0*([1-9][0-9]*).*\)");
    using (var reader = new StringReader(search))
    {
      string line;
      while ((line = reader.ReadLine()) != null)
      {
        var match = regex.Match(line);
        if (match.Success)
        {
          string result = $"{match.Groups[2].Value}={match.Groups[1].Value}";
          Console.WriteLine(result);
        }
      }
    }
  }
}
