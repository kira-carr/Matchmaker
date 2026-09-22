using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class KeyBuilderService
    {
        private readonly Dictionary<int, int> counters = new();

        public string BuildFindNumberUnique(int fn) {

            // append unique number to end of find number
            if (!counters.ContainsKey(fn))
            {
                counters[fn] = 1;
            }
            else
            {
                counters[fn]++;
            }
            return $"{fn}-{counters[fn]}";
        }


        public string BuildLtMpnKey(string lt, string mpn) {

            // concatenate LT part number and MPN
            return $"{lt}-{mpn}";
        }
    }
}
