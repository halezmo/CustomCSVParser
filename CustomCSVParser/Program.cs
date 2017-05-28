using CustomCSVParser.Service;
using System;
using System.Collections.Generic;

namespace CustomCSVParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvFilePath;
            int[] column;
            string[] value;

            ExtractArguments(args, out csvFilePath, out column, out value);
          
            ICsvParser parser = new CsvParser(csvFilePath);

            var data = parser.GetParsedData(column, value);

            ShowData(data);

            Console.ReadLine();
        }

        /// <summary>
        /// Extract argument that are used in CsvParser
        /// </summary>
        /// <param name="args">all args</param>
        /// <param name="filePath">extracted file path</param>
        /// <param name="column">extracted column filter</param>
        /// <param name="value">extracte value filter</param>
        private static void ExtractArguments(string[] args, out string filePath, out int[] column, out string[] value)
        {
            filePath = null;
            column = null;
            value = null;
            if(args.Length >= 1)
            {
                filePath = args[0];
            }
            if (args.Length >= 2)
            {
                column = Array.ConvertAll(args[1].Split(','), int.Parse);
            }
            if (args.Length >= 3)
            {
                value = args[2].Split(',');
            }
        }

        /// <summary>
        /// Enumerate through data and display resuld
        /// </summary>
        /// <param name="data">data to enumerate</param>
        private static void ShowData(IEnumerable<string> data)
        {
            using (var enumeratorItem = data.GetEnumerator())
            {
                while (enumeratorItem.MoveNext())
                {
                    Console.WriteLine(enumeratorItem.Current);
                }
            }
        }
    }
}
