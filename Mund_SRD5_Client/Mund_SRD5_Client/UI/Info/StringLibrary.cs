using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Client
{
    public class StringLibrary
    {
        private static Dictionary<uint, string> _lib = new Dictionary<uint, string>();

        /// <summary>
        /// Cycles through the libraries in the TextLibraries directory and loads the strings contained therein. Duplicate
        /// keys are discarded.
        /// </summary>
        public static void Load()
        {
            foreach(string file in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\TextLibraries\\"))
            {
                FileStream strLib = File.Open(file, FileMode.Open);
                using(StreamReader read = new StreamReader(strLib, Encoding.UTF8))
                {
                    while (read.Peek() >= 0)
                    {
                        string str = read.ReadLine();
                        string[] split = str.Split(new char[] { '|' });
                        uint index = 0;
                        if (uint.TryParse(split[0], out index) &&
                            !_lib.ContainsKey(index))
                        {
                            _lib.Add(index, split[1].Replace("\\n", Environment.NewLine));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fetches a string from the cached library by key.
        /// </summary>
        /// <param name="key">The key to search on</param>
        /// <returns>The found string, or String.Empty on error</returns>
        public static string GetString(uint key)
        {
            if(_lib.ContainsKey(key))
            {
                return _lib[key];
            }
            return String.Empty;
        }
    }
}
