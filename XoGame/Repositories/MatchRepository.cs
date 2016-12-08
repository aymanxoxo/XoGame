using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XoGame.Models;

namespace XoGame.Repositories
{
    public class MatchRepository
    {
        public void AddMatch(Match match)
        {
            using (var context = new XoContext())
            {
                if (match.Id == Guid.Empty) match.Id = Guid.NewGuid();
                if (context.Match.Any(x => x.Id == match.Id)) return;
                match.Table = context.Table.FirstOrDefault(x => x.Id == match.TableId);
                context.Match.Add(match);
                context.SaveChanges();
            }
        }

        public List<PlayerScore> GetTopTen()
        {
            using (var context = new XoContext())
            {
                return
                    (from x in context.PlayerScores
                        group x by new {x.PlayerId}
                        into gx
                        select new
                        {
                            gx.Key.PlayerId,
                            Score = gx.Sum(s => s.Score)
                        }).OrderBy(x=>x.Score).Take(10).ToList().Select(x=>new PlayerScore
                        {
                            PlayerId = x.PlayerId,
                            Score = x.Score
                        }).ToList();
            }
        }

        public bool Exists(Guid id)
        {
            using (var context = new XoContext())
            {
                return context.Match.Any(x => x.Id == id);
            }
        }
    }
}