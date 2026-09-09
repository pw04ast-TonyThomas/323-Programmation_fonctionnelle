using DataSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fil_Rouge_TonyThomas_323
{
    public static class MatchGenerator
    {
        public static DataSeries<DataPoint<Cs2Match>> GenerateCs2(string player, int count, int seed = 42)
        {
            Random rng = new Random(seed);
            string[] maps = new[] { "Dust2", "Mirage", "Inferno", "Nuke", "Ancient" };
            string[] sides = new[] { "CT", "T" };
            var start = new DateTime(2023, 9, 1); // début de la pré-saison

            return DataSeries<DataPoint<Cs2Match>>.From(
                Enumerable.Range(1, count)
                    .Select(i => new DataPoint<Cs2Match>(
                        start.AddDays(i),
                        new Cs2Match(
                            player, 
                            maps[rng.Next(maps.Length)], 
                            sides[rng.Next(sides.Length)], 
                            rng.Next(10, 28), 
                            rng.Next(6, 18), 
                            rng.Next(0, 8), 
                            rng.Next(0, 5), 
                            rng.Next(2) == 1)
                    ))
            );
        }

        public static DataSeries<DataPoint<ValorantMatch>> GenerateValorant(string player, int count, int seed = 42)
        {
            Random rng = new Random(seed);

            string[] agents =
            {
                "Jett",
                "Omen",
                "Reyna",
                "Astra",
                "Brimstone",
                "Neon",
                "Viper"
            };

            var start = new DateTime(2024, 1, 1);

            return DataSeries<DataPoint<ValorantMatch>>.From(
                Enumerable.Range(1, count)
                    .Select(i =>
                    {
                        int kills = rng.Next(9, 26);
                        int deaths = rng.Next(5, 14);
                        int assists = rng.Next(2, 12);
                        int headshots = rng.Next(2, Math.Min(kills, 14) + 1);
                        int roundsWon = rng.Next(5, 14);

                        return new DataPoint<ValorantMatch>(
                            start.AddDays(i),
                            new ValorantMatch(
                                player,
                                agents[rng.Next(agents.Length)],
                                kills,
                                deaths,
                                assists,
                                headshots,
                                roundsWon,
                                rng.Next(2) == 1
                            )
                        );
                    })
            );
        }

        public static DataSeries<DataPoint<LolMatch>> GenerateLol(string player, int count, int seed = 42)
        {
            Random rng = new Random(seed);

            string[] champions =
            {
                "Thresh",
                "Nautilus",
                "Lulu",
                "Soraka",
                "Leona",
                "Blitzcrank",
                "Janna"
            };

            var start = new DateTime(2024, 1, 1);

            return DataSeries<DataPoint<LolMatch>>.From(
                Enumerable.Range(1, count)
                    .Select(i =>
                    {
                        int kills = rng.Next(0, 4);
                        int deaths = rng.Next(3, 8);
                        int assists = rng.Next(14, 26);
                        int cs = rng.Next(32, 48);
                        int visionScore = rng.Next(52, 83);

                        return new DataPoint<LolMatch>(
                            start.AddDays(i),
                            new LolMatch(
                                player,
                                champions[rng.Next(champions.Length)],
                                kills,
                                deaths,
                                assists,
                                cs,
                                visionScore,
                                rng.Next(2) == 1
                            )
                        );
                    })
            );
        }
    }
}
