using DataSeries;

namespace Fil_Rouge_TonyThomas_323
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            DataSeries.DataSeries<DataPoint<ValorantMatch>> valorant = DataSeries.DataSeries<DataPoint<ValorantMatch>>.From(new[]
            {
                new DataPoint<ValorantMatch>(DateTime.Now, new ValorantMatch("Léa", "Jett",  18, 6, 4, 8,  13, true)),
                new DataPoint<ValorantMatch>(DateTime.Now, new ValorantMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false)),
                new DataPoint<ValorantMatch>(DateTime.Now, new ValorantMatch("Léa", "Neon",  20, 7, 5,  9, 13, true)),
            });

            DataSeries.DataSeries<DataPoint<LolMatch>> lol = DataSeries.DataSeries<DataPoint<LolMatch>>.From(new[]
            {
                new DataPoint<LolMatch>(DateTime.Now, new LolMatch("Léa", "Jett",  18, 6, 4, 8,  13, true)),
                new DataPoint<LolMatch>(DateTime.Now, new LolMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false)),
                new DataPoint<LolMatch>(DateTime.Now, new LolMatch("Léa", "Neon",  20, 7, 5,  9, 13, true)),
            });

            DataSeries.DataSeries<DataPoint<Cs2Match>> cs2 = DataSeries.DataSeries < DataPoint<Cs2Match>>.From(new[]
{
                new DataPoint<Cs2Match>(DateTime.Now, new Cs2Match("Léa", "Jett",  "CT", 6, 4, 8,  13, true)),
                new DataPoint<Cs2Match>(DateTime.Now, new Cs2Match("Léa", "Reyna", "CT", 8, 2, 11,  9, false)),
                new DataPoint<Cs2Match>(DateTime.Now, new Cs2Match("Léa", "Neon",  "T", 7, 5,  9, 13, true)),
            });

            Console.WriteLine(valorant.Count);
            Console.WriteLine(lol.Count);
            Console.WriteLine(cs2.Count);
        }
    }
}
