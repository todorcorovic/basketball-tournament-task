using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Olimpijske_igre.Models
{
    public class Team
    {
        [JsonPropertyName("Team")]
        public string Name { get; set; }

        [JsonPropertyName("ISOCode")]
        public string ISOCode { get; set; } 

        [JsonPropertyName("FIBARanking")]
        public int FIBARanking { get; set; } 

        public int Points { get; private set; }
        public int PointsScored { get; private set; }
        public int PointsAgainst { get; private set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }

        public int PointsDifference => PointsScored - PointsAgainst;

        public double Form { get; set; }
        
        public void AddMatchResult(int pointsScored, int pointsAgainst, bool isWin)
        {
            PointsScored += pointsScored;
            PointsAgainst += pointsAgainst;
            if (isWin)
            {
                Points += 2;
                Wins++;
            }
            else
            {
                Points += 1;
                Losses++;
            }
        }

        public void CalculateInitialForm(List<ExhibitionMatch> exhibitionMatches)
        {
            int totalPoints = 0;

            foreach (var match in exhibitionMatches)
            {
                var resultParts = match.Result.Split('-');
                int scoreFor = int.Parse(resultParts[0]);
                int scoreAgainst = int.Parse(resultParts[1]);
                int scoreDifference = scoreFor - scoreAgainst;

                // Prilagodi bodove na osnovu rezultata, manje se forme 
                if (scoreDifference > 0)
                {
                    // Pobeda - veći uticaj na formu
                    totalPoints += scoreDifference / 2;
                }
                else
                {
                    // Poraz - manji uticaj na formu
                    totalPoints += scoreDifference / 4;
                }

                // Dodaj ispis za svaki meč i rezultat bodova
                //Console.WriteLine($"Match: {match.Opponent}, Result: {match.Result}, Score Difference: {scoreDifference}, Points: {totalPoints}");
            }

            // Postavi formu na osnovu ukupnih bodova
            Form = totalPoints;

            // Ispis za potvrdu postavljanja forme
            //Console.WriteLine($"Final Form for {Name}: {Form:F2}");
        }

        public void UpdateFormAfterMatch(Team opponent, int score, int opponentScore)
        {
            double scoreDifference = score - opponentScore;
            double points = 0;

            if (scoreDifference > 0)
            {
                // Pobeda
                points = scoreDifference / 2.0;
                opponent.Form -= scoreDifference / 4.0;
            }
            else
            {
                // Poraz
                points = -(-scoreDifference) / 4.0;
                opponent.Form += (-scoreDifference) / 2.0;
            }

            // Ažuriraj formu za trenutni tim
            this.Form += points;

            // Ograniči formu između 0.0 i 100.0
            this.Form = Math.Clamp(this.Form, 0.0, 100.0);
            opponent.Form = Math.Clamp(opponent.Form, 0.0, 100.0);

            // Ispis rezultata i forme 
            //Console.WriteLine($"Match: {this.Name} vs {opponent.Name}, Score: {score}-{opponentScore}");
            //Console.WriteLine($"{this.Name} Points: {points}, New Form: {this.Form:F2}");
            //Console.WriteLine($"{opponent.Name} Points: {-points}, New Form: {opponent.Form:F2}");
        }


    }
}
