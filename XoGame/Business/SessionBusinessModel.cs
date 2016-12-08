using System.Web;
using XoGame.BusinessInterfaces;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Business
{
    public class SessionBusinessModel : ICurrentUserBusinessModel
    {
        public Registered GetUserSession()
        {
            if (HttpContext.Current.Session == null) return null;
            return HttpContext.Current.Session["currentUser"] as Registered;
        }

        public void SetUserSession(Player player)
        {
            if (HttpContext.Current.Session == null) return;
            HttpContext.Current.Session.Add("currentUser", player);
        }

    }
}