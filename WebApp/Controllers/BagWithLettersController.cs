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
using App.Public.DTO.Mappers;
using AutoMapper;

namespace WebApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BagWithLettersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        public BagWithLettersController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
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
        [Route("{Id}/byShipments")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithLetters>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BagWithLetters>>> GetBagWithLettersByShipment(Guid shipmentId)
        {
            var _bagMapper = new BagWithLettersMapper(_mapper);
            return (await _bll.BagWithLetters.GetBagWithLettersByShipmentId(shipmentId)).Select(x => _bagMapper.Map(x)!).ToList();
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
            var _bagMapper = new BagWithLettersMapper(_mapper);
            var newBagWithLetters = new BagWithLetters();
            try
            {
                newBagWithLetters = _bagMapper.Map(_bll.BagWithLetters.PostBagWithLetters(_bagMapper.Map(bagWithLetters)!));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithLetters",
                new
                {
                    id = newBagWithLetters!.Id,
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
