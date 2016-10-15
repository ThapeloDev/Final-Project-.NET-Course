using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class UserLogic{
        public Task<Dictionary<string, string>> GetUser(string username)
        {
            return Task.Run(() =>
            {
                var user = Server.GetUser(username);
                return user == null ? null : user.ItemList;
            });
        }
        public Task<bool> IsUsernameExist(string username)
        {
            return Task.Run(() =>
            {
                var user = Server.GetUser(username);
                return user != null;
            });
        }
        public Task<bool> SignNewUserToDb(string username)
        {
            return Task.Run(() =>
            {
                var ans = Server.SignNewUser(username);
                return ans != null;
            });
        }
        public Task SavefavouriteCart(string username, Dictionary<string, double> cart)
        {
            return Task.Run(() => Server.SaveUserFavoriteCart(username, cart));
        }
    }
}
