using Newtonsoft.Json;

namespace adr_tool;

public sealed class AdrSettings
{
  private const string DefaultFileName = "adr.config.json";

  private static AdrSettings _instance = new AdrSettings();

  private AdrSettings()
  {
  }

  public static AdrSettings Current
  {
    get
    {
      if (_instance == null)
      {
        _instance = Read(new AdrSettings());
      }

      return _instance;
    }
  }

  public string DocFolder { get; set; } = string.Empty;

  public string TemplateFolder { get; set; } = string.Empty;

  public AdrSettings Write()
  {
    using (var stream = File.CreateText(DefaultFileName))
    {
      var value = new
      {
        path = this.DocFolder,
        templates = this.TemplateFolder
      };
      var serializer = new JsonSerializer
      {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
      };
      serializer.Serialize(stream, value);
    }

    return this;
  }

  private static AdrSettings Read(AdrSettings settings)
  {
    if (!File.Exists(DefaultFileName))
    {
      settings.DocFolder = "docs\\adr";
      settings.TemplateFolder = "";
      return settings;
    }

    using var stream = File.OpenText(DefaultFileName);
    var serializer = new JsonSerializer
    {
      Formatting = Formatting.Indented,
      NullValueHandling = NullValueHandling.Ignore
    };

    var value = serializer.Deserialize(stream, typeof(object)) as dynamic;
    settings.DocFolder = value?.path ?? string.Empty;
    settings.TemplateFolder = value?.template ?? string.Empty;
    return settings;
  }
}
