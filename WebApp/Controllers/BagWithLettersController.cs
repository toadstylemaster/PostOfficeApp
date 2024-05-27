using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Authorization;
using App.Public.DTO.v1;
using Asp.Versioning;

namespace WebApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BagWithLettersController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public BagWithLettersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all bagWithLetters entities.
        /// </summary>
        /// <returns>List of all Letters</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithLetters>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<BagWithLetters>> GetBagWithLetters()
        {
            return await _bll.BagWithLetters.GetBagWithLetters();
        }

        /// <summary>
        /// Get all bagWithLetters entities that are linked to given shipment entity.
        /// </summary>
        /// <returns>List of all Letters</returns>
        [Route("{shipmentId}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithLetters>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BagWithLetters>>> GetBagWithLettersByShipment(Guid shipmentId)
        {
            return (await _bll.BagWithLetters.GetBagWithLettersByShipmentId(shipmentId)).ToList();
        }

        // POST: api/BagWithLetters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new bagWithLetters entity. Add it to database.
        /// </summary>
        /// <param name="bagWithLetters">Supply bagWithParces entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BagWithLetters>> PostBagWithLetters(BagWithLetters bagWithLetters)
        {
            var newBagWithLetters = new BagWithLetters();
            try
            {
                newBagWithLetters = _bll.BagWithLetters.PostBagWithLetters(bagWithLetters);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithLetters",
                new
                {
                    id = newBagWithLetters.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                newBagWithLetters);
        }

        // DELETE: api/BagWithLetters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBagWithLetters(Guid id)
        {
            try
            {
                await _bll.BagWithLetters.RemoveBagWithLettersFromDB(id);

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
