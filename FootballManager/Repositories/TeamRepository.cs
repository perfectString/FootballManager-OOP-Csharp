using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Models;
using FootballManager.Models.Contracts;
using FootballManager.Repositories.Contracts;

namespace FootballManager.Repositories
{
    public class TeamRepository : IRepository<ITeam>
    {
        private List<ITeam> teams;
        public TeamRepository()
        {
            this.teams = new List<ITeam>();
        }
        public IReadOnlyCollection<ITeam> Models
        {
            get { return this.teams.AsReadOnly(); }
        }

        public int Capacity => 10;

        public void Add(ITeam model)
        {
            if (this.teams.Count == Capacity)
            {
                return;
            }
            this.teams.Add(model);
        }

        public bool Exists(string name)
        {
            return this.teams.Any(t => t.Name == name);
        }

        public ITeam Get(string name)
        {
            return this.teams.FirstOrDefault(t => t.Name == name);
        }

        public bool Remove(string name)
        {
            var team = this.teams.FirstOrDefault(t => t.Name == name);
            if (team == null)
            {
                return false;
            }
            this.teams.Remove(team);
            return true;
        }
    }
}
