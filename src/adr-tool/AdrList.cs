using System.Text.RegularExpressions;

using adr;

namespace adr_tool;
internal class AdrList
{
  private static readonly Regex _regex = new Regex(@"^\d+-[^/]*\.md$");

  public static List<string> FindAdrFiles()
  {
    string adrDir = AdrSettings.Current.DocFolder;

    if (string.IsNullOrEmpty(adrDir))
    {
      throw new ArgumentException("Directory path cannot be null or empty.", nameof(adrDir));
    }
    var adrFiles = Directory.EnumerateFiles(adrDir, "*.md", SearchOption.AllDirectories)
        .Where(file => _regex.IsMatch(file))
        .OrderBy(file => file, StringComparer.Ordinal)
        .ToList();
    return adrFiles;
  }
}
