using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using MiddleMan;
using Newtonsoft.Json;
using XoGame.Events;
using XoGame.Models;

namespace XoGame.Hubs
{
    public class TableHub : Hub
    {
        private static List<Connection> Connections;
        public TableHub()
        {
            if(Connections == null) Connections = new List<Connection>();
        }

        public void TableCreatedEvent(string jsonTable)
        {
            var table = JsonConvert.DeserializeObject<Table>(jsonTable);
            var connection = Connections.FirstOrDefault(
                x =>
                    x.ConnectionId == Context.ConnectionId);
            if (connection == null) return;
            connection.GroupName = table.Name;

            Groups.Add(connection.ConnectionId, table.Name);
            Clients.Others.tableAdded(table);
        }

        public void Connect(string playerId, string playerName)
        {
            var playerGuid = new Guid(playerId);
            var connection =
                Connections.FirstOrDefault(x => x.PlayerId == playerGuid || x.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                if (connection.PlayerId == playerGuid && connection.ConnectionId == Context.ConnectionId)
                    return;
                Connections.Remove(connection);
            }
            Connections.Add(new Connection {ConnectionId = Context.ConnectionId, PlayerId = playerGuid, PlayerName = playerName});
        }

        public void JoinTable(string tableName, string playerId,string playerName)
        {
            if (tableName == null || playerId == null) return;
            
            var connection = Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connection == null)
            {
                connection = new Connection
                {
                    ConnectionId = Context.ConnectionId,
                    PlayerId = new Guid(playerId),
                    PlayerName = playerName,
                    GroupName = tableName
                };
                Connections.Add(connection);
            }

            //remove player from previous table
            if (!string.IsNullOrEmpty(connection.GroupName))
            {
                EventAggregator.Resolve<PlayerLeftTable>().Publish(new Table
                {
                    Name = connection.GroupName,
                    Players = new List<Registered>
                {
                    new Registered {Id = new Guid(playerId)}
                }
                });
                Groups.Remove(connection.ConnectionId, connection.GroupName);
            }

            //add player to the new table
            connection.GroupName = tableName;
            Groups.Add(connection.ConnectionId, tableName);
            Clients.OthersInGroup(tableName).playerJoined(playerName);
            EventAggregator.Resolve<PlayerJoinedTable>().Publish(new Table
            {
                Name = tableName,
                Players = new List<Registered>
                {
                    new Registered {Id = new Guid(playerId)}
                }
            });
            Clients.All.UpdateTables();
        }
        
        public void Play(string location, string tableName)
        {
            var x = Context.ConnectionId;
            //var currentPlayer = Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            //var otherPlayer = Connections.FirstOrDefault(x => x.GroupName == currentPlayer?.GroupName
            //                                                  && x.ConnectionId != currentPlayer?.ConnectionId);
            //if (otherPlayer == null) return;
            //Clients.Client(otherPlayer.ConnectionId).played(location);
            Clients.OthersInGroup(tableName).played(location);
        }

        public void NewMatch()
        {
            var player = Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            Clients.OthersInGroup(player?.GroupName)
                .MatchRestarted();
        }

        public void Win()
        {
            var player = Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (player == null) return;

            EventAggregator.Resolve<TopTenChanged>().Subscribe(topTen =>
            {
                Clients.All.TopTenChanged(topTen.OrderBy(x=>x.Score).ToList());
            });

            var match = new Match
            {
                Scores = Connections.Where(x=>x.GroupName == player.GroupName).Select(x=> new PlayerScore
                {
                    Id = Guid.NewGuid(),
                    PlayerId = x.PlayerId,
                    Score = x.PlayerId == player.PlayerId ? 1 : 0
                }).ToList(),
                Table = new Table { Name = player.GroupName},
                Status = MatchStatus.Ended
            };
            EventAggregator.Resolve<MatchEnded>().Publish(match);
            
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var player = Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (player != null)
            {
                EventAggregator.Resolve<PlayerLeftTable>().Publish(new Table
                {
                    Name = player.GroupName,
                    Players = new List<Registered>
                    {
                        new Registered {Id = player.PlayerId}
                    }
                });

                Clients.All.PlayerLeft(player.PlayerName);
                Connections.Remove(player);
            }

            return base.OnDisconnected(stopCalled);
        }
    }

    internal class Connection
    {
        internal string ConnectionId { get; set; }
        internal Guid PlayerId { get; set; }
        internal string PlayerName { get; set; }
        internal string GroupName { get; set; }
    }
    
}