using System;
using System.Collections.Generic;
using System.Text;

namespace FromDB
{
    class Hello
    {

        private string StringSearch = null;

        public void HelloDialog()
        {
            Console.WriteLine("\tВведите слово или его часть для поиска в базе данных \r\n\r\n\t");

            StringSearch = Convert.ToString(Console.ReadLine());
        }


        public string GetStringSearch
        {
            get
            {
                return StringSearch;
            }
        }


    }
}
