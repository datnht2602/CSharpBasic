using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using System.IO;

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
        string[] result = {};
            try
            {
                List<int> hashcodeWord = new List<int>();

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
                            hashcodeWord.Add(normalizedWord.GetHashCode());
                               
                            }
                        }

                        fs.Close();
                        reader.Close();
                    }
                    hashcodeWord.Sort();
                }

            List<int> topTenhashCode = new List<int>();
            List<int> counts = new List<int>();
            int count = 1;
            for (int i = 0; i < hashcodeWord.Count - 1; i++)
            {
                if (hashcodeWord[i] == hashcodeWord[i + 1])
                {
                    count++;
                }
                else
                {
                    counts.Add(count);
                    topTenhashCode.Add(hashcodeWord[i]);
                    count = 1;
                    if (counts.Count > 10)
                    {
                        int min = counts.Min();
                        int minIndex = counts.IndexOf(min);
                        counts.RemoveAt(minIndex);
                        topTenhashCode.RemoveAt(minIndex);
                    }
                }
            }
            List<string> frequentWords = new List<string>();
            bool shouldBreak = false;
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
                            foreach (var item in topTenhashCode)
                            {
                                if(item == normalizedWord.GetHashCode())
                                {
                                    if(!frequentWords.Contains(normalizedWord))
                                    {
                                        frequentWords.Add(normalizedWord);
                                        if (frequentWords.Count > 10)
                                        {
                                            shouldBreak = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                        }

                        fs.Close();
                        reader.Close();
                    }
                if (shouldBreak) break;
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
