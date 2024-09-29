namespace adr_tool.Strategies;

public class GenerateToc : IGenerateStrategy
{
  private readonly string? _intro;
  private readonly string? _outro;
  private readonly string? _linkPrefix;

  public GenerateToc(string intro, string outro, string linkPrefix)
  {
    _intro = intro;
    _outro = outro;
    _linkPrefix = linkPrefix;
  }

  public void Build()
  {
    using (StreamWriter writer = new StreamWriter(GlobalVariables.AdrFolder + "/toc"))
    {
      writer.WriteLine("# Architecture Decision Records\n");

      if (!string.IsNullOrEmpty(_intro))
      {
        writer.WriteLine(File.ReadAllText(_intro));
        writer.WriteLine();
      }

      var files = Directory.GetFiles(GlobalVariables.AdrFolder, "*.md")
        .Select(f => new { Title = GenerateHelper.GetTitle(f), Link = $"{_linkPrefix}{Path.GetFileName(f)}" });

      foreach (var file in files)
      {
        writer.WriteLine(file.Title + file.Link);
      }

      if (!string.IsNullOrEmpty(_outro))
      {
        writer.WriteLine();
        Console.WriteLine(File.ReadAllText(_outro));
      }
    }
  }
}
