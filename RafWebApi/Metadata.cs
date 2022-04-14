using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RafWebApi
{
    public class Metadata
    {
        public int id { get; set; }
        public int movieId { get; set; }
        public string title { get; set; }
        public string language { get; set; }
        public string duration { get; set; }
        public int releaseYear { get; set; }

        public static List<Metadata> CSVreader()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\\..\\..\\metadata.csv");
            List<Metadata> values = File.ReadAllLines(path)
                               .Skip(1)
                               .Select(v => Metadata.FromCsv(v))
                               .ToList();
            return values;
        }

        public static void CSVWriter(List<Metadata> metadata)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\\..\\..\\metadata1.csv");

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Id,	MovieId	Title,	Language,	Duration,	ReleaseYear");

                foreach (var item in metadata)
                {
                    writer.WriteLine($"{item.id },{item.movieId},{item.title},{item.language},{item.duration},{item.releaseYear}");
                }
            }
        }

        public static Metadata FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Metadata metadata = new Metadata();
            try
            {
                metadata.id = Convert.ToInt32(values[0]);
                metadata.movieId = Convert.ToInt32(values[1]);
                metadata.title = values[2];
                metadata.language = values[3];
                metadata.duration = values[4];
                metadata.releaseYear = Convert.ToInt32(values[5]);
            }
            catch (Exception)
            {
                metadata.id = -1;
                metadata.title = csvLine;
            }
            return metadata;
        }

        public static List<Metadata> GetMetadataById(int id)
        {
            List<Metadata> metadata = new List<Metadata>();

            var allMetadata = Metadata.CSVreader();

            var selectedById = from amd in allMetadata where amd.movieId == id orderby amd.id descending select amd;

            var distinct = selectedById.GroupBy(p => new { p.movieId, p.language }).Select(g => g.First());

            var alpha = distinct.OrderBy(r => r.language).ToList();

            return alpha;
        }

        public static List<Metadata> GetMetadata4Stats()
        {
            List<Metadata> metadata = new List<Metadata>();

            var allMetadata = Metadata.CSVreader();

            var orderedAll = from amd in allMetadata orderby amd.id ascending select amd;

            var distinctMovies = (from Metadata in allMetadata
                                  select new
                                  {
                                      Metadata.movieId
                                  }).Distinct();
            foreach (var item in distinctMovies)
            {
                var disitnctMovie = orderedAll.ToList().Where(p => p.movieId == item.movieId && p.id != -1).First();
                metadata.Add(disitnctMovie);
            }

            return metadata;
        }
    }
}
