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
            string[] result = { };
            try
            {
                Dictionary<int, byte> wordFrequency = new Dictionary<int, byte>();

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
                                if (wordFrequency.ContainsKey(normalizedWord.GetHashCode()))
                                {
                                    wordFrequency[normalizedWord.GetHashCode()]++;
                                }
                                else
                                {
                                    wordFrequency[normalizedWord.GetHashCode()] = 1;
                                }
                            }
                        }

                        fs.Close();
                        reader.Close();
                    }

                }

                var top10Words = wordFrequency
                     .Where(entry => entry.Value >1)
                    .OrderByDescending(s => s.Value)
                    .Take(10)
                    .Select(entry => entry.Key);
                List<string> frequentWords = new List<string>();
                foreach (var word in top10Words)
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
                                if (word == normalizedWord.GetHashCode())
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
