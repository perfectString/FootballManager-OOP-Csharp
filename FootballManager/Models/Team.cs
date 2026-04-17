using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Models.Contracts;
using FootballManager.Utilities.Messages;

namespace FootballManager.Models
{
    public class Team : ITeam
    {
        private string name;

        public Team(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return this.name; }

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.TeamNameNull);
                }
                this.name = value;
            }
        }

        public int ChampionshipPoints {  get; private set; }

        public IManager TeamManager { get; private set; }

        public int PresentCondition
        {
            get
            {
                if (this.TeamManager == null)
                {
                    return 0;
                }
                double result = 0;

                if (this.ChampionshipPoints == 0)
                {
                    result = TeamManager.Ranking;
                }
                else
                {
                    result = this.ChampionshipPoints * this.TeamManager.Ranking;
                }

                return (int)Math.Floor(result);
            }
        }

        public void GainPoints(int points)
        {
            this.ChampionshipPoints += points;
        }

        public void ResetPoints()
        {
            this.ChampionshipPoints = 0;
        }

        public void SignWith(IManager manager)
        {
            this.TeamManager = manager;
        }

        public override string ToString()
        {
            return $"Team: {this.Name} Points: {this.ChampionshipPoints}";
        }
    }
}
