using System;
using System.Collections.Generic;
using namespace LL1Grammar;
public partial class LL1Grammar
{
    public LL1Grammar()
    {
        Productions = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> Productions { get; }



    public void AddProduction(string nonTerminal, List<string> production)
    {
        if (!Productions.ContainsKey(nonTerminal))
        {
            Productions[nonTerminal] = new List<string>();
        }
        Productions[nonTerminal].AddRange(production);
    }

    private bool IsTerminal(char symbol)
    {
        return char.IsLower(symbol);
    }

    private bool IsNonTerminal(char symbol)
    {
        return char.IsUpper(symbol);
    }
}
