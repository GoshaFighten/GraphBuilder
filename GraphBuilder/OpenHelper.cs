using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder
{
    public static class OpenHelper
    {
        public async static Task<double[]> ReadFile(string fileName)
        {
            char[] file;
            StringBuilder builder = new StringBuilder();

            using (StreamReader reader = File.OpenText(fileName))
            {
                file = new char[reader.BaseStream.Length];
                await reader.ReadAsync(file, 0, (int)reader.BaseStream.Length);
            }

            foreach (char c in file)
            {
                builder.Append(c);
            }
            var array = builder.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToDouble(i)).ToArray();
            return array;
        }
    }
}
