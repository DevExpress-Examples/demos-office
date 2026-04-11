namespace BlazorDemo.OfficeFileApi
{
    public enum RangeExportMode
    {
        First,
        All,
        CustomRange
    }
    public static class RangeHelper
    {
        public static List<int> GetIndexesByRange(RangeExportMode exportMode, string range)
        {
            return exportMode switch
            {
                RangeExportMode.First => new List<int> { 0 },
                RangeExportMode.All => new List<int>(),
                RangeExportMode.CustomRange => new List<int>(ParseRange(range)),
                _ => new List<int>(),
            };
        }
        public static IEnumerable<int> ParseRange(string range)
        {
            if (string.IsNullOrWhiteSpace(range))
                return Enumerable.Empty<int>();
            var result = new SortedSet<int>();
            var tokens = range.Split(new[] { ',', ';', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in tokens)
            {
                var part = raw.Trim();
                if (part.Length == 0) continue;
                int dashIndex = part.IndexOf('-');
                if (dashIndex > 0)
                {
                    var startStr = part.Substring(0, dashIndex).Trim();
                    var endStr = part.Substring(dashIndex + 1).Trim();
                    if (int.TryParse(startStr, out int start) && int.TryParse(endStr, out int end))
                    {
                        if (start <= 0 || end <= 0) continue;
                        if (end < start) (start, end) = (end, start);
                        for (int v = start; v <= end; v++) result.Add(v - 1);
                    }
                    continue;
                }
                if (int.TryParse(part, out int single))
                {
                    if (single > 0) result.Add(single - 1);
                }
            }
            return result;
        }
    }
}
