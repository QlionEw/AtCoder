namespace Qlibrary
{
    public static class BaseN
    {
        public static string ToString(long value, int baseValue)
        {
            string s = "";

            while (value > 0)
            {
                var c = value % baseValue;
                s = c.ToString() + s;
                value /= baseValue;
            }

            return string.IsNullOrEmpty(s) ? "0" : s;
        }

        public static long ToLong(string s, int baseValue)
        {
            long value = 0;
            for (int i = 0; i < s.Length; i++)
            {
                value = ((value * baseValue) + (s[i] - '0'));
            }

            return value;
        }

        public static string Convert(string s, int beforeBase, int afterBase)
        {
            return ToString(ToLong(s, beforeBase), afterBase);
        }
    }
}