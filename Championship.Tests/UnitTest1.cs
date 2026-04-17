using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Championship.Tests
{
    public class Tests
    {
        private List<Team> teams;
        [SetUp]
        public void Setup()
        {
            this.teams = new List<Team>();
            for (int i = 0; i < 10; i++)
            {
                teams.Add(new Team($"Team#{i}"));
            }
        }

        [Test]
        public void TestAddTeamThrowsExceptionWhenMaxCapacity()
        {


            var newTeam = new Team("New");

            var league = new League();

            foreach (var team in teams)
            {
                league.AddTeam(team);
            }

            Assert.Throws<InvalidOperationException>(() =>
            {

                league.AddTeam(newTeam);

            }, "League is full."); //messegite ne se printqt taka

        }

        [Test]
        public void AddTeamThrowsExceptionIfTNameExists()
        {
            var league = new League();

            foreach (var teams in this.teams)
            {
                league.AddTeam(teams);
            }

            var team = new Team("Team#0");

            Assert.Throws<InvalidOperationException>(() =>
            {

                league.AddTeam(team);

            }, "Team already exists.");
        }

        [Test]
        public void AddDoesntWork()
        {
            var league = new League();
            var awayTeam = new Team("AwayTeam");
            league.AddTeam(awayTeam);
            var fakeAwayTeam = new Team("AwayTeam");

            Assert.Throws<InvalidOperationException>(() =>
            {

                league.AddTeam(fakeAwayTeam);

            }, "Team already exists.");
        }


        [Test]
        public void AddTeamWorksCorrectly()
        {
            var teamName = "NewTeam";
            var team = new Team(teamName);
            var league = new League();

            league.AddTeam(team);

            var expected = league.GetTeamInfo(teamName);
            Assert.AreEqual(expected, team.ToString());


        }

        [Test]
        public void GetTeamInfoCantWorkWithNull()
        {
            var league = new League();

            Assert.Throws<InvalidOperationException>(() => league.GetTeamInfo("M"), "Team does not exist.");
        }

        [Test]
        public void RemoveShouldReturnFalse()
        {
            var teamName = "NewTeam";
            var team = new Team(teamName);
            var league = new League();
            league.AddTeam(team);
            var exists = league.RemoveTeam("Team");

            Assert.That(exists, Is.False);
            Assert.That(league.Teams.Count, Is.EqualTo(1));
        }
        [Test]
        public void RemoveShouldReturnTrue()
        {
            var teamName = "NewTeam";
            var team = new Team(teamName);
            var league = new League();
            league.AddTeam(team);
            var exists = league.RemoveTeam("NewTeam");

            Assert.That(exists, Is.True);
            Assert.That(league.Teams.Count, Is.EqualTo(0));
        }
        [Test]
        public void PlayMatchThrowsExceptionIfTeamDoesntExist()
        {

            var homeTeam = new Team("HomeTeam");
            var awayTeam = new Team("AwayTeam");
            var league = new League();
            league.AddTeam(homeTeam);
            league.AddTeam(awayTeam);

            Assert.Throws<InvalidOperationException>(() =>
            {
                league.PlayMatch("HomeTeam", "Away", 3, 2);
            }, "One or both teams do not exist.");

            Assert.Throws<InvalidOperationException>(() =>
            {
                league.PlayMatch("HTeam", "AwayTeam", 3, 2);
            }, "One or both teams do not exist.");
            Assert.Throws<InvalidOperationException>(() =>
            {
                league.PlayMatch("CatTeam", "AwayDogs", 3, 2);
            }, "One or both teams do not exist.");
        }

        [Test]
        public void DrawWorksCorrectly()
        {

            var homeTeam = new Team("HomeTeam");
            var awayTeam = new Team("AwayTeam");
            var league = new League();
            league.AddTeam(homeTeam);
            league.AddTeam(awayTeam);
            


            league.PlayMatch("HomeTeam", "AwayTeam", 3, 3);

            Assert.That(homeTeam.Draws, Is.EqualTo(1));
            Assert.That(homeTeam.Points, Is.EqualTo(1));
            Assert.That(awayTeam.Draws, Is.EqualTo(1));
            Assert.That(awayTeam.Points, Is.EqualTo(1));
            Assert.That(league.Capacity, Is.EqualTo(10));

        }

        [Test]
        public void HomeTeamWinsGivePointsCorrectly()
        {

            var homeTeam = new Team("HomeTeam");
            var awayTeam = new Team("AwayTeam");
            var league = new League();
            league.AddTeam(homeTeam);
            league.AddTeam(awayTeam);


            league.PlayMatch("HomeTeam", "AwayTeam", 1, 0);
            Assert.That(homeTeam.Points, Is.EqualTo(3));
            Assert.That(homeTeam.Wins, Is.EqualTo(1));
            Assert.That(awayTeam.Loses, Is.EqualTo(1));
        }
        [Test]
        public void AwayTeamWinsGivePointsCorrectly()
        {

            var homeTeam = new Team("HomeTeam");
            var awayTeam = new Team("AwayTeam");
            var league = new League();
            league.AddTeam(homeTeam);
            league.AddTeam(awayTeam);


            league.PlayMatch("HomeTeam", "AwayTeam", 0, 1);
            Assert.That(homeTeam.Points, Is.EqualTo(0));
            Assert.That(awayTeam.Points, Is.EqualTo(3));
            Assert.That(homeTeam.Wins, Is.EqualTo(0));
            Assert.That(awayTeam.Wins, Is.EqualTo(1));
            Assert.That(awayTeam.Loses, Is.EqualTo(0));
        }
    }
}