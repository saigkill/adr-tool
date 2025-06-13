using adr_tool.Common;

namespace adr_tool;

public class AdrLink
{
  internal void Link(string source, string forwardLink, string target, string reverseLink)
  {
    AddAdrLink.LinkAdr(source, forwardLink, target);
    AddAdrLink.LinkAdr(target, reverseLink, source);
  }
}
