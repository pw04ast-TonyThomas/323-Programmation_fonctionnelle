using DataSeries;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Fil_Rouge_TonyThomas_323
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataPoint<ValorantMatch> ParseValorant(string[] cols)
            {
                DateTime time = DateTime.Parse(cols[0]);
                ValorantMatch match = new ValorantMatch(
                    cols[1],
                    cols[2],
                    int.Parse(cols[3]),
                    int.Parse(cols[4]),
                    int.Parse(cols[5]),
                    int.Parse(cols[6]),
                    int.Parse(cols[7]),
                    bool.Parse(cols[8])
                );
                return new DataPoint<ValorantMatch>(time, match);
            }

            DataPoint<Cs2Match> ParseCs2(string[] cols) 
            {
                DateTime time = DateTime.Parse(cols[0]);
                Cs2Match match = new Cs2Match(
                    cols[1],              // player
                    cols[2],              // map
                    cols[3],              // startSide (côté joué en 1re mi-temps — CT ou T)
                    int.Parse(cols[4]),   // kills
                    int.Parse(cols[5]),   // deaths
                    int.Parse(cols[6]),   // assists
                    int.Parse(cols[7]),   // mvps
                    bool.Parse(cols[8])   // won
                );
                return new DataPoint<Cs2Match>(time, match);
            }

            DataPoint<LolMatch> ParseLol(string[] cols)
            {
                DateTime time = DateTime.Parse(cols[0]);
                LolMatch match = new LolMatch(
                    cols[1],              // player
                    cols[2],              // champion
                    int.Parse(cols[4]),   // kills
                    int.Parse(cols[5]),   // deaths
                    int.Parse(cols[6]),   // assists
                    int.Parse(cols[7]),   // cs
                    int.Parse(cols[8]),   // visionScore
                    bool.Parse(cols[9])   // won
                );
                return new DataPoint<LolMatch>(time, match);
            } 

            var valorant = DataSeries<DataPoint<ValorantMatch>>.FromCsv("../../../data/valorant.csv", ParseValorant);
            var cs2 = DataSeries<DataPoint<Cs2Match>>.FromCsv("../../../data/cs2.csv", ParseCs2);
            var lol = DataSeries<DataPoint<LolMatch>>.FromCsv("../../../data/lol.csv", ParseLol);


            Console.WriteLine(valorant.Count);
            Console.WriteLine(lol.Count);
            Console.WriteLine(cs2.Count);
        }
    }
}
