using System.Text;
using static Qlibrary.Common;

namespace QlibraryTester
{
    public class InputCreator
    {
        private readonly string path;
        private Random r = new Random(DateTime.Now.Millisecond);
        public InputCreator(string path)
        {
            this.path = path;
            using var sw = new StreamWriter(path);
        }

        public void AddString(int count, char start, int range)
        {
            var sb = new StringBuilder();
            Loop(count, () => sb.Append((char)(start + r.Next(0, range))));
            using var sw = new StreamWriter(path, true);
            sw.WriteLine(sb.ToString());
        }

        public void AddInteger(int min, int max)
        {using var sw = new StreamWriter(path, true);
            sw.WriteLine(r.Next(min, max - min + 1));
        }
    }
}