using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Core.Contracts;
using FootballManager.Models;
using FootballManager.Models.Contracts;
using FootballManager.Repositories;
using FootballManager.Utilities.Messages;

namespace FootballManager.Core
{
    public class Controller : IController
    {
        private readonly string[] managerTypes = new string[]
        {
            typeof(AmateurManager).Name,
            typeof(SeniorManager).Name,
            typeof(ProfessionalManager).Name
        };
        private TeamRepository championship;

        public Controller()
        {
            this.championship = new TeamRepository();
        }
        public string ChampionshipRankings()
        {
            var sortedTeams = this.championship.Models.OrderByDescending(t => t.ChampionshipPoints).ThenByDescending(t => t.PresentCondition).ToList();
            int counter = 1;
            StringBuilder result = new StringBuilder();
            result.AppendLine("***Ranking Table***");
            foreach (var team in sortedTeams)
            {
                result.AppendLine($"{counter++}. {team}/{team.TeamManager}");
            }

            return result.ToString().TrimEnd();

        }

        public string JoinChampionship(string teamName)
        {
            if (this.championship.Capacity == this.championship.Models.Count)
            {
                return OutputMessages.ChampionshipFull;
            }

            if (this.championship.Exists(teamName))
            {
                return string.Format(OutputMessages.TeamWithSameNameExisting, teamName);
            }

            var team = new Team(teamName);
            this.championship.Add(team);

            return string.Format(OutputMessages.TeamSuccessfullyJoined, teamName);
        }

        public string MatchBetween(string teamOneName, string teamTwoName)
        {
            if (!championship.Exists(teamOneName) || !championship.Exists(teamTwoName))
            {
                return string.Format(OutputMessages.OneOfTheTeamDoesNotExist);
            }

            var teamOne = this.championship.Get(teamOneName);
            var teamTwo = this.championship.Get(teamTwoName);

            var winner = teamOne.PresentCondition > teamTwo.PresentCondition 
                ? teamOne
                : teamTwo;

            ITeam loser;

            if (winner == teamOne)
            {
                loser = teamTwo;
            }
            else if (winner == teamTwo)
            {
                loser = teamOne;
            }
            else
            {
                teamOne.GainPoints(1); teamTwo.GainPoints(1);

                return string.Format(OutputMessages.MatchIsDraw, teamOneName, teamTwoName);
            }
            winner.GainPoints(3);

            winner.TeamManager?.RankingUpdate(5);
            loser.TeamManager?.RankingUpdate(-5);

            return string.Format(OutputMessages.TeamWinsMatch, winner.Name, loser.Name);


        }

        public string PromoteTeam(string droppingTeamName, string promotingTeamName, string managerTypeName, string managerName)
        {
            if (!this.championship.Exists(droppingTeamName))
            {
                return string.Format(OutputMessages.DroppingTeamDoesNotExist, droppingTeamName);
            }
            if (this.championship.Exists(promotingTeamName))
            {
                return string.Format(OutputMessages.TeamWithSameNameExisting, promotingTeamName);
            }

            var newTeam = new Team(promotingTeamName);
            var existingManager = false;

            foreach (var existing in this.championship.Models)
            {
                if (existing.TeamManager?.Name == managerName)
                {
                    existingManager = true;
                    break;
                }
            }

            if (!existingManager && managerTypes.Contains(managerTypeName))
            {
                var manager = this.CreateManager(managerName, managerTypeName);

                newTeam.SignWith(manager);
            }

            foreach (var existingTeam in this.championship.Models)
            {
                existingTeam.ResetPoints();
            }

            this.championship.Remove(droppingTeamName);
            this.championship.Add(newTeam);

            return string.Format(OutputMessages.TeamHasBeenPromoted, newTeam.Name); // moje bi imash greshka tuk iskam da vidq
        }

        public string SignManager(string teamName, string managerTypeName, string managerName)
        {
            if (!this.championship.Exists(teamName))
            {
                return string.Format(OutputMessages.TeamDoesNotTakePart, teamName);
            }

            if (!managerTypes.Contains(managerTypeName))
            {
                return string.Format(OutputMessages.ManagerTypeNotPresented, managerTypeName);
            }

            var team = this.championship.Get(teamName);

            if (team.TeamManager != null)
            {
                return string.Format(OutputMessages.TeamSignedWithAnotherManager, teamName, team.TeamManager.Name);

            }

            foreach (var existingTeam in this.championship.Models)
            {

                if (existingTeam.TeamManager?.Name == managerName)
                {
                    return string.Format(OutputMessages.ManagerAssignedToAnotherTeam, managerName);
                }
            }

            var manager = this.CreateManager(managerName, managerTypeName);
            team.SignWith(manager);

            return string.Format(OutputMessages.TeamSuccessfullySignedWithManager,managerName, teamName);

        }

        private Manager CreateManager(string managerName, string managerTypeName)
        {
            if (managerTypeName == typeof(AmateurManager).Name)
            {
                return new AmateurManager(managerName);
            }
            else if (managerTypeName == typeof(SeniorManager).Name)
            {
                return new SeniorManager(managerName);
            }
            else if (managerTypeName == typeof(ProfessionalManager).Name)
            {
                return new ProfessionalManager(managerName);
            }

            return null;
        }



    }
}
