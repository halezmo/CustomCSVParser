using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomCSVParser.Service
{
    public class CsvParser : ICsvParser
    {
        private const char CSV_SEPARATPOR = ',';
        private HashSet<string[]> extractedValues = new HashSet<string[]>();
        private Dictionary<int, int> maxColSize = new Dictionary<int, int>();

        public CsvParser(String filePath)
        {
            LoadFile(filePath);
        }

        private void LoadFile(String filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            FileInfo csvFile = new FileInfo(filePath);
            if (!csvFile.Exists)
            {
                throw new FileNotFoundException("filePath");
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineValues = lines[i].Split(CSV_SEPARATPOR);
                for (int j = 0; j < lineValues.Length; j++)
                {
                    HandleColumnSize(j, lineValues[j]);
                }
                extractedValues.Add(lineValues);
            }
        }

        private void HandleColumnSize(int index, string value)
        {
            if (maxColSize.ContainsKey(index))
            {
                if (value.Length > maxColSize[index])
                {
                    maxColSize[index] = value.Length;
                }
            }
            else
            {
                maxColSize.Add(index, value.Length);
            }
        }
        
        private void AppendValue(StringBuilder sBuilder, int index, string[] value)
        {
            bool isLastIndex = index + 1 == value.Length;
            sBuilder.AppendFormat("|{0}", value[index].PadRight(maxColSize[index]));
            if (isLastIndex)
            {
                sBuilder.Append("|");//in case of last value from line we want to add | at the end
            }
        }

        public IEnumerable<string> GetParsedData(IList<int> column, IList<string> value)
        {
            if (extractedValues == null)
                throw new InvalidOperationException("extractedValues not defiend");

            IList<string> outputData = new List<string>();

            StringBuilder lineStringBuilder = new StringBuilder();

            foreach (var item in extractedValues)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (i == 0)
                    {
                        AppendValue(lineStringBuilder, i, item);
                    }
                    else
                    {
                        bool useColumn = column == null || column.Contains(i);
                        if (useColumn)
                        {
                            bool useValue = value == null || value.Contains(item[i]);
                            if (useValue)
                            {
                                AppendValue(lineStringBuilder, i, item);
                            }
                        }
                    }                              
                }
                outputData.Add(lineStringBuilder.ToString());
                lineStringBuilder.Clear();
            }
            return outputData;
        }
    }
}
