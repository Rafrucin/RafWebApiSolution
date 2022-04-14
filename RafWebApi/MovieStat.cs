namespace RafWebApi
{
    public class MovieStat
    {
        public int movieId { get; set; }
        public string title { get; set; }
        public long averageWatchDurationS { get; set; }
        public int watches { get; set; }
        public int releaseYear { get; set; }

    }
}
