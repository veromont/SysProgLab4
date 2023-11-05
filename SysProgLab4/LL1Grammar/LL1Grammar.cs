namespace SysProgLab4.LL1Grammar;
public struct Rule
{
    public Rule(string SymbolFrom, string Production)
    {
        this.SymbolFrom = SymbolFrom;
        this.Production = Production;
    }
    public string SymbolFrom { get; set; }
    public string Production { get; set; }
    public override string ToString()
    {
        return $"{SymbolFrom} => {Production}";
    }
}

public partial class LL1Grammar
{
    public LL1Grammar(string startNonTerminal)
    {
        RulesDictionary = new Dictionary<string, List<string>>();
        Terminals = new List<string>();
        NonTerminals = new List<string>();
        StartNonTerminal = startNonTerminal;
        NonTerminals.Add(StartNonTerminal);
        EpsilonNonTerminals = new HashSet<string>();
        RulesList = new List<Rule>();
    }

    public Dictionary<string, List<string>> RulesDictionary { get; }
    public List<Rule> RulesList { get; }
    public List<string> Terminals { get; }
    public List<string> NonTerminals { get; }
    public string? Epsilon { get; set; }
    public string StartNonTerminal { get; }
    public HashSet<string> EpsilonNonTerminals { get; private set; }

    public void Rule(string nonTerminal, params string[] rules)
    {
        if (!RulesDictionary.ContainsKey(nonTerminal))
        {
            RulesDictionary[nonTerminal] = new List<string>();
        }
        RulesDictionary[nonTerminal].AddRange(rules);
        var Rules = rules.Select(production => new Rule(nonTerminal, production));
        RulesList.AddRange(Rules);
        UpdateEpsilonNonTerminals();
    }
    public void AddTerminal(params string[] symbols)
    {
        var symbolsToAdd = symbols.Where(symbol => !ContainsSymbol(symbol)).ToList();
        Terminals.AddRange(symbolsToAdd);
        UpdateEpsilonNonTerminals();
    }
    public void AddNonTerminal(params string[] symbols)
    {
        var symbolsToAdd = symbols.Where(symbol => !ContainsSymbol(symbol)).ToList();
        foreach (var symbol in symbolsToAdd)
        {
            RulesDictionary[symbol] = new List<string>();
        }
        NonTerminals.AddRange(symbolsToAdd);
        UpdateEpsilonNonTerminals();
    }
    public bool ContainsSymbol(string symbol)
    {
        return NonTerminals.Contains(symbol) || Terminals.Contains(symbol) || Epsilon == symbol;
    }

    private bool IsTerminal(string symbol)
    {
        return Terminals.Contains(symbol) || symbol == Epsilon;
    }
    private bool IsNonTerminal(string symbol)
    {
        return NonTerminals.Contains(symbol);
    }
    private void UpdateEpsilonNonTerminals()
    {
        EpsilonNonTerminals = FindEpsilonNonTerminals();
    }
}
