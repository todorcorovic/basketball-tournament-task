using Olimpijske_igre.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Olimpijske_igre.Services
{
    public class TournamentDraw
    {
        private static Random _random = new Random();

        public static void ConductDraw(List<Team> topTeams)
        {
            var sortedTeams = topTeams.OrderByDescending(t => t.Points)
                                      .ThenByDescending(t => t.PointsDifference)
                                      .ThenByDescending(t => t.PointsScored)
                                      .ToList();

            // Sortiranje timova po šeširima 
            var potD = sortedTeams.Take(2).ToList(); //  1-2
            var potE = sortedTeams.Skip(2).Take(2).ToList(); //  3-4
            var potF = sortedTeams.Skip(4).Take(2).ToList(); //  5-6
            var potG = sortedTeams.Skip(6).Take(2).ToList(); //  7-8

            Console.WriteLine("\nŠeširi:");
            PrintPot("D", potD);
            PrintPot("E", potE);
            PrintPot("F", potF);
            PrintPot("G", potG);

            // Četvrtfinale 
            var quarterFinalPairs = new List<Tuple<Team, Team>>();
            quarterFinalPairs.AddRange(FormQuarterFinalPairs(potD, potG));
            quarterFinalPairs.AddRange(FormQuarterFinalPairs(potE, potF));
            Console.WriteLine("\nČetvrtfinale:");
            var quarterFinalWinners = SimulateAndDisplayMatches(quarterFinalPairs, true);

            // Polufinale
            var semiFinalPairs = FormPairsForNextStage(quarterFinalWinners);
            Console.WriteLine("\nPolufinale:");
            var (semiFinalWinners, semiFinalLosers) = SimulateAndDisplaySemiFinals(semiFinalPairs);

            // Treće mesto
            var thirdPlaceMatch = new Tuple<Team, Team>(semiFinalLosers[0], semiFinalLosers[1]);
            Console.WriteLine("\nUtakmica za treće mesto:");
            var thirdPlaceWinner = SimulateAndDisplayMatch(thirdPlaceMatch);

            // Finale
            var finalMatch = new Tuple<Team, Team>(semiFinalWinners[0], semiFinalWinners[1]);
            Console.WriteLine("\nFinale:");
            var finalWinner = SimulateAndDisplayMatch(finalMatch);

            // Prikaz medalja
            Console.WriteLine("\nMedalje:");
            Console.WriteLine($"\t1. {finalWinner.Name}");
            Console.WriteLine($"\t2. {(finalWinner == finalMatch.Item1 ? finalMatch.Item2.Name : finalMatch.Item1.Name)}");
            Console.WriteLine($"\t3. {thirdPlaceWinner.Name}");
        }

        private static void PrintPot(string potName, List<Team> teams)
        {
            Console.WriteLine($"\tŠešir {potName}");
            foreach (var team in teams)
            {
                Console.WriteLine($"\t\t{team.Name}");
            }
        }

        private static List<Tuple<Team, Team>> FormQuarterFinalPairs(List<Team> pot1, List<Team> pot2)
        {
            var pairs = new List<Tuple<Team, Team>>();

            while (pot1.Any() && pot2.Any())
            {
                var team1 = pot1[_random.Next(pot1.Count)];
                var team2 = pot2[_random.Next(pot2.Count)];

                 if (!pairs.Any(p => p.Item1 == team1 || p.Item2 == team1 || p.Item1 == team2 || p.Item2 == team2))
                {
                    pairs.Add(new Tuple<Team, Team>(team1, team2));
                    pot1.Remove(team1);
                    pot2.Remove(team2);
                }
            }

            return pairs;
        }

        private static List<Team> SimulateAndDisplayMatches(List<Tuple<Team, Team>> pairs, bool includeResults)
        {
            var winners = new List<Team>();

            foreach (var pair in pairs)
            {
                var match = new Match { Team1 = pair.Item1, Team2 = pair.Item2 };
                var winner = match.Simulate();
                var loser = winner == pair.Item1 ? pair.Item2 : pair.Item1;

                if (includeResults)
                {
                    Console.WriteLine($"\t{pair.Item1.Name} vs {pair.Item2.Name} - Result: {match.ScoreTeam1}:{match.ScoreTeam2}");
                }

                pair.Item1.UpdateFormAfterMatch(pair.Item2, match.ScoreTeam1, match.ScoreTeam2);

                winners.Add(winner);
            }

            return winners;
        }

        private static List<Tuple<Team, Team>> FormPairsForNextStage(List<Team> winners)
        {
            var pairs = new List<Tuple<Team, Team>>();

            for (int i = 0; i < winners.Count; i += 2)
            {
                if (i + 1 < winners.Count)
                {
                    pairs.Add(new Tuple<Team, Team>(winners[i], winners[i + 1]));
                }
            }

            return pairs;
        }

        private static (List<Team> Winners, List<Team> Losers) SimulateAndDisplaySemiFinals(List<Tuple<Team, Team>> pairs)
        {
            var winners = new List<Team>();
            var losers = new List<Team>();

            foreach (var pair in pairs)
            {
                var match = new Match { Team1 = pair.Item1, Team2 = pair.Item2 };
                var winner = match.Simulate();
                var loser = winner == pair.Item1 ? pair.Item2 : pair.Item1;
                Console.WriteLine($"\t{pair.Item1.Name} vs {pair.Item2.Name} - Result: {match.ScoreTeam1}:{match.ScoreTeam2}");
                
                pair.Item1.UpdateFormAfterMatch(pair.Item2, match.ScoreTeam1, match.ScoreTeam2);
                winners.Add(winner);
                losers.Add(loser);
            }

            return (winners, losers);
        }

        private static Team SimulateAndDisplayMatch(Tuple<Team, Team> match)
        {
            var matchSimulation = new Match { Team1 = match.Item1, Team2 = match.Item2 };
            var winner = matchSimulation.Simulate();
            var loser = winner == match.Item1 ? match.Item2 : match.Item1;
            Console.WriteLine($"\t{match.Item1.Name} vs {match.Item2.Name} - Result: {matchSimulation.ScoreTeam1}:{matchSimulation.ScoreTeam2}");
            
            // Ažuriraj formu oba tima
            match.Item1.UpdateFormAfterMatch(match.Item2, matchSimulation.ScoreTeam1, matchSimulation.ScoreTeam2);
            return winner;
        }
    }
}
