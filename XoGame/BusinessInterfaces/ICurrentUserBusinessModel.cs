using XoGame.Models;

namespace XoGame.BusinessInterfaces
{
    public interface ICurrentUserBusinessModel
    {
        Registered GetUserSession();
        void SetUserSession(Player player);
    }
}