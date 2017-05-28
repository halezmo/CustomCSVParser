using System;
using System.Collections.Generic;

namespace CustomCSVParser.Service
{
    public interface ICsvParser
    {
        IEnumerable<String> GetParsedData(IList<int> column, IList<string> value);


    }
}
