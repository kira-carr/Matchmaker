using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaker
{
    internal class NormalizerService
    {
        public string NormalizeMpn(string mpn) {

            if (string.IsNullOrWhiteSpace(mpn)) {
                return "";
            }
            return mpn.Trim().ToUpper().Replace("999-", string.Empty);    // TODO: ditch leading 999 on LT parts ... except i need the change to not be permanent
        }


        public string NormalizeLt(string? s) {
            if (string.IsNullOrWhiteSpace(s))
            {
                return "";
            }
            return s.Trim().ToUpper();
        }
    }
}
