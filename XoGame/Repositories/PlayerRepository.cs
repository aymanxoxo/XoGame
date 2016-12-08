using System;
using System.Linq;
using XoGame.Models;

namespace XoGame.Repositories
{
    public class PlayerRepository
    {
        public void Add(Registered player)
        {
            player.Name = player.Name.ToLower().Trim();
            using (var context = new XoContext())
            {
                if (
                    context.Players.Any(
                        p => p.Name == player.Name))
                    throw new InvalidOperationException("Another user exists with the same name.");
                context.Players.Add(player);
                context.SaveChanges();
            }
        }

        public Player UserExists(string name, string password)
        {

            using (var context = new XoContext())
            {
                return context.Players.OfType<Registered>().FirstOrDefault(x => x.Name == name && x.Password == password);
            }
        }

        public bool UserExists(string name)
        {

            using (var context = new XoContext())
            {
                if (context.Players.OfType<Registered>().Any(x => x.Name == name))
                    return true;
                return false;
            }
        }

        public bool UserExists(Guid? playerId)
        {
            if (playerId == null || playerId == Guid.Empty) return false;
            using (var context = new XoContext())
            {
                if (context.Players.OfType<Registered>().Any(x => x.Id == playerId))
                    return true;
                return false;
            }
        }

        public Registered FindUserById(Guid? playerId)
        {
            using (var context = new XoContext())
            {
                return context.Players.OfType<Registered>().FirstOrDefault(p => p.Id == playerId);
            }
        }

        public Registered FindUserByName(string name)
        {
            using (var context = new XoContext())
            {
                return context.Players.OfType<Registered>().FirstOrDefault(p => p.Name == name);
            }
        }
    }
}