using System.Text.RegularExpressions;

namespace LumenCodex.Domain.Utils;

public class NaturalStringComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x == y) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        var expr = new Regex(@"([-+]?[0-9]+)|([^0-9]+)", RegexOptions.Compiled);
        var xChunks = expr.Matches(x);
        var yChunks = expr.Matches(y);

        int minCount = Math.Min(xChunks.Count, yChunks.Count);

        for (int i = 0; i < minCount; i++)
        {
            string xVal = xChunks[i].Value;
            string yVal = yChunks[i].Value;

            if (char.IsDigit(xVal[0]) && char.IsDigit(yVal[0]))
            {
                if (long.TryParse(xVal, out long xNum) && long.TryParse(yVal, out long yNum))
                {
                    int cmp = xNum.CompareTo(yNum);
                    if (cmp != 0) return cmp;
                }
            }

            int strCmp = string.Compare(xVal, yVal, StringComparison.OrdinalIgnoreCase);
            if (strCmp != 0) return strCmp;
        }

        return xChunks.Count.CompareTo(yChunks.Count);
    }
}