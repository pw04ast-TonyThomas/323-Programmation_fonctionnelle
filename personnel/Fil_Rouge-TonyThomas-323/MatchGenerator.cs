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
    }
}
