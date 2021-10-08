using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace FromDB
{
    class GetString
    {

        private Dictionary<string, int> Upload = new Dictionary<string, int>();
        private Dictionary<string, int> SortedUpload = new Dictionary<string, int>();

        public GetString(string Search)
        {

            if (Search == "")
            {
                Console.WriteLine("\tДанные для поиска не заданы. \r\n\tПрограмма завершена");
            }
            else
            {
                Console.WriteLine("\tИдёт поиск");

                if (Getlist(Search, Upload))
                {

                    GetSortWords(Upload, SortedUpload);

                }
                else
                {
                    Console.WriteLine("\tДанные не найдены.");
                }

                
            }
        }


        private bool Getlist(string _searsh, Dictionary<string, int> WordCount)
        {
            string selectDB = "SELECT * FROM " + _searsh.Substring(0,1);

            string ExistConnetion = @"Data Source=.\SQLEXPRESS;Initial Catalog = MyDatabase;Integrated Security = True";

            bool ReadOK = false;

            using (SqlConnection conn = new SqlConnection(ExistConnetion))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(selectDB, conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // добавляем к выборке только то что соответствует условию: содержит букву или подстроку
                    if (Convert.ToString(reader.GetValue(1)).Substring(0,_searsh.Length) == _searsh)
                    {
                        WordCount.Add(Convert.ToString(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)));
                    }
                    

                    ReadOK = true;
                }

                return ReadOK;

            }
        }


        private void GetSortWords(Dictionary<string, int> Words, Dictionary<string, int> SortWords)
        {
            string[] keys = new string[Words.Count];
            int[] Values = new int[Words.Count];

            //делаем два массива из ключей и значений для сортировки
            int i = 0;
            foreach (KeyValuePair<string, int> h in Words)
            {
                keys[i] = h.Key;
                Values[i] = h.Value;
                i++;
            };


            // тупо пузырёк по двум параметрам
            // который проходит с конца в начало ровно пять раз
            for (int j = 0; j < 5; j++)
            {
                
                for (int k = Words.Count - 1; k>j; k--)
                {
                    if (Values[k] > Values[k - 1])
                    {
                        int tempI = Values[k];
                        Values[k] = Values[k - 1];
                        Values[k-1] = tempI;

                        string tempS = keys[k];
                        keys[k] = keys[k - 1];
                        keys[k - 1] = tempS;
                    }
                    
                    // если количество слов равно между собой
                    else if (Values[k] == Values[k - 1])
                    {
                        int MinLenght;
                        // находим наиболее длинное слово
                        if (keys[k].Length > keys[k - 1].Length)
                        {
                            MinLenght = keys[k - 1].Length;
                        }
                        else
                        {
                            MinLenght = keys[k].Length;
                        }

                        // по этому флагу будем смотреть 
                        // выяснилось ли в процессе сравнения по алфавиту
                        // какое слово главнее
                        // если нет - главнее более длинное
                        bool CompareWordsComplete = false;

                        for (int l = 0; l < MinLenght; l++)
                        {
                            // первая буква по алфавиту в сравнении меньше второй, поэтому
                            // на самом деле ещё есть русская ё, которая вне правил
                            if (Convert.ToChar(keys[k].Substring(l,1)) < Convert.ToChar(keys[k - 1].Substring(l, 1)))
                            {
                                int tempI = Values[k];
                                Values[k] = Values[k - 1];
                                Values[k - 1] = tempI;

                                string tempS = keys[k];
                                keys[k] = keys[k - 1];
                                keys[k - 1] = tempS;

                                // сравнение слов состоялось
                                CompareWordsComplete = true;
                                break;
                            }

                            // если вдруг очередная буква текущего слова меньше соответствующей предыдущего слова
                            else if(Convert.ToChar(keys[k].Substring(l, 1)) > Convert.ToChar(keys[k - 1].Substring(l, 1)))
                            {
                                // сравнение слов состоялось
                                CompareWordsComplete = true;
                                break;
                            }

                            // если сравнение не состоялось
                            if (!CompareWordsComplete)
                            {
                                // выше поднимаем более длинное слово
                                if (keys[k].Length > keys[k - 1].Length)
                                {
                                    int tempI = Values[k];
                                    Values[k] = Values[k - 1];
                                    Values[k - 1] = tempI;

                                    string tempS = keys[k];
                                    keys[k] = keys[k - 1];
                                    keys[k - 1] = tempS;
                                }
                            }

                        }


                    } 
                }


            }

            // больше пяти элементов возвращать смысла нет, поэтому
            int countElem = 5;
            if (Words.Count < 5)
            {
                countElem = Words.Count;
            }



            for (int j = 0; j < countElem; j++)
            {
                SortWords.Add(keys[j], Values[j]);

                Console.WriteLine(keys[j] + Convert.ToString(Values[j]));
            }

        }
    }
}
