using System;
using System.Collections.Generic;
using System.IO;
namespace ConsoleApp1
{
    internal class Funkcje
    {
        int a { get; set; } //wsp. a równania
        int b { get; set; } //wsp. b równania
        int c { get; set; } //wsp. c równania
        int ile_os { get; set; } //ilosc osobników w populacji
        double max { get; set; } //wartość maksymalna
        double pr_krzyz { get; set; } //prawdopodobieństwo krzyżowania
        double pr_mutacji { get; set; } //prawdopobobieństwo mutacji
        int ind { get; set; } //indeks
        int lb_pop { get; set; } //liczba populacji
        double f_suma { get; set; }
        double min { get; set; }
        static int rozm { get; set; } //rozmiar list
        List<string> rand_os = new List<string>(new string[rozm]); //lista liczb w postaci binarnej
        List<string> potomek_k = new List<string>(new string[rozm]); //lista do przechowywania potomków 
        List<double> f_x = new List<double>(new double[rozm]); //lista wyników funkcji
        List<double> p_x = new List<double>(new double[rozm]); //lista prawdopodobieństw 
        List<int> liczby_p = new List<int>(new int[rozm]); //lista początkowych liczb wylosowanych
        List<bool> sprawdz = new List<bool>(new bool[rozm]); //lista skrzyżowanych osobników
        List<Tuple<double, int>> wynik = new List<Tuple<double, int>>(); //lista najlepszych z populacji
        public Funkcje() //przypisanie początkowych wartości
        {
            f_suma = 0;
            min = 0;
            //lb_pop = 10;
            //ile_os = 10;
            //a = -1;
            //b = 0;
            //c = 0;
            //pr_krzyz = 0.9;
            //pr_mutacji = 0.2;
            //max = 0;
            //rozm = ile_os;
            //in_l();
        }
        void in_l() //nadanie początkowych wartosci do list
        {

            for (int i = 0; i < ile_os; i++)
            {
                rand_os.Add("");
                potomek_k.Add("");
                f_x.Add(0);
                sprawdz.Add(false);
            }
        }
        public void wpisywanie() //funkcja do przyjmowania parametrów od użytkownika
        {
            string ex = "";
            int a = 0, b = 0, c = 0, il_o = 0, pop = 0;
            double k = 0, m = 0;

            do
            {
                Console.WriteLine("Podaj parametr a: ");
                try
                {
                    a = int.Parse(Console.ReadLine());
                    ex = "";
                }
                catch (Exception e)
                {
                    ex = e.ToString();
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                if (ex != "")
                    continue;
                else
                    break;
            } while (true);

            do
            {
                Console.WriteLine("Podaj parametr b: ");
                try
                {
                    b = int.Parse(Console.ReadLine());
                    ex = "";
                }
                catch (Exception e)
                {
                    ex = e.ToString();
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                if (ex != "")
                    continue;
                else
                    break;
            } while (true);

            do
            {
                Console.WriteLine("Podaj parametr c: ");
                try
                {
                    c = int.Parse(Console.ReadLine());
                    ex = "";
                }
                catch (Exception e)
                {
                    ex = e.ToString();
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                if (ex != "")
                {
                    continue;
                }
                else
                    break;
            } while (true);

            do
            {
                Console.WriteLine("podaj ilosc osobników w populacji: ");
                try
                {
                    il_o = int.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                Console.WriteLine("podaj ilosc populacji: ");
                try
                {
                    pop = int.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                if (il_o * pop > 150 && il_o > 2)
                {
                    Console.Clear();
                    Console.WriteLine("podaj poprawna wartość");
                    Console.ReadKey();
                }
            } while (il_o * pop > 150 && il_o > 2);

            do
            {
                try
                {
                    Console.WriteLine("Podaj prawdopodobienstwo krzyzowania: ");
                    k = double.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                try
                {
                    Console.WriteLine("Podaj prawdopodobienstwo mutacji: ");
                    m = double.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("błąd:{0} ", e.Message);
                    Console.ReadKey();
                    Console.Clear();
                }
                if ((m < 0.0 || m > 1.0) || (k < 0.0 || k > 1.0))
                {
                    Console.Clear();
                    Console.WriteLine("wprowadzone wartości spoza zakresu 0-1");
                    Console.ReadKey();
                    continue;
                }
                else
                    break;
            } while (true);
            param_start(a, b, c, il_o, pop, k, m);
        }
        void param_start(int _a, int _b, int _c, int ilosc_osob, int populacja, double pr_krz, double pr_mut) //przypisanie parametrów
        {
            a = _a;
            b = _b;
            c = _c;
            ile_os = ilosc_osob;
            pr_krzyz = pr_krz;
            pr_mutacji = pr_mut;
            lb_pop = populacja;
            rozm = ilosc_osob;
            in_l();
        }
        void wyczysc() //resetowania wartosci w liscie sprawdzającej
        {
            for (int i = 0; i < sprawdz.Count; i++)
            {
                sprawdz[i] = false;
            }
        }
        void krzyzowanie() //tworzenie potomków z pary rodziców
        {
            double w_lb_psl;
            Random r = new Random();
            int rodzic1 = r.Next(0, ile_os);
            int rodzic2 = r.Next(0, ile_os);
            int i = 0;
            for (; i < ile_os; i += 2)
            {
                if (sprawdz[rodzic1] == true)
                {
                    rodzic1 = r.Next(0, ile_os);
                    i -= 2;
                    continue;
                }
                else if (sprawdz[rodzic2] == true || rodzic1 == rodzic2)
                {
                    rodzic2 = r.Next(0, ile_os);
                    i -= 2;
                    continue;
                }
                else
                {
                    sprawdz[rodzic1] = true;
                    sprawdz[rodzic2] = true;
                    string temp;
                    w_lb_psl = r.NextDouble();
                    if (w_lb_psl <= pr_krzyz)
                    {
                        int przeciecie = r.Next(0, 8);
                        temp = rand_os[rodzic1].Substring(0, przeciecie);
                        temp += rand_os[rodzic2].Substring(przeciecie);
                        potomek_k[i] = temp;
                        temp = rand_os[rodzic2].Substring(0, przeciecie);
                        temp += rand_os[rodzic1].Substring(przeciecie);
                        potomek_k[i + 1] = temp;
                    }
                    else
                    {
                        potomek_k[i] = rand_os[rodzic1];
                        potomek_k[i + 1] = rand_os[rodzic2];
                    }
                }
            }
            wyczysc();
        }
        void mutacja() //mutacja osobników
        {
            Random rand = new Random();
            double w_lb_psl;
            char[] tmp;
            for (int i = 0; i < potomek_k.Count; i++)
            {
                tmp = potomek_k[i].ToCharArray(); //wczytanie jednego potomka i zamiana na tablicę znaków
                for (int j = 0; j < 8; j++)
                {
                    w_lb_psl = rand.NextDouble();
                    if (w_lb_psl <= pr_mutacji)
                    {
                        if (tmp[j] == '1')
                        {
                            tmp[j] = '0';
                        }
                        else
                        {
                            tmp[j] = '1';
                        }
                    }
                }
                string temp = new string(tmp);
                potomek_k[i] = temp;
            }
        }
        void selekcja() //selekcja osobników metodą ruletki 
        {
            double losowanie = 0;
            int temp;
            double sum_p;
            Random ra = new Random();
            for (int i = 0; i < ile_os; i++)
            {
                temp = zamiana_dec(potomek_k[i]);
                f_x[i] = a * Math.Pow(temp, 2) + b * temp + c;
                if (f_x[i] < min && i == ile_os - 1)
                {
                    ujemna();
                }
                else if (f_x[i] > min && i == ile_os - 1)
                {
                    dodatnia();
                }
            }
            for (int i = 0; i < ile_os; i++)
            {
                losowanie = ra.NextDouble();
                sum_p = 0;
                for (int j = 0; j < ile_os; j++)
                {
                    sum_p += p_x[j];
                    if (sum_p >= losowanie)
                    {
                        rand_os[i] = potomek_k[i];
                        break;
                    }
                }
            }
        }
        void ujemna()
        {
            double praw;
            for (int i = 0; i < ile_os; i++)
            {
                if (f_x[i] < min)
                {
                    min = f_x[i];
                }
            }
            for (int i = 0; i < ile_os; i++)
            {
                f_x[i] = f_x[i] - min;
                f_suma += f_x[i];
            }
            for (int i = 0; i < ile_os; i++)
            {
                praw = f_x[i] / f_suma;
                p_x.Add(praw);
            }
        }
        void dodatnia()
        {
            double praw;
            for (int i = 0; i < ile_os; i++)
            {
                f_suma += f_x[i];
            }
            for (int i = 0; i < ile_os; i++)
            {
                praw = f_x[i] / f_suma;
                p_x.Add(praw);
            }
        }
        int zamiana_dec(string os) //zamiana liczb na dziesiętne 
        {
            return Convert.ToInt32(os, 2);
        }
        public void losowanie() //losowanie
        {
            Random r = new Random();
            for (int i = 0; i < ile_os; i++)
            {
                liczby_p.Add(r.Next(0, 256));
                rand_os[i] = Convert.ToString(liczby_p[i], 2).PadLeft(8, '0');
            }
        }
        public void wyswietl() //wyświetlanie końcowego wyniku
        {
            foreach (var w in wynik)
            {
                Console.WriteLine("wynik: {0} ", w.Item1 + " liczba: " + w.Item2.ToString());
            }
        }
        void wyswietl_p_x()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("prawdopodobieństwo przed posortowaniem:{0}", p_x[i]);
            }
            sort();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("prawdopodobieństwo po posortowaniu:{0}", p_x[i]);
            }
        }
        void sort() //sortowanie prawdopodobieństw
        {
            double wart;
            int i, j;
            for (i = 1; i < p_x.Count; i++)
            {
                wart = p_x[i];
                j = i - 1;
                while (j >= 0 && p_x[j] > wart)
                {
                    p_x[j + 1] = p_x[j];
                    j = j - 1;
                }
                p_x[j + 1] = wart;
            }
        }
        public void start() //start głównej funkcji
        {
            for (int j = 0; j < lb_pop; j++)
            {
                krzyzowanie();
                mutacja();
                selekcja();
            }
            max = f_x[0];
            for (int i = 0; i < ile_os; i++)
            {
                if (f_x[i] > max)
                {
                    max = f_x[i];
                    ind = i;
                }
            }
            int liczba = zamiana_dec(rand_os[ind]);
            wynik.Add(new Tuple<double, int>(max, liczba));
            max = 0;
            ind = 0;
        }
        void usun_plik() //usuwanie poprzedniego pliku z wynikami
        {
            try
            {
                File.Delete("wyniki.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void zapis() //zapisanie pliku z wynikami
        {
            usun_plik();
            using (StreamWriter wf = new StreamWriter("wyniki.txt", true))
            {
                foreach (var w in wynik)
                {
                    wf.WriteLine(w.Item1 + " " + w.Item2);
                }
                wf.Dispose();
            }
        }
    }
}