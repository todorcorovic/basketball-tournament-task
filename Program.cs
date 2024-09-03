using Olimpijske_igre.Models;
using Olimpijske_igre.Services;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Učitaj grupne podatke
        var groups = JsonLoader.LoadGroups("groups.json");
        Dictionary<string, List<Team>> groupTeams = new Dictionary<string, List<Team>>();

        foreach (var group in groups)
        {
            groupTeams[group.Key] = group.Value;
        }

        // Učitaj prijateljske utakmice
        var exhibitionMatches = JsonLoader.LoadExhibitionMatches("exibitions.json");

        // Prikaz prijateljskih utakmica za svaki tim i inicijalno postavljanje forme
        foreach (var team in exhibitionMatches)
        {
            /* Ispis svih prijateljskih meceva
            Console.WriteLine($"Team: {team.Key}");
            foreach (var match in team.Value)
            {
                Console.WriteLine($"{match.Date} - {match.Opponent}: {match.Result}");
            }
            */

            foreach (var group in groupTeams)
            {
                foreach (var groupTeam in group.Value)
                {
                    if (groupTeam.ISOCode == team.Key)
                    {
                        groupTeam.CalculateInitialForm(exhibitionMatches[team.Key]);

                        // Prikaz forme nakon inicijalnog postavljanja
                        //Console.WriteLine($"Initial Form for {groupTeam.Name}: {groupTeam.Form:F2}");
                    }
                }
            }
        }

        // Simulacija grupne faze turnira
        TournamentSimulator.SimulateGroupPhase(groupTeams);

        // Simulacija zreba i eleminacione faze turnira
        var topTeams = TournamentSimulator.GetTopTeams(groupTeams);
        TournamentDraw.ConductDraw(topTeams);


        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
