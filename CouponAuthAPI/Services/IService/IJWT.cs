using CouponAuthAPI.Model;

namespace CouponAuthAPI.Services.IService
{
    public interface IJWT
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
