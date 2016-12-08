using System;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Business
{
    public class Authentication
    {
        private readonly PlayerRepository _repository;

        public Authentication()
        {
            _repository = new PlayerRepository();
        }

        public void SignUp(Registered player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (string.IsNullOrEmpty(player.Name))
                throw new InvalidOperationException("Name must be provided.");
            if (string.IsNullOrEmpty(player.Password))
                throw new InvalidOperationException("Password must be provided.");
            _repository.Add(player);
        }

        public bool IsAuthorized(Registered player)
        {
            if (player == null) throw new ArgumentNullException("Name must be provided.");
            if (string.IsNullOrEmpty(player.Name))
                throw new InvalidOperationException("Name must be provided.");
            if (string.IsNullOrEmpty(player.Password))
                throw new InvalidOperationException("Password must be provided.");

            if (_repository.UserExists(player.Name, player.Password) != null)
                return true;

            SignUp(player);
            return true;
        }
    }
}