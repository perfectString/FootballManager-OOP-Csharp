using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Models
{
    public class ProfessionalManager : Manager
    {
        private const int InitialRanking = 60;
        public ProfessionalManager(string name) : base(name, InitialRanking)
        {
        }

        public override void RankingUpdate(double updateValue)
        {
            ValidateRanking(updateValue * 1.50);
        }
    }
}
