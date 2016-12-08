using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XoGame.Business;

namespace XoGame.Controllers.APIs
{
    public class MatchController : ApiController
    {
        private readonly MatchBusinessModel _matchBusinessModel;

        public MatchController()
        {
            _matchBusinessModel = new MatchBusinessModel();
        }

        [Route("api/Match/GetTopTen")]
        [HttpGet]
        public IHttpActionResult GetTopTen()
        {
            return Ok(_matchBusinessModel.GetTopTen());
        }
    }
}
