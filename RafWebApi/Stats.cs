using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RafWebApi
{
    public class Stats
    {
        public int movieId { get; set; }
        public long watchDurationMs { get; set; }
        public long averageWatchDurationS { get; set; }
        public int watches { get; set; }

        public static List<Stats> CSVreader()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\\..\\..\\stats.csv");
            List<Stats> values = File.ReadAllLines(path)
                               .Skip(1)
                               .Select(v => Stats.FromCsv(v))
                               .ToList();
            return values;
        }


        public static Stats FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Stats stats = new Stats();
            try
            {
                stats.movieId = Convert.ToInt32(values[0]);
                stats.watchDurationMs = Convert.ToInt64(values[1]);
            }
            catch (Exception)
            {
                stats.movieId = -1;
                stats.watchDurationMs = -1;
            }
            return stats;
        }

        public static List<Stats> StatsAvg()
        {
            var allStats = Stats.CSVreader();

            var res1 = from Movies in allStats
                       group Movies by new
                       {
                           Movies.movieId
                       } into g
                       select new Stats
                       {
                           movieId = g.Key.movieId,
                           watches = g.Count(p => p.movieId != null),
                           averageWatchDurationS = (long)(double?)g.Average(p => p.watchDurationMs)
                       };

            return res1.ToList();
        }
    }
}
