using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace CalendrierPisteScraping
{
    class Scraper
    {
        public Scraper()
        {

        }

        public async void ScrapeWebsite(string siteUrl)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);
            var ids = document.All.Select(x => x.Id).ToList();

            var li = document.All.Where(m => m.Id == "orgaTrackdayLst").FirstOrDefault();
            var records = new List<InfoRoulage>();

            foreach (var item in li.Children)
            {
                InfoRoulage info = new InfoRoulage();
                info.Orga = item.GetElementsByClassName("title").FirstOrDefault().InnerHtml;
                info.Date = item.GetElementsByClassName("date").FirstOrDefault().InnerHtml;
                info.Jours = item.GetElementsByClassName("dayNb").FirstOrDefault().InnerHtml;
                info.Groupes = item.GetElementsByClassName("groupNb").FirstOrDefault().InnerHtml;
                info.Prix = item.GetElementsByClassName("price").FirstOrDefault().InnerHtml;

                records.Add(info);
            }

            using (var writer = new StreamWriter("InfoRoulage.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }

        private void GetScrapeResults(IHtmlDocument document)
        {
            //IEnumerable<IElement> articleLink;


            //foreach (var term in QueryTerms)
            //{
            //    articleLink = document.All.Where(x => x.ClassName == "views-field views-field-nothing" && (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower())));
            //}

            //if (articleLink.Any())
            //{
            //    // Print Results: See Next Step
            //}
        }
    }
}
