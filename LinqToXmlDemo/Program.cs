using System;
using System.Linq;
using System.Xml.Linq;

namespace LinqToXmlDemo
{
    // Avem un fisier xml cu o lista de carti
    // de afisat numele autorilor impreuna cu informatia despre cartile lor (titlu, data publicarii, pret)
    // autorii trebuie afisati in ordine descrescatoare dupa numarul cartilor afisate si in ordine crescatoare dupa nume
    // cartile unui autor trebuie sa fie afisate in ordine crescatoare dupa data publicarii
    // de afisat doar cartile cu pretul mai mare de $5.0
    class Program
    {
        static void Main(string[] args)
        {
            XDocument document = XDocument.Load("books.xml");

            var query = document.Descendants("book")
                .Where(book => decimal.Parse(book.Element("price").Value) > 5)
                .GroupBy(book => book.Element("author").Value)
                .OrderByDescending(g => document.Descendants("book")
                                .Count(book => book.Element("author").Value == g.Key))
                .ThenBy(g => g.Key);

            foreach (var item in query)
            {
                Console.WriteLine(item.Key);

                foreach(var book in item.OrderBy(el => el.Element("publish_date").Value))
                {
                    Console.WriteLine(
                    $"\t{book.Element("title").Value,-40}" +
                    $"\t{book.Element("publish_date").Value,-15}" +
                    $"${decimal.Parse(book.Element("price").Value):0#.00}");
                }

                //Console.WriteLine($"\t{"Title",-40}{"Publish date",-15}{"Price",-3}");

                Console.WriteLine();
            }
        }
    }
}
