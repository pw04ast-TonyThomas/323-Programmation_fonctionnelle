namespace Fil_Rouge_TonyThomas_323
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            DataSeries.DataSeries<ValorantMatch> valorant = DataSeries.DataSeries<ValorantMatch>.From(new[]
            {
                new ValorantMatch("Léa", "Jett",  18, 6, 4, 8,  13, true),
                new ValorantMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false),
                new ValorantMatch("Léa", "Neon",  20, 7, 5,  9, 13, true),
            });

            DataSeries.DataSeries<LolMatch> lol = DataSeries.DataSeries<LolMatch>.From(new[]
            {
                new LolMatch("Léa", "Jett",  18, 6, 4, 8,  13, true),
                new LolMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false),
                new LolMatch("Léa", "Neon",  20, 7, 5,  9, 13, true),
            });

            DataSeries.DataSeries<Cs2Match> cs2 = DataSeries.DataSeries<Cs2Match>.From(new[]
{
                new Cs2Match("Léa", "Jett",  "CT", 6, 4, 8,  13, true),
                new Cs2Match("Léa", "Reyna", "CT", 8, 2, 11,  9, false),
                new Cs2Match("Léa", "Neon",  "T", 7, 5,  9, 13, true),
            });

            Console.WriteLine(valorant.Count);
            Console.WriteLine(lol.Count);
            Console.WriteLine(cs2.Count);
        }
    }
}
