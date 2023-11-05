using SysProgLab4.LL1Grammar;

var grammar = new LL1Grammar("S");
/*
S -> ACB | Cbb | Ba
A -> da | BC
B -> g | Є
C -> h | Є        
 */
grammar.AddTerminal("+", "*", "(", ")", "a");
grammar.AddNonTerminal("A", "B", "C", "D");
grammar.Epsilon = "eps";

grammar.Rule("S", "B A");
grammar.Rule("A", "+ B A", "eps");
grammar.Rule("B", "D C");
grammar.Rule("C", "* D C", "eps");
grammar.Rule("D", "( S )", "a");

var First = new Dictionary<string, HashSet<string>>();
var Follow = new Dictionary<string, HashSet<string>>();
foreach (var NonTerminal in grammar.NonTerminals)
{
    First.Add(NonTerminal, grammar.First(NonTerminal));
    Follow.Add(NonTerminal, grammar.Follow(NonTerminal));
}
var eps_non_terminals = grammar.EpsilonNonTerminals;

foreach (var nonTerminal in grammar.NonTerminals)
{
    Console.WriteLine($"FIRST for {nonTerminal}: {"{"} {string.Join(", ", First[nonTerminal])} {"}"}");
}
Console.WriteLine();
foreach (var nonTerminal in grammar.NonTerminals)
{
    Console.WriteLine($"FOLLOW for {nonTerminal}: {"{"} {string.Join(", ", Follow[nonTerminal])} {"}"}");
}
Console.WriteLine();
Console.WriteLine($"Epsilon symbols are: {"{"} {string.Join(", ", eps_non_terminals)} {"}"}");
Console.WriteLine();

Dictionary<int, HashSet<string>> ruleProduction = new Dictionary<int, HashSet<string>>();

// for rule A -> w
Console.WriteLine("RULE            | RULE NUMBER | FIRST(w) + FOLLOW(A)");
Console.WriteLine("----------------------------------------------------");
for (int i = 0; i < grammar.RulesList.Count; i++)
{
    var A = grammar.RulesList[i].SymbolFrom;
    var w = grammar.ParseProduction(grammar.RulesList[i].Production)[0];
    ruleProduction[i] = grammar.ConcatSets(grammar.First(w), Follow[A]);
    Console.WriteLine($"{grammar.RulesList[i],-15} | {i+1,-11} | {"{"} {string.Join(", ", ruleProduction[i])} {"}"}");
}