using System.Text;

namespace Task_1
{
    public static class Extensions
    {
        public static string ConcatenateWithSeparator(this List<string> list, string separator)
        {
            if (list == null || list.Count == 0) return string.Empty;
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i]);
                if (i < list.Count - 1) stringBuilder.Append(separator);
            }
            return stringBuilder.ToString();
        }
    }
}
