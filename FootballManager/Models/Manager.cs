using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Models.Contracts;
using FootballManager.Utilities.Messages;

namespace FootballManager.Models
{
    public abstract class Manager : IManager
    {
        private string name;

        protected Manager(string name, double ranking)
        {
            Name = name;
            Ranking = ranking;
        }

        public string Name
        {
            get { return this.name; }
           private set

            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(ExceptionMessages.ManagerNameNull);
                }

                this.name = value;
            }
        }


        public double Ranking { get; protected set; }

        public abstract void RankingUpdate(double updateValue);

        public override string ToString()
        {
            return $"{this.Name} - {this.GetType().Name} (Ranking: {this.Ranking:F2})";
        }

        protected void ValidateRanking(double updatedValue)
        {
            this.Ranking += updatedValue;

            if (this.Ranking < 0)
            {
                this.Ranking = 0;
            }

            if (this.Ranking > 100)
            {
                this.Ranking = 100;
            }
        }
    }
}
