namespace adr_tool.Common;
internal static class AdrFile
{
  // _adr_file
  internal static string GetFile(string searchTerm)
  {
    List<string> output = AdrList.FindAdrFiles();
    var matchingLine = output.FirstOrDefault(line => line.Contains(searchTerm));
    return matchingLine;
  }

  // _adr_title
  internal static string GetTitle(string searchTerm)
  {
    string title = GetFile(searchTerm);
    return title.Substring(2);
  }
}
