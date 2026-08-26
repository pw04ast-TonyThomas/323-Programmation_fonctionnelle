namespace Fil_Rouge_TonyThomas_323
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            ValorantMatch[] valorantMatches = new[]
            {
                new ValorantMatch("Léa", "Jett",  18, 6, 4, 8,  13, true),
                new ValorantMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false),
                new ValorantMatch("Léa", "Neon",  20, 7, 5,  9, 13, true),
            };

            LolMatch[] lolMatches = new[]
            {
                new LolMatch("Léa", "Jett",  18, 6, 4, 8,  13, true),
                new LolMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false),
                new LolMatch("Léa", "Neon",  20, 7, 5,  9, 13, true),
            };

            DataSeries.DataSeries<ValorantMatch> valorantMatch = DataSeries.DataSeries<ValorantMatch>.From(valorantMatches);
            DataSeries.DataSeries<LolMatch> lolMatch = DataSeries.DataSeries<LolMatch>.From(lolMatches);

            Console.WriteLine(valorantMatch.Count);
            Console.WriteLine(lolMatch.Count);
        }
    }
}
