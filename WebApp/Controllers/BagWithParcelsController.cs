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
using Base.Domain;
using Asp.Versioning;

namespace WebApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BagWithParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public BagWithParcelsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all bagWithParcels entities.
        /// </summary>
        /// <returns>List of all parcels</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithParcels>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<BagWithParcels>> GetBagWithParcels()
        {
            return await _bll.BagWithParcels.GetBagWithParcels();
        }

        /// <summary>
        /// Get all bagWithParcels entities that are linked with given shipment entity.
        /// </summary>
        /// <returns>List of all Letters</returns>
        [Route("/{Id}/byShipments")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithParcels>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BagWithParcels>>> GetBagWithParcelsByShipment(Guid shipmentId)
        {
            return (await _bll.BagWithParcels.GetBagWithParcelsByShipmentId(shipmentId)).ToList();
        }

        // PUT: api/Shipments/5/PutBags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update bagWithParcels with parcels. Find entity via parameter id and update it with 
        /// </summary>
        /// <param name="id">Supply bagWithParcels entity id you want to change.</param>
        /// <param name="parcels">Supply list of parcel entities you want to add to bagWithParcels entity.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("~/{id}/PutParcels")]
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutParcelsBag(Guid id, List<Parcel> parcels)
        {
            try
            {
                _bll.BagWithParcels.PutParcelsToBag(id, parcels);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/BagWithParcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new bagWithParcels entity. Add it to database.
        /// </summary>
        /// <param name="bagWithParcels">Supply bagWithParces entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BagWithParcels>> PostBagWithParcels(BagWithParcels bagWithParcels)
        {
            var newBagWithParcels = new BagWithParcels();
            try
            {
                newBagWithParcels = _bll.BagWithParcels.PostBagWithParcels(bagWithParcels);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithParcels",
                new
                {
                    id = newBagWithParcels.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                newBagWithParcels);
        }

        // DELETE: api/BagWithParcels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBagWithParcels(Guid id)
        {
            try
            {
                await _bll.BagWithParcels.RemoveBagWithParcelsFromDb(id);

            }catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
