using DataSeries;
using System;
using System.Linq;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks.Sources;

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

            void ExportCs2(DataSeries<DataPoint<Cs2Match>> matches, string path)
            {
                var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Map},{m.Value.StartSide},{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Mvps},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            void ExportValorant(DataSeries<DataPoint<ValorantMatch>> matches, string path)
            {
                var header = "date,player,agent,kills,deaths,assists,headshots,rounds_won,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Agent},{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Headshots},{m.Value.RoundsWon},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            void ExportLol(DataSeries<DataPoint<LolMatch>> matches, string path)
            {
                var header = "date,player,champion,role,kills,deaths,assists,cs,vision_score,won";
                var lines = matches.Values.Select(m => $"{m.Timestamp:yyyy-MM-dd},{m.Value.Player},{m.Value.Champion},Support,{m.Value.Kills},{m.Value.Deaths},{m.Value.Assists},{m.Value.Cs},{m.Value.VisionScore},{m.Value.Won.ToString().ToLower()}");
                File.WriteAllLines(path, lines.Prepend(header));
            }

            // Import matches
            var valorant = DataSeries<DataPoint<ValorantMatch>>.FromCsv("../../../data/valorant.csv", ParseValorant);
            var cs2 = DataSeries<DataPoint<Cs2Match>>.FromCsv("../../../data/cs2.csv", ParseCs2);
            var lol = DataSeries<DataPoint<LolMatch>>.FromCsv("../../../data/lol.csv", ParseLol);

            //// Find outliers
            //var badValorant = valorant.Outliers(m => m.Value.Kills < 20);
            //Console.WriteLine(valorant.Values.Count());
            //Console.WriteLine(badValorant.Values.Count());

            // Sanitize
            var sanitizedValorant = valorant.Sanitize(m => m.Value.Kills < 0 || m.Value.Kills > 50 || m.Value.Deaths < 0 || m.Value.Deaths > 30 || m.Value.Assists < 0);
            //Console.WriteLine(valorant.Values.Count());
            //Console.WriteLine(sanitizedValorant.Values.Count());

            var sanitizedCs2 = cs2.Sanitize(m => m.Value.Kills + m.Value.Assists > 50 || m.Value.Deaths < 0);
            //Console.WriteLine(cs2.Values.Count());
            //Console.WriteLine(sanitizedCs2.Values.Count());

            var sanitizedLol = lol.Sanitize(m => m.Value.Kills > 10 || m.Value.Deaths < 1 || m.Value.Assists < 0 || m.Value.Cs < 0);
            //Console.WriteLine(lol.Values.Count());
            //Console.WriteLine(sanitizedLol.Values.Count());


            // Find Args and use them
            string? gameArgs = args.Contains("--game") ? args[Array.IndexOf(args, "--game") + 1] : null;
            string? playerArgs = args.Contains("--player") ? args[Array.IndexOf(args, "--player") + 1] : "all";
            string? filterMode = args.Contains("--filter") ? args[Array.IndexOf(args, "--filter") + 1] : "all";

            if (gameArgs == null || playerArgs == null) 
            { 
                Console.WriteLine("Please provide both the game (cs2, valorant, lol) and the player's name."); 
                return; 
            }

            if (gameArgs.ToLower() == "cs2")
            {
                DataSeries<DataPoint<Cs2Match>> series = MatchGenerator.GenerateCs2(playerArgs, 20);
                DataSeries<DataPoint<Cs2Match>> seriesValid = DataSeries<DataPoint<Cs2Match>>.From(series.Values.Where(cs2Match => cs2Match.Value.Kills + cs2Match.Value.Assists <= 50 && cs2Match.Value.Deaths >= 1));
                ExportCs2(seriesValid, $"../../../data/{playerArgs.ToLower()}_generated.csv");
            }
            else if (gameArgs.ToLower() == "valorant")
            {
                DataSeries<DataPoint<ValorantMatch>> series = MatchGenerator.GenerateValorant(playerArgs, 20);
                DataSeries<DataPoint<ValorantMatch>> seriesValid = DataSeries<DataPoint<ValorantMatch>>.From(series.Values.Where(valorantMatch => valorantMatch.Value.Kills + valorantMatch.Value.Assists <= 50 && valorantMatch.Value.Deaths >= 1));
                ExportValorant(seriesValid, $"../../../data/{playerArgs.ToLower()}_generated.csv");
            }
            else if (gameArgs.ToLower() == "lol")
            {
                DataSeries<DataPoint<LolMatch>> series = MatchGenerator.GenerateLol(playerArgs, 20);
                DataSeries<DataPoint<LolMatch>> seriesValid = DataSeries<DataPoint<LolMatch>>.From(series.Values.Where(valorantMatch => valorantMatch.Value.Kills + valorantMatch.Value.Assists <= 50 && valorantMatch.Value.Deaths >= 1));
                ExportLol(seriesValid, $"../../../data/{playerArgs.ToLower()}_generated.csv");
            }
            Console.WriteLine($"{playerArgs} : données générées et exportées");
        }
    }
}
