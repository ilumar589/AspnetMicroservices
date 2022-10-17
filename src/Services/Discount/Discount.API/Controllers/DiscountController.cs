using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Cupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Cupon>> GetDiscount(string productName)
        {
            var discount = await _discountRepository.GetDiscount(productName);

            return Ok(discount);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cupon), (int) HttpStatusCode.Created)]
        public async Task<ActionResult<Cupon>> CreateDiscount([FromBody] Cupon cupon)
        {
            await _discountRepository.CreateDiscount(cupon);
            return CreatedAtRoute("GetDiscount", new { cupon.ProductName }, cupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Cupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Cupon>> UpdateDiscount([FromBody] Cupon cupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(cupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(Cupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await _discountRepository.DeleteDiscount(productName));
        }

    }
}
