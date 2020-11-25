using System;

namespace CalendrierPisteScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter to scrap");
            Console.ReadLine();
            Scraper s = new Scraper();
            s.ScrapeWebsite($"https://www.calendrier-piste.fr/circuit/9-Ales");

            Console.WriteLine("Please enter to scrap");
            Console.ReadLine();
        }

    }

}