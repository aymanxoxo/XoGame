using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XoGame.Business;
using XoGame.BusinessInterfaces;
using XoGame.Repositories;

namespace XoGame.Controllers.Views
{
    public class HomeController : Controller
    {

        private readonly ICurrentUserBusinessModel _userSessionUserModel;

        public HomeController()
        {
            _userSessionUserModel = new SessionBusinessModel();
        }



        // GET: Home
        public ActionResult Index()
        {
            var player = _userSessionUserModel.GetUserSession();
            ViewBag.authenticated = player == null ? "0" : "1";
            if (player == null)
                ViewBag.currentPlayer = new {};
            else
                ViewBag.currentPlayer = player;
            return View();
        }
    }
}