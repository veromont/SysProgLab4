using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SysProgLab4.ControlTableAndCell
{
    public struct ControlTableCell
    {
        public string Symbol;
        public int RuleNumber;
    }
    public class ControlTable
    {
        List<string> terminals;
        public Dictionary<string, List<ControlTableCell>> Table { get; }
        public ControlTable(List<string> terminals)
        {
            Table = new Dictionary<string, List<ControlTableCell>>();
            this.terminals = terminals;
        }
        public void AddCell(string NonTerminal, string Symbol, int RuleNumber)
        {
            if (!Table.ContainsKey(NonTerminal))
            {
                Table.Add(NonTerminal, new List<ControlTableCell>());
            }
            var cell = new ControlTableCell();
            cell.Symbol = Symbol;
            cell.RuleNumber = RuleNumber;
            Table[NonTerminal].Add(cell);
        }
        public int GetRule(string NonTerminal, string Symbol)
        {
            var row = Table[NonTerminal];
            var cell = row.Where(cell => cell.Symbol == Symbol);
            if (cell.Count() == 0)
            {
                var expectedSymbols = row.Select(cell => cell.Symbol).ToList();
                throw new Exception($"\nError: for non-terminal {NonTerminal} expected symbols [{string.Join(", ", expectedSymbols)}].\n" +
                    $"Got symbol {Symbol}");
            }
            return cell.First().RuleNumber;
        }

        //public override string ToString()
        //{
        //    const int OFFSET = 5;

        //    StringBuilder str = new StringBuilder();
        //    string header = "      |";
        //    foreach(var terminal in terminals)
        //    {
        //        header += $"{terminal,OFFSET} |";
        //    }

        //    str.AppendLine(header);
        //    str.AppendLine(new string('-', header.Length));
        //    foreach (var key in Table.Keys)
        //    {
        //        var row = $"{key,OFFSET} |";
        //        var genericCellLength = row.Length;
        //        foreach (var terminal in terminals)
        //        {
        //            int? rule = Table[key].Where(cell => cell.Symbol == terminal).Select(cell => cell.RuleNumber).FirstOrDefault();
        //            if (rule == null)
        //            {
        //                row += $"|{genericCellLength-2}|";
        //            }
        //            row += $"{rule,OFFSET} |";
        //        }

        //        str.AppendLine(row);
        //    }
        //    return str.ToString();
        //}
    }
}
