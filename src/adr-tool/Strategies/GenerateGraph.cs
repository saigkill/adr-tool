using System.Text.RegularExpressions;

using adr_tool.Common;

namespace adr_tool.Strategies;

// _adr_generate_graph
internal class GenerateGraph : IGenerateStrategy
{
  private string LinkPrefix { get; set; }
  private string LinkExtension { get; set; }

  internal GenerateGraph(string[] args)
  {
    LinkPrefix = String.Empty;
    LinkExtension = ".html";

    for (int i = 0; i < args.Length; i++)
    {
      switch (args[i])
      {
        case "-e":
          if (i + 1 < args.Length)
          {
            LinkExtension = args[++i];
          }
          else
          {
            Console.Error.WriteLine("Option -e requires an argument.");
            Environment.Exit(1);
          }
          break;
        case "-p":
          if (i + 1 < args.Length)
          {
            LinkPrefix = args[++i];
          }
          else
          {
            Console.Error.WriteLine("Option -p requires an argument.");
            Environment.Exit(1);
          }
          break;
        default:
          Console.Error.WriteLine($"Not implemented: {args[i]}");
          Environment.Exit(1);
          break;
      }
    }
  }

  private string Index(string path)
  {
    var fileName = Path.GetFileName(path);
    var index = Regex.Replace(fileName, @"-.*", "");
    return Regex.Replace(index, @"^0*", "");
  }

  public void Build()
  {
    Console.WriteLine("digraph {");
    Console.WriteLine("  node [shape=plaintext];");
    Console.WriteLine("  subgraph {");
    var adrList = AdrList.FindAdrFiles();
    var previousIndex = -1;
    foreach (var f in adrList)
    {
      var n = int.Parse(Index(f));
      var title = AdrFile.GetTitle(f);
      Console.WriteLine($"    _{n} [label=\"{title}\"; URL=\"{LinkPrefix}{Path.GetFileNameWithoutExtension(f)}{LinkExtension}\"];");
      if (previousIndex != -1)
      {
        Console.WriteLine($"    _{previousIndex} -> _{n} [style=\"dotted\", weight=1];");
      }
      previousIndex = n;
    }
    Console.WriteLine("  }");
    foreach (var f in adrList)
    {
      var n = int.Parse(Index(f));
      var links = AdrList.FindAdrFiles();
      foreach (var link in links)
      {
        if (!link.EndsWith(" by"))
        {
          var match = Regex.Match(link, @"^([0-9]+)=(.+)$");
          if (match.Success)
          {
            var targetIndex = match.Groups[1].Value;
            var label = match.Groups[2].Value;
            Console.WriteLine($"  _{n} -> _{targetIndex} [label=\"{label}\", weight=0];");
          }
        }
      }
    }
    Console.WriteLine("}");
  }
}
