using CouponServiceAPI.Model;
using MySql.Data.MySqlClient;
using System.Data;
using ZstdSharp.Unsafe;

namespace CouponServiceAPI.Repository
{
    public class CouponRepository
    {
        private readonly string _connectionStrings;
        public CouponRepository(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetConnectionString("Dbcs");
        }
        public List<Coupon> GetCoupon()
        {
            List<Coupon> coupons = new List<Coupon>();
            using (MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_getList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Coupon couponlist = new Coupon();
                    couponlist.CouponId = Convert.ToInt32(reader["CouponId"]);
                    couponlist.CouponCode = reader["CouponCode"].ToString() ?? "";
                    couponlist.DiscountAmount = Convert.ToDouble(reader["DiscountAmount"]);
                    couponlist.MinAmount = Convert.ToInt32(reader["MinAmount"]);
                    coupons.Add(couponlist);
                }
            }
            return coupons;
        }
        public IEnumerable<Coupon> GetDetailByCoupon(string code)
        {
            List<Coupon> coupons = new List<Coupon>();
            using (MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_getdetailbycode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_CouponCode", code);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Coupon coupon = new Coupon();
                    coupon.CouponCode = reader["CouponCode"].ToString() ?? "";
                    coupon.DiscountAmount = Convert.ToDouble(reader["DiscountAmount"]);
                    coupon.MinAmount = Convert.ToInt32(reader["MinAmount"]);
                    coupons.Add(coupon);
                }
            }
            return coupons;
        }
        public void CreateCoupon(Coupon coupon)
        {
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_createCoupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_CouponCode", coupon.CouponCode);
                cmd.Parameters.AddWithValue("p_DiscountAmount", coupon.DiscountAmount);
                cmd.Parameters.AddWithValue("p_MinAmount", coupon.MinAmount);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteCoupon(int id)
        {
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                MySqlCommand cmd = new MySqlCommand("sp_deleteCoupon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_CouponId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Coupon> GetCouponById(int id)
        {
            List<Coupon> coupons = new List<Coupon>();
            using (MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                //List<Coupon> coupons = new List<Coupon>();
                MySqlCommand cmd = new MySqlCommand("sp_getById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_CouponId", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Coupon coupon = new Coupon();
                    coupon.CouponId = Convert.ToInt32(reader["CouponId"]);
                    coupon.CouponCode = reader["CouponCode"].ToString() ?? "";
                    coupon.DiscountAmount = Convert.ToDouble(reader["DiscountAmount"]);
                    coupon.MinAmount = Convert.ToInt32(reader["MinAmount"]);
                    coupons.Add(coupon);
                }
            }
            return coupons;
        }
    }
}
