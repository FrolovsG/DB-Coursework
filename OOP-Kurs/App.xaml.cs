using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OOP_Kurs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void TagSearch<T>(List<T> list, ref DataGrid dataGrid, string input, Func<List<T>, Func<Predicate<T>, List<T>>, string, string, List<T>> switchFunc)
        {
            //separate parts of the search pattern into tokens
            var tokens = input.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

            List<T> matches = list;
            foreach (var token in tokens)
            {
                Func<Predicate<T>, List<T>> Find;
                if (token.Contains('~'))
                    Find = (Predicate<T> P) => matches.Union(list.FindAll(P).AsEnumerable()).ToList();
                else
                    Find = (Predicate<T> P) => matches.FindAll(P);

                var tag = new string(token.TakeWhile(ch => ch != ':').ToArray()).Trim(new char[] { ' ', '~' }).ToLower();
                var pattern = "^" + new string(token.Skip(token.IndexOf(':') + 1).ToArray()).Trim().Replace("?", "[a-zA-Z0-9\\/\\\\\\-\\s:]").Replace("*", "[a-zA-Z0-9\\/\\\\\\-\\s:]*") + "$";

                Console.WriteLine(token + " => " + tag + ", " + pattern);

                matches = switchFunc(matches, Find, tag, pattern);
            }

            dataGrid.ItemsSource = matches;
        }

    }
}
