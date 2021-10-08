using System;

namespace FromDB
{
    class Program
    {
        static void Main(string[] args)
        {

            Hello hello = new Hello();

            GetString getString = null;

            // приглашение к введению строки
            hello.HelloDialog();

            getString = new GetString(hello.GetStringSearch);
        }
    }
}
