using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using System.IO;
using Microsoft.VisualBasic;

    public class DerekAnalyser : IDataAnalyser
    {
        public string Author => "Derek";

        public string Path
        {
            get;
            private set;
        }

        public IEnumerable<string> GetTopTenStrings(string path)
        {
            string[] result = { };

        Dictionary<int, int> mp = new Dictionary<int, int>();
        try
            {
               

                string[] datFiles = Directory.GetFiles(path, "*.dat");
                foreach (string filePath in datFiles)
                {
                    
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var words = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                           
                            foreach (var word in words)
                            {
                                string normalizedWord = word.ToLower();
                            if (mp.ContainsKey(normalizedWord.GetHashCode()))
                            {
                                mp[normalizedWord.GetHashCode()]++;
                            }
                            else
                            {
                                mp[normalizedWord.GetHashCode()] = 1;
                            }

                        }
                        
                    }

                        fs.Close();
                        reader.Close();
                    }
             
            }
            List<int> queue = mp.Keys.ToList();
            queue.Sort(delegate (int y, int x) {
                if (mp[x] == mp[y])
                    return x.CompareTo(y);
                else
                    return (mp[x]).CompareTo(mp[y]);
            });
            Console.WriteLine(  " numbers with the most occurrences are:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(queue[i] + $" occurs {mp[queue[i]] }");
            }
           
            List<string> frequentWords = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                foreach (string filePath in datFiles)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var words = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var wordInDatFiles in words)
                            {
                                string normalizedWord = wordInDatFiles.ToLower();
                                if (queue[i] == normalizedWord.GetHashCode())
                                {
                                    if (!frequentWords.Contains(normalizedWord))
                                    {
                                        frequentWords.Add(normalizedWord);

                                    }
                                }
                            }

                        }
                        fs.Close();
                        reader.Close();
                    }
                }
            }
           


            return frequentWords;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return result;

        }
    
    public DerekAnalyser(string path)
        {
            this.Path = path;
        }

    }
