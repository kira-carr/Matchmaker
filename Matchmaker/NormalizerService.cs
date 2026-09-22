using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Matchmaker
{
    internal class NormalizerService
    {
        public string NormalizeMpn(string? mpn) {

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


        public string NormalizeSupplier(string? supplier)
        {
            var s = (supplier ?? "").Trim().ToUpperInvariant();
            // Optionally map known variants ("3M COMPANY" -> "3M")
            return s;
        }

        public string NormalizeSpec(string? spec)
        {
            var s = (spec ?? "").Trim().ToUpperInvariant();
            // Remove spaces to match "AMS 4023" == "AMS4023"
            s = Regex.Replace(s, @"\s+", "");
            return s;
        }

    }
}
