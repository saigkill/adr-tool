using adr_tool.Strategies;

namespace adr_tool;

public class GenerateOutput
{
  private IGenerateStrategy Strategy { get; set; }

  public void OutputStrategy(IGenerateStrategy strategy)
  {
    this.Strategy = strategy;
  }

  public void Generate()
  {
    if (this.Strategy == null)
    {
      throw new InvalidOperationException("Strategy not set");
    }

    this.Strategy.Build();
  }
}
