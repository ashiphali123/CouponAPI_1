
using CouponMVC.Models;
using CouponMVC.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CouponMVC.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto? response = await _couponService.GetAllCouponAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        
        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _couponService.CreateCouponAsync(couponDto);

                if (response != null && response.IsSuccess) // agar aapke ResponseDto me success flag hai
                {
                    TempData["success"] = "Coupon Created successfully!!";
                    return RedirectToAction(nameof(CouponIndex)); // ✅ Redirect to Index page
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Something went wrong";
                }
            }

            return View(couponDto); // agar validation fail ho ya create fail ho to wahi form dobara dikhao
        }

        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if(response != null && response.IsSuccess)
            {
                /*List<CouponDto>? modelList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
                CouponDto? model = modelList?.FirstOrDefault(); // get single coupon
                return View(model);*/
                var coupons = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
                CouponDto model = coupons.FirstOrDefault();
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCouponConfirmed(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted successfully!!";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);
        }
    }
}
