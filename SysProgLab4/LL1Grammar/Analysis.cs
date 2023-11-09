using SysProgLab4.ControlTableAndCell;

namespace SysProgLab4.LL1Grammar;

public partial class LL1Grammar
{
    public ControlTable ControlTable { get; private set; }
    public void ConstructControlTable()
    {
        ControlTable = new ControlTable(Terminals);
        for (int i = 0; i < RulesList.Count; i++)
        {
            var A = RulesList[i].SymbolFrom;
            var w = ParseProduction(RulesList[i].Production)[0];
            var finalSet = ConcatSets(First(w), Follow(A));
            foreach (var symbol in finalSet)
            {
                ControlTable.AddCell(A, symbol, i);
            }
        }
    }

    public string ParseStringOfLexemes(string StringOfLexemes)
    {
        List<int> ruleSequence = new List<int>();
        Stack<string> stack = new Stack<string>();
        var str = StringOfLexemes;
        stack.Push(StartNonTerminal);
        int i = 0;
        string header = "|            STACK            |  RULE INDEX  |              RULE              |  CURRENT LEXEME   |";
        string divider = new string('_', header.Length);
        Console.WriteLine(header);
        Console.WriteLine(divider);

        ParseString(str, stack, ruleSequence, false);

        if (Epsilon == null && stack.Count > 0)
        {
            throw new Exception("No epsilon but stack is not empty after reading all string");
        }

        ParseString(str, stack, ruleSequence, true);

        Console.WriteLine(divider);
        if (stack.Count > 0)
        {
            throw new Exception("Stack is not empty after reading all string");
        }

        return string.Join(", ", ruleSequence);
    }

    private void ParseString(string str, Stack<string> stack, List<int> ruleSequence, bool destroy)
    {
        int i = 0;

        while (i < str.Length && stack.Count > 0)
        {
            var topStackLexeme = stack.Peek();
            var currentLexeme = destroy ? Epsilon : str[i].ToString();

            if (topStackLexeme == Epsilon)
            {
                stack.Pop();
                continue;
            }

            if (topStackLexeme == currentLexeme)
            {
                stack.Pop();
                i++;
                continue;
            }

            if (IsTerminal(topStackLexeme))
            {
                throw new Exception($"Expected lexeme {topStackLexeme}");
            }

            int ruleIndex = ControlTable.GetRule(topStackLexeme, currentLexeme);
            ruleSequence.Add(ruleIndex + 1);
            var ruleProduct = ParseProduction(RulesList[ruleIndex].Production);
            stack.Pop();
            ruleProduct.Reverse();

            foreach (var temp in ruleProduct)
            {
                stack.Push(temp);
            }

            Console.WriteLine($"|{string.Join(" ", stack),28} | {ruleIndex + 1,12} | {RulesList[ruleIndex],30} | {currentLexeme,17} |");
        }
    }
}
