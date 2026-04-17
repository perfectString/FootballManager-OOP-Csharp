using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Models
{
    public class SeniorManager : Manager
    {
        private const int InitialRanking = 30;
        public SeniorManager(string name) : base(name, InitialRanking)
        {
        }

        public override void RankingUpdate(double updateValue)
        {
            ValidateRanking(updateValue);
        }
    }
}
