using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Business
{
    public class PlayerBusinessModel
    {
        private readonly PlayerRepository _playerRepository;

        public PlayerBusinessModel()
        {
            _playerRepository = new PlayerRepository();
        }

        public Registered FindPlayerById(Guid? playerId)
        {
            return _playerRepository.FindUserById(playerId);
        }

        public Registered FindPlayerByName(string name)
        {
            return _playerRepository.FindUserByName(name);
        }

        public Player UserExists(string name, string password)
        {
            var result = _playerRepository.UserExists(name, password);
            var player = new Registered {Name = name, Password = password};
            if (result == null)
            {
                _playerRepository.Add(player);
                return player;
            }
            else
            {
                return result;
            }
        }

        public bool UserExists(string name)
        {
            return _playerRepository.UserExists(name);
        }
    }
}