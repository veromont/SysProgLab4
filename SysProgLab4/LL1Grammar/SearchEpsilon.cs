using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgLab4.LL1Grammar
{
    public partial class LL1Grammar
    {
        private void AddEpsilonNonTerminals(HashSet<string> EpsilonNonTerminals)
        {
            if (Epsilon == null) return;

            if (EpsilonNonTerminals.Count == 0) 
            {
                EpsilonNonTerminals.UnionWith(RulesDictionary
                    .Where(production => production.Value.Contains(Epsilon))
                    .Select(production => production.Key));
            }

            EpsilonNonTerminals.UnionWith(RulesDictionary
                .Where(production =>
                    production.Value.Any(prodValue =>
                    {
                        var productionResult = ParseProduction(prodValue);
                        return productionResult.All(symbol => symbol == Epsilon || EpsilonNonTerminals.Contains(symbol));
                    }))
                .Select(production => production.Key));
        }
        public HashSet<string> FindEpsilonNonTerminals()
        {
            var result = new HashSet<string>();
            int countBefore;
            do
            {
                countBefore = result.Count;
                AddEpsilonNonTerminals(result);
            } while (result.Count - countBefore > 0);

            return result;
        }
    }
}
