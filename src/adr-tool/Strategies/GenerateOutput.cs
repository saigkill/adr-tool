namespace adr_tool.Strategies;

public class GenerateOutput
{
  private IGenerateStrategy Strategy { get; set; } = null!;

  public void OutputStrategy(IGenerateStrategy strategy)
  {
    Strategy = strategy;
  }

  public void Generate()
  {
    if (Strategy == null)
    {
      throw new InvalidOperationException("Strategy not set");
    }

    Strategy.Build();
  }
}
