using Olimpijske_igre.Models;
using System;

public class Match
{
    private static Random _random = new Random();

    public Team Team1 { get; set; }
    public Team Team2 { get; set; }
    public int ScoreTeam1 { get; private set; }
    public int ScoreTeam2 { get; private set; }
    public Team Winner { get; private set; }

    public Team Simulate()
    {
        // Izračunavanje verovatnoće pobede za tim1
        double team1WinProbability = CalculateWinProbability(Team1, Team2);

        // Generisanje rezultata sa minimalnim razlikama
        ScoreTeam1 = _random.Next(80, 101); // Nasumičan rezultat između 80 i 100
        ScoreTeam2 = _random.Next(80, 101); // Nasumičan rezultat između 80 i 100

        // Prilagođavanje rezultata na osnovu verovatnoće pobede
        if (_random.NextDouble() < team1WinProbability)
        {
            // Tim1 je verovatnije da pobedi
            ScoreTeam1 = Math.Max(ScoreTeam2 + _random.Next(1, 10), ScoreTeam1);
        }
        else
        {
            // Tim2 je verovatnije da pobedi
            ScoreTeam2 = Math.Max(ScoreTeam1 + _random.Next(1, 10), ScoreTeam2);
        }

        // Određivanje pobednika
        Winner = ScoreTeam1 > ScoreTeam2 ? Team1 : Team2;

        // Ažuriranje rezultata i forme timova
        UpdateTeamsAfterMatch();

        // Vraćanje pobednika
        return Winner;
    }

    private double CalculateWinProbability(Team team1, Team team2)
    {
        // Normalizacija FIBA rangiranja
        double rankScore1 = 1.0 / team1.FIBARanking;
        double rankScore2 = 1.0 / team2.FIBARanking;

        // Kombinovanje rangiranja i forme
        double team1Score = 0.7 * rankScore1 + 0.3 * team1.Form;
        double team2Score = 0.7 * rankScore2 + 0.3 * team2.Form;

        // Izračunavanje verovatnoće pobede
        double probability = team1Score / (team1Score + team2Score);
        return Math.Max(0.05, Math.Min(probability, 0.95)); // Ograničavanje između 0.05 i 0.95
    }

    private void UpdateTeamsAfterMatch()
    {
        bool isTeam1Winner = Winner == Team1;

        // Ažuriranje rezultata za Team1
        Team1.AddMatchResult(ScoreTeam1, ScoreTeam2, isTeam1Winner);

        // Ažuriranje rezultata za Team2
        Team2.AddMatchResult(ScoreTeam2, ScoreTeam1, !isTeam1Winner);
    }

}
