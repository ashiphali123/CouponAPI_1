//using CouponMVC.Model.Dto;
using CouponMVC.Models;

namespace CouponMVC.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
