using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elegium.Data;
using Elegium.Models;
using Microsoft.AspNetCore.Authorization;
using Elegium.Dtos;

namespace Elegium.Controllers.api
{
    [Route("api/[controller]")]
    [Authorize(Roles = RoleName.Admin)]
    [ApiController]
    public class PaymentGatewaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentGatewaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentGateways
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentGatewayDto>>> GetPaymentGateway()
        {
            var list = await _context.PaymentGateway.ToListAsync();
            var dtoList = list.Select(a => new PaymentGatewayDto()
            {
                Action = "U",
                Id = a.Id,
                ApiKey = a.ApiKey,
                ApiSecret = a.ApiSecret,
                Default = a.Default,
                Provider = a.Provider
            }).ToList();

            return dtoList;
        }

        // GET: api/PaymentGateways/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentGateway>> GetPaymentGateway(Guid id)
        {
            var paymentGateway = await _context.PaymentGateway.FindAsync(id);

            if (paymentGateway == null)
            {
                return NotFound();
            }

            return paymentGateway;
        }

        // PUT: api/PaymentGateways/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentGateway(Guid id, PaymentGateway paymentGateway)
        {
            if (id != paymentGateway.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentGateway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentGatewayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentGateways
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostPaymentGateway(List<PaymentGatewayDto> paymentGateway)
        {
            try
            {
                var newAddedlist = paymentGateway.Where(a => a.Action == "I").Select(A => new PaymentGateway()
                {
                    ApiKey = A.ApiKey,
                    ApiSecret = A.ApiSecret,
                    Default = A.Default,
                    Provider = A.Provider,
                });

                await _context.PaymentGateway.AddRangeAsync(newAddedlist);

                var deletedList = paymentGateway.Where(a => a.Action == "D").Select(A => new PaymentGateway()
                {
                    ApiKey = A.ApiKey,
                    ApiSecret = A.ApiSecret,
                    Default = A.Default,
                    Provider = A.Provider,
                    Id = A.Id.Value
                });

                _context.PaymentGateway.RemoveRange(deletedList);

                var updatedList = paymentGateway.Where(a => !(a.Action == "D" || a.Action == "I")).Select(A => new PaymentGateway()
                {
                    ApiKey = A.ApiKey,
                    ApiSecret = A.ApiSecret,
                    Default = A.Default,
                    Provider = A.Provider,
                    Id = A.Id.Value
                });

                foreach (var P in updatedList)
                {
                    _context.Entry(P).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                var list = await _context.PaymentGateway.ToListAsync();
                var dtoList = list.Select(a => new PaymentGatewayDto()
                {
                    Action = "U",
                    Id = a.Id,
                    ApiKey = a.ApiKey,
                    ApiSecret = a.ApiSecret,
                    Default = a.Default,
                    Provider = a.Provider
                }).ToList();

                return Ok(new { success = true, Msg = "Payment gateway saved successfully!", List = dtoList });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, Msg = ex.Message + (ex.InnerException == null ? string.Empty : " - " + ex.InnerException.Message) });
            }
        }

        // DELETE: api/PaymentGateways/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentGateway>> DeletePaymentGateway(Guid id)
        {
            var paymentGateway = await _context.PaymentGateway.FindAsync(id);
            if (paymentGateway == null)
            {
                return NotFound();
            }

            _context.PaymentGateway.Remove(paymentGateway);
            await _context.SaveChangesAsync();

            return paymentGateway;
        }

        private bool PaymentGatewayExists(Guid id)
        {
            return _context.PaymentGateway.Any(e => e.Id == id);
        }
    }
}
