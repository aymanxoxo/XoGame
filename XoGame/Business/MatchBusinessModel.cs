using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiddleMan;
using XoGame.Events;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Business
{
    public class MatchBusinessModel
    {
        private readonly MatchRepository _matchRepository;
        private readonly TableBusinessModel _tableBusinessModel;
        private readonly PlayerBusinessModel _playerBusinessModel;
        public MatchBusinessModel()
        {
            _matchRepository = new MatchRepository();
            _tableBusinessModel = new TableBusinessModel();
            _playerBusinessModel = new PlayerBusinessModel();

            EventAggregator.Resolve<MatchEnded>().Subscribe(MatchEndedTableEvent);
        }


        private void MatchEndedTableEvent(Match obj)
        {
            obj.Id = Guid.NewGuid();
            var match = new Match
            {
                Id = obj.Id,
                Scores = obj.Scores,
                Table = _tableBusinessModel.FindTableByName(obj.Table.Name)
            };
            match.TableId = match.Table.Id;
            match.Status = obj.Status;
            _matchRepository.AddMatch(match);
            if (obj.Status == MatchStatus.Ended)
            {
                EventAggregator.Resolve<TopTenChanged>().Publish(GetTopTen());
            }
        }

        public List<TopScore> GetTopTen()
        {
            return _matchRepository.GetTopTen().Select(x=> new TopScore
            {
                Player = _playerBusinessModel.FindPlayerById(x.PlayerId),
                Score = x.Score
            }).OrderBy(x=>x.Score).ToList();
        }
    }
}