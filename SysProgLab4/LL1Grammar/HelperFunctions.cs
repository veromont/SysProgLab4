using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgLab4.LL1Grammar
{
    public partial class LL1Grammar
    {
        public List<string> ParseProduction(string production)
        {
            var result = production.Split(" ").ToList();
            return result;
        } 
        public HashSet<string> ConcatSets(HashSet<string> set1, HashSet<string> set2) 
        {
            if (set1.Count == 0 || set2.Count == 0) 
            {
                return new HashSet<string>();
            }
            if (Epsilon == null)
            {
                return set1;
            }

            if (set1.Contains(Epsilon))
            {
                return set1.Union(set2).ToHashSet();
            }
            else
            {
                return set1;
            }
        }
    }
}
