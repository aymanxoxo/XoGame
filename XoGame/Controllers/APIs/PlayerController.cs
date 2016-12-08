using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XoGame.Business;
using XoGame.BusinessInterfaces;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Controllers.APIs
{
    public class PlayerController : ApiController
    {


        private readonly ICurrentUserBusinessModel _userSessionUserRepository;
        private readonly PlayerBusinessModel _playerBusinessModel;

        public PlayerController()
        {
            _userSessionUserRepository = new SessionBusinessModel();
            _playerBusinessModel = new PlayerBusinessModel();
        }

        [Route("api/Player")]
        [HttpPost]
        public IHttpActionResult Player(Registered player)
        {
            return Ok(player);
        }

        [Route("api/Player/IsAuthorized")]
        [HttpPost]
        public IHttpActionResult IsAuthorized(Registered player)
        {
            if (player == null) return BadRequest();

            // check if there is an active session
            var sessionPlayer = _userSessionUserRepository.GetUserSession();
            if (sessionPlayer != null)
                return Ok(sessionPlayer);

            // if User is authenticated succeccfully data is saved in session and response code 200 is returned
            var currentPlayer = _playerBusinessModel.UserExists(player.Name, player.Password);
            if (currentPlayer != null)
            {
                _userSessionUserRepository.SetUserSession(currentPlayer);
                return Ok(currentPlayer);
            }
            return BadRequest();
        }

        [Route("api/Player/NameAvailable")]
        [HttpPost]
        public IHttpActionResult NameAvailable(Registered player)
        {
            if (_playerBusinessModel.UserExists(player?.Name))
                return Ok(false);
            return Ok(true);
        }
    }
}
