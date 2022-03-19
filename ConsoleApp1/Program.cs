using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
                int l_ur = 40;
                Funkcje func = new Funkcje();
                func.wpisywanie();
                func.losowanie();
                for (int i = 0; i < l_ur; i++)
                {
                    func.start();
                    Thread.Sleep(1);
                }
                func.zapis();
                func.wyswietl();
                Console.ReadKey();
        }
    }
}
