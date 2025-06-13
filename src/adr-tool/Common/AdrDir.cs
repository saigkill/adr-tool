using System.Diagnostics;

namespace adr_tool.Common;

// _adr_dir
internal class AdrDir
{
  private string reldir = ".";
  private string MkRel(string path)
  {
    var d = Path.Combine(reldir, path);
    return d.StartsWith("./") ? d.Substring(2) : d;
  }
  private string AbsDir(string path)
  {
    var startInfo = new ProcessStartInfo
    {
      FileName = "cmd.exe",
      Arguments = $"/c \"cd /d {Path.GetDirectoryName(path)} && cd && cd\"",
      RedirectStandardOutput = true,
      UseShellExecute = false,
      CreateNoWindow = true
    };
    using (var process = Process.Start(startInfo))
    {
      if (process == null) throw new InvalidOperationException("Failed to start process.");
      using (var reader = process.StandardOutput)
      {
        return reader.ReadToEnd().Trim();
      }
    }
  }
  public void FindAdrDirectory()
  {
    while (AbsDir(reldir) != "/")
    {
      if (File.Exists(MkRel(".adr-dir")))
      {
        Console.WriteLine(MkRel(File.ReadAllText(MkRel(".adr-dir")).Trim()));
        return;
      }
      else if (Directory.Exists(MkRel("doc/adr")))
      {
        Console.WriteLine(MkRel("doc/adr"));
        return;
      }
      else
      {
        reldir = Path.Combine(reldir, "..");
      }
    }
    Console.WriteLine("doc/adr");
  }
}
