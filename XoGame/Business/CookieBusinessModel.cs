using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using XoGame.BusinessInterfaces;
using XoGame.Models;

namespace XoGame.Business
{
    public class CookieBusinessModel : ICurrentUserBusinessModel
    {
        public Registered GetUserSession()
        {
            try
            {
                return
                    JsonConvert.DeserializeObject<Registered>(
                        HttpContext.Current.Request.Cookies.Get("currentUser")?.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetUserSession(Player player)
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("currentUser", JsonConvert.SerializeObject(player))
            {
                Expires = DateTime.Now.AddMinutes(20)
            });
        }
    }
}