using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiddleMan;
using XoGame.Events;
using XoGame.Hubs;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Business
{
    public class TableBusinessModel
    {
        private readonly TableRepository _tableRepository;
        private readonly PlayerBusinessModel _playerBusinessModel;

        public TableBusinessModel()
        {
            _playerBusinessModel = new PlayerBusinessModel();
            _tableRepository = new TableRepository(new SessionBusinessModel());

            EventAggregator.Resolve<PlayerJoinedTable>().Subscribe(PlayerJoinedTableEvent);
            EventAggregator.Resolve<PlayerLeftTable>().Subscribe(PlayerLeftTableEvent);
        }

        private void PlayerLeftTableEvent(Table obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.Name) || obj.Players?.FirstOrDefault() == null) return;

            var player = _playerBusinessModel.FindPlayerById(obj.Players.FirstOrDefault().Id);
            var table = _tableRepository.FindTableByName(obj.Name);
            if (table == null || player == null) return;

            _tableRepository.RemovePlayerToTable(table, player);
        }

        private void PlayerJoinedTableEvent(Table obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.Name) || obj.Players?.FirstOrDefault() == null) return;

            var player = _playerBusinessModel.FindPlayerById(obj.Players.FirstOrDefault().Id);
            var table = _tableRepository.FindTableByName(obj.Name);
            if (table == null || player == null) return;
            
            _tableRepository.AddPlayerToTable(table, player);
        }

        public List<Table> GetCurrentTables()
        {
            return _tableRepository.GetCurrentTables();
        }

        public Table AddTable(Table table)
        {
            if (_tableRepository.TableExists(table.Name))
                throw new InvalidOperationException("a table with the same name exists.");
            var newTable = _tableRepository.AddTable(table);
            return newTable;
        }

        public void JoinTable(Table table)
        {
            if (table == null || table.Id == Guid.Empty || table.Players == null || table.Players.Count == 0)
                throw new ArgumentNullException(nameof(table));

            var originalTable = _tableRepository.FindTableById(table.Id);
            if (originalTable == null) throw new InvalidOperationException("couldn't find table");
            var player = _playerBusinessModel.FindPlayerById(table.Players?.FirstOrDefault()?.Id);
            if (player == null) throw new InvalidOperationException("couldn't find player");

            originalTable.Players.Add(player);

            _tableRepository.SaveTable(originalTable);
        }

        public Table FindTableByName(string name)
        {
            return _tableRepository.FindTableByName(name);
        }
    }
}