using CouponAuthAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using Mysqlx;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace CouponAuthAPI.Repository
{
    public class UserRepository
    {
        public readonly string _connectionStrings;
        public UserRepository(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetConnectionString("Dbcs");
        }
        public void UserRegister(ApplicationUser user)
        {
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_registerCouponUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_UserName", user.UserName);
                cmd.Parameters.AddWithValue("p_UserEmailId", user.UserEmail);
                cmd.Parameters.AddWithValue("p_MobileNumber", user.MobileNumber);
                string password = user.UserPassword;
                var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
                cmd.Parameters.AddWithValue("p_UserPassWord", hashedPassword);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public IEnumerable<ApplicationUser> LogInCoupon(string email,string password)
        {
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_logInCoupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_UserEmailId", email);
                var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
                cmd.Parameters.AddWithValue("p_UserPassWord", hashedPassword);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.UserName = reader["UserName"].ToString() ?? "";
                    applicationUser.UserEmail = reader["UserEmailId"].ToString() ?? "";
                    applicationUser.MobileNumber = reader["MobileNumber"].ToString() ?? "";
                    applicationUsers.Add(applicationUser);
                }
            }
            return(applicationUsers);
        }
        public void RollAssign(RoleAssigning roleAssigning)
        {
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_RoleassignforCoupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_UserName", roleAssigning.UserName);
                cmd.Parameters.AddWithValue("p_UserEmailId", roleAssigning.UserEmailId);
                var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(roleAssigning.UserPassWord)));
                cmd.Parameters.AddWithValue("p_UserPassWord", hashedPassword);
                cmd.Parameters.AddWithValue("p_RoleName", roleAssigning.RoleName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
