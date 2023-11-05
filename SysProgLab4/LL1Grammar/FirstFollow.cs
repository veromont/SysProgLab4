namespace SysProgLab4.LL1Grammar;

public partial class LL1Grammar
{
    // now for first
    public HashSet<string> First(string symbol, int k=1)
    {
        if (k < 0) throw new ArgumentException("k cannot be negative.");
        else if (!ContainsSymbol(symbol)) throw new ArgumentException($"symbol '{symbol}' is not known in this grammar");

        var result = new HashSet<string>();

        if (k == 0) 
        { 
            return result; 
        }
        else if (IsTerminal(symbol))
        {
            result.Add(symbol);
            return result;
        }
        else if (!RulesDictionary.ContainsKey(symbol)) throw new ArgumentException($"{symbol} is a not productive non-terminal");

        foreach (var production in RulesDictionary[symbol])
        {
            var Y = ParseProduction(production);
            var FirstY0 = First(Y[0]);
            result.UnionWith(FirstY0);

            if (Epsilon == null) continue;

            if (EpsilonNonTerminals.Contains(symbol))
            {
                result.Add(Epsilon);
            }

            if (!EpsilonNonTerminals.Contains(Y[0]) || IsTerminal(Y[0]))
            {
                continue;
            }

            // while non-terminal is epsilon one, add also next non-terminal
            for (int i = 0; i < Y.Count; i++)
            {
                result.UnionWith(First(Y[i]).Where(symbol => symbol != Epsilon));
                var FirstYi = First(Y[i]);
                if (!EpsilonNonTerminals.Contains(Y[i]))
                {
                    break;
                }
            }
        }
        return result;
    }
    public HashSet<string> Follow(string symbol, int k=1)
    {
        var result = new HashSet<string>();
        if (symbol == StartNonTerminal && Epsilon != null)
        {
            result.Add(Epsilon);
        }
        foreach(var SymbolProductions in RulesDictionary)
        {
            foreach(var production in SymbolProductions.Value)
            {
                result.UnionWith(FollowFromProduction(symbol, SymbolProductions.Key, production));
            }
        }
        return result;
    }
    private HashSet<string> FollowFromProduction(string symbol,string productionFrom,string productionTo)
    {
        var result = new HashSet<string>();
        var producedSymbols = ParseProduction(productionTo);
        if (!producedSymbols.Contains(symbol))
        {
            return new HashSet<string>();
        }
        for (int i = 0; i < producedSymbols.Count; i++)
        {
            if (producedSymbols[i] == symbol)
            {
                if (i  == producedSymbols.Count - 1 && productionFrom != symbol)
                {
                    result.UnionWith(Follow(productionFrom));
                }
                else
                {
                    var nextSymbolIndex = i + 1;
                    if (nextSymbolIndex >= producedSymbols.Count) break;

                    result.UnionWith(First(producedSymbols[nextSymbolIndex]).Where(symbol => symbol != Epsilon));
                    while (EpsilonNonTerminals.Contains(producedSymbols[nextSymbolIndex]))
                    {
                        nextSymbolIndex++;

                        if (nextSymbolIndex >= producedSymbols.Count) 
                        {
                            result.UnionWith(Follow(productionFrom));
                            return result;
                        };

                        result.UnionWith(First(producedSymbols[nextSymbolIndex]).Where(symbol => symbol != Epsilon));
                    }
                }
            }
        }
        return result;
    }
}
