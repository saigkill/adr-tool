namespace adr_tool.Common;

// _adr_add_link
public static class AddAdrLink
{
  internal static void LinkAdr(string source, string linkType, string target)
  {
    var adrBinDir = AdrSettings.Current.DocFolder;

    var sourceAdr = AdrFile.GetFile(source);
    var targetAdr = AdrFile.GetFile(target);
    var targetTitle = AdrFile.GetTitle(target);
    var lines = File.ReadAllLines(source);
    using (var writer = new StreamWriter(source + ".tmp"))
    {
      var inStatusSection = false;
      foreach (var line in lines)
      {
        if (line.StartsWith("##"))
        {
          if (inStatusSection)
          {
            writer.WriteLine($"{linkType} [{targetTitle}]({Path.GetFileName(target)})");
            writer.WriteLine();
          }
          inStatusSection = false;
        }
        if (line.StartsWith("## Status"))
        {
          inStatusSection = true;
        }
        writer.WriteLine(line);
      }
    }
    File.Replace(source + ".tmp", source, null);
  }
}
