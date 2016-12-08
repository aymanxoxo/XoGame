using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XoGame.Business;
using XoGame.BusinessInterfaces;
using XoGame.Models;

namespace XoGame.Repositories
{
    public class TableRepository
    {
        private const int MaxPlayers = 2;

        private readonly ICurrentUserBusinessModel _currentUserUserRepository;

        public TableRepository(ICurrentUserBusinessModel currentUserUserRepository)
        {
            _currentUserUserRepository = currentUserUserRepository;
        }

        public List<Table> GetCurrentTables()
        {
            using (var context = new XoContext())
            {
                var tables = context.Table.Include(x=>x.Players).Where(x => !x.IsDeleted && x.Players.Count < MaxPlayers).ToList();
                return tables;
            }
        }

        public Table AddTable(Table table)
        {
            if (string.IsNullOrEmpty(table?.Name)) throw new ArgumentNullException(nameof(table));
            using (var context = new XoContext())
            {
                if (context.Table.Any(t => t.Name == table.Name))
                    return context.Table.FirstOrDefault(x => x.Name == table.Name);
                var owner = _currentUserUserRepository.GetUserSession();
                table = new Table
                {
                    Id = Guid.NewGuid(),
                    Owner = owner,
                    OwnerId = _currentUserUserRepository.GetUserSession()?.Id ?? Guid.Empty,
                    Players = new List<Registered> { _currentUserUserRepository.GetUserSession()},
                    Name = table.Name
                };
                context.Entry(table.Owner).State = EntityState.Unchanged;
                context.Table.Add(table);
                context.SaveChanges();
            }
            return table;
        }
        
        public   Table FindTableById(Guid tableId)
        {
            using (var context = new XoContext())
            {
                return context.Table.FirstOrDefault(t => t.Id == tableId);
            }
        }

        public void SaveTable(Table table)
        {
            using (var context = new XoContext())
            {
                context.Entry(table).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public bool TableExists(string name)
        {
            using (var context = new XoContext())
            {
                return context.Table.Any(x => x.Name == name);
            }
        }

        public Table FindTableByName(string name)
        {
            using (var context = new XoContext())
            {
                return context.Table.Include(x=>x.Players).FirstOrDefault(x => x.Name == name);
            }
        }

        public void AddPlayerToTable(Table table, Registered player)
        {
            using (var context = new XoContext())
            {
                context.Entry(table).State = EntityState.Modified;
                var p = context.Players.OfType<Registered>().FirstOrDefault(x => x.Id == player.Id);
                table.Players.Add(p);
                context.SaveChanges();
            }
        }

        public void RemovePlayerToTable(Table table, Registered player)
        {
            using (var context = new XoContext())
            {
                context.Entry(table).State = EntityState.Modified;
                var p = context.Players.OfType<Registered>().FirstOrDefault(x => x.Id == player.Id);
                table.Players.Remove(p);
                context.SaveChanges();
            }
        }
    }
}