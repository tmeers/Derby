using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Derby.Models;
using Derby.ViewModels;

namespace Derby.Infrastructure
{
    public class LeaderboardHelper
    {
        public static void GenerateLeaderboard(CompetitionViewModel competition)
        {
            competition.Leaderboard = new List<LeaderViewModel>();

            foreach (var racer in competition.Racers)
            {
                var _leader = new LeaderViewModel();

                var _racer = racer;
                List<Heat> _racerHeats = competition.Races.Where(x => x.DenId == _racer.Den.Id).SelectMany(h => h.Heats).ToList();
                List<Contestant> _contestants = _racerHeats.SelectMany(x => x.Contestants.Where(y => y.RacerId == _racer.Id)).ToList();
                int _points = 0;
                foreach (var item in _contestants)
                {
                    if (item.Place != 0)
                    {
                        _points = _points + PointsCalculator.Calculate(competition.LaneCount, item.Place);
                    }
                }

                _leader.Id = _racer.Id;
                _leader.Name = _racer.Scout.Name;
                _leader.DenId = _racer.Den.Id;
                _leader.CarNumber = _racer.CarNumber;
                _leader.Points = _points;
                _leader.DenLogo = _racer.Den.LogoPath;

                competition.Leaderboard.Add(_leader);
            }
        }
    }
}