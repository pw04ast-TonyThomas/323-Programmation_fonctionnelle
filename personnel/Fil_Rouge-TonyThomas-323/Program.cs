using DataSeries;
using System;
using System.Linq;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.Arm;

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

            void exportCs2(DataSeries<DataPoint<Cs2Match>> matches, string path)
            {
                var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Map},{m.Value.StartSide},{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Mvps},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            void exportValorant(DataSeries<DataPoint<ValorantMatch>> matches, string path)
            {
                var header = "date,player,agent,kills,deaths,assists,headshots,rounds_won,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Agent},{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Headshots},{m.Value.RoundsWon},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            void exportLol(DataSeries<DataPoint<LolMatch>> matches, string path)
            {
                var header = "date,player,champion,role,kills,deaths,assists,cs,vision_score,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Champion},Support,{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Cs},{m.Value.VisionScore},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            var valorant = DataSeries<DataPoint<ValorantMatch>>.FromCsv("../../../data/valorant.csv", ParseValorant);
            var cs2 = DataSeries<DataPoint<Cs2Match>>.FromCsv("../../../data/cs2.csv", ParseCs2);
            var lol = DataSeries<DataPoint<LolMatch>>.FromCsv("../../../data/lol.csv", ParseLol);

            // Cs2 Raphael
            DataSeries<DataPoint<Cs2Match>> raphaelGenerated = MatchGenerator.GenerateCs2("Raphael", 20);
            DataSeries<DataPoint<Cs2Match>> raphaelValid = DataSeries<DataPoint<Cs2Match>>.From(raphaelGenerated.Values.Where(cs2Match => cs2Match.Value.Kills + cs2Match.Value.Assists <= 50 && cs2Match.Value.Deaths >= 1));
            // Valorant Dylan
            DataSeries<DataPoint<ValorantMatch>> dylanGenerated = MatchGenerator.GenerateValorant("Dylan", 20);
            DataSeries<DataPoint<ValorantMatch>> dylanValid = DataSeries<DataPoint<ValorantMatch>>.From(dylanGenerated.Values.Where(valorantMatch => valorantMatch.Value.Kills + valorantMatch.Value.Assists <= 50 && valorantMatch.Value.Deaths >= 1));
            // Lol Noé
            DataSeries<DataPoint<LolMatch>> noeGenerated = MatchGenerator.GenerateLol("Noe", 20);
            DataSeries<DataPoint<LolMatch>> noeValid = DataSeries<DataPoint<LolMatch>>.From(noeGenerated.Values.Where(lolmatch => lolmatch.Value.Kills + lolmatch.Value.Assists <= 50 && lolmatch.Value.Deaths >= 1));

            exportCs2(raphaelValid, "../../../data/Cs2Raphael.csv");
            exportValorant(dylanValid, "../../../data/ValorantDylan.csv");
            exportLol(noeValid, "../../../data/LolNoe.csv");




            Console.WriteLine($"Avant : {raphaelGenerated.Count}, après : {raphaelValid.Count}");

            Console.WriteLine(valorant.Count);
            Console.WriteLine(lol.Count);
            Console.WriteLine(cs2.Count);
        }
    }
}
