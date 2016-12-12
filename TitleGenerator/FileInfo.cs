using System.Collections.Generic;
using System.IO;

namespace TitleGenerator
{
    public class FileInfo
    {
        public FileInfo()
        {

        }

        public Dictionary<string, string> ConvertFileToDictionary(string path)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                if (line.Length > 1)
                {
                    var arr = line.Split(';');
                    result.Add(arr[0], arr[1]);
                }
            }

            return result;
        }

        public string ReturnNumberFromDictionary(string key, Dictionary<string, string> dictionary)
        {
            string result = string.Empty;

            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key];
            }
            else
            {
                result = "problem";
            }

            return result;
        }
    }
}
