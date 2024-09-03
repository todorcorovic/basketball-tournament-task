using Olimpijske_igre.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Olimpijske_igre.Services
{
    public class TournamentSimulator
    {
        private static Random _random = new Random();

        public static void SimulateGroupPhase(Dictionary<string, List<Team>> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine($"Grupna faza:");
                Console.WriteLine($"Grupa {group.Key}:");

                var teams = group.Value;
                var matchResults = new List<string>();

                // Simulacija svih mečeva u grupi
                for (int i = 0; i < teams.Count; i++)
                {
                    for (int j = i + 1; j < teams.Count; j++)
                    {
                        var team1 = teams[i];
                        var team2 = teams[j];

                        if (team1 == null || team2 == null)
                        {
                            Console.WriteLine("Greška: Jedan od timova je null.");
                            continue;
                        }

                        // Kreiranje instance meča i simulacija meča
                        var match = new Match
                        {
                            Team1 = team1,
                            Team2 = team2
                        };

                        var winner = match.Simulate();
                        var loser = winner == team1 ? team2 : team1;

                        // Dodavanje rezultata meča timovima
                        team1.AddMatchResult(match.ScoreTeam1, match.ScoreTeam2, winner == team1);
                        team2.AddMatchResult(match.ScoreTeam2, match.ScoreTeam1, winner == team2);

                        // Ažuriranje forme nakon meča
                        team1.UpdateFormAfterMatch(team2, match.ScoreTeam1, match.ScoreTeam2);

                        matchResults.Add($"{team1.Name.PadRight(20)} - {team2.Name.PadRight(20)} ({match.ScoreTeam1}:{match.ScoreTeam2})");
                    }
                }

                // Ispisivanje rezultata mečeva
                foreach (var result in matchResults)
                {
                    Console.WriteLine(result);
                }

                // Ispisivanje konačnog plasmana
                PrintStandings(group.Key, teams);
            }

            // Određivanje i ispisivanje top 8 timova koji prolaze u eliminacionu fazu
            PrintTopTeams(groups);
        }

        public static List<Team> GetTopTeams(Dictionary<string, List<Team>> groups)
        {
            return groups
                .SelectMany(g => g.Value)
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.PointsDifference)
                .ThenByDescending(t => t.PointsScored)
                .Take(8)
                .ToList();
        }

        private static void PrintStandings(string groupName, List<Team> teams)
        {
            var sortedTeams = teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.PointsDifference)
                .ThenByDescending(t => t.PointsScored)
                .ToList();

            Console.WriteLine($"\nKonačan plasman u grupi {groupName}:");
            Console.WriteLine($"{"Pozicija".PadRight(5)}{"Tim".PadRight(20)}{"Pobede / Porazi / Bodovi / Postignuti Koševi / Primljeni Koševi / Koš Razlika / Forma".PadRight(30)}");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < sortedTeams.Count; i++)
            {
                var team = sortedTeams[i];
                Console.WriteLine($"{(i + 1).ToString().PadRight(5)}{team.Name.PadRight(20)}{team.Wins} / {team.Losses} / {team.Points} / {team.PointsScored} / {team.PointsAgainst} / {team.PointsDifference} / {team.Form:F2}".PadRight(30));
            }
            Console.WriteLine("\n");
        }

        private static void PrintTopTeams(Dictionary<string, List<Team>> groups)
        {
            var topTeams = GetTopTeams(groups);

            Console.WriteLine("\nTop 8 timova koji su prošli dalje:");
            Console.WriteLine($"{"Tim".PadRight(20)}{"Bodovi".PadRight(10)}");
            Console.WriteLine(new string('-', 30));

            foreach (var team in topTeams)
            {
                Console.WriteLine($"{team.Name.PadRight(20)}{team.Points.ToString().PadRight(10)}");
            }
        }

    }
}
