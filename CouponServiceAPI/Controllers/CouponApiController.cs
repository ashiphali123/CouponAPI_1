using CouponServiceAPI.Model;
using CouponServiceAPI.Model.Dto;
using CouponServiceAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponServiceAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly CouponRepository _couponRepository;
        private ResponseDto _responseDto;
        public CouponApiController(CouponRepository coupon)
        {
            _couponRepository = coupon;
            _responseDto = new ResponseDto();
        }
        [HttpGet]
        public ResponseDto GetCouponList()
        {
            try
            {
                var coupons = _couponRepository.GetCoupon(); // ideally List<Coupon>
                _responseDto.IsSuccess = true;
                _responseDto.Result = coupons;  // store data in Result property
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
        [HttpGet("GetByCode/{code}")]
        public ActionResult<ResponseDto> GetDataCoupon(string code)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                var data = _couponRepository.GetDetailByCoupon(code);

                if (data == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Coupon not found";
                    return NotFound(responseDto);
                }

                responseDto.IsSuccess = true;
                responseDto.Message = "Coupon found";
                responseDto.Result = data;
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message; // capture actual error
                return StatusCode(500, responseDto); // 500 for server errors
            }
        }
        [HttpPost]
        public IActionResult CreateCoupon(Coupon coupon)
        {
            try
            {
                if (coupon.CouponCode == null || coupon.DiscountAmount == 0 || coupon.MinAmount == 0)
                {
                    //TempData["error"] = "Please fill all required fields.";
                    return BadRequest("Fill all data!");
                }
                _couponRepository.CreateCoupon(coupon);
                return Ok(new { message = "Coupon Created Successfully", IsSuccess = true });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            //return coupon;
        }
        [HttpDelete("couponId/{couponId}")]
        public IActionResult DeleteCoupon(int couponId)
        {
            try
            {
                _couponRepository.DeleteCoupon(couponId);
                return Ok(new { message= "Coupon deleted", IsSuccess = true});
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("{couponId}")]
        public IActionResult GetByID(int couponId)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                var coupon = _couponRepository.GetCouponById(couponId);

                if (coupon == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Coupon not found";
                    return NotFound(responseDto);
                }

                responseDto.IsSuccess = true;
                responseDto.Result = coupon;
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return StatusCode(500, responseDto);
            }
        }

    }
}
