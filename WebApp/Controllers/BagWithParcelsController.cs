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

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<BagWithParcels>>> GetBagWithParcels()
        {
            var res = (await _bll.BagWithParcels.AllAsync())
                .Select(x => new App.Public.DTO.v1.BagWithParcels()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    ListOfParcels = (ICollection<App.Public.DTO.v1.Parcel>?)x.ListOfParcels
                })
                .ToList();
            return res;
        }

        // GET: api/BagWithParcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BagWithParcels>> GetBagWithParcels(Guid id)
        {
            var bagWithParcels = await _bll.BagWithParcels.FindAsync(id);

            if (bagWithParcels == null)
            {
                return NotFound();
            }

            var bagWithParcelsFromDb = new App.Public.DTO.v1.BagWithParcels()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
                ListOfParcels = (ICollection<Parcel>?)bagWithParcels.ListOfParcels
            };

            return bagWithParcelsFromDb;
        }

        // PUT: api/BagWithParcels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update bagWithParcels entity with list of Parcels.
        /// </summary>
        /// <param name="id">Supply parcel entity id you want to change.</param>
        /// <param name="bagWithParcels">Supply bagWithParcels entity with updated values.</param>
        /// <returns>404 if bagWithParcels with given id is not found or 204, if changes were successful</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBagWithParcels(Guid id, BagWithParcels bagWithParcels)
        {
            if (id != bagWithParcels.Id)
            {
                return BadRequest();
            }


            var bagWithParcelsFromDb = await _bll.BagWithParcels.FindAsync(id);
            if (bagWithParcelsFromDb == null)
            {
                return NotFound();
            }

            bagWithParcelsFromDb.Id = bagWithParcels.Id;
            bagWithParcelsFromDb.BagNumber = bagWithParcels.BagNumber;
            bagWithParcelsFromDb.ListOfParcels = (ICollection<App.BLL.DTO.Parcel>?)bagWithParcels.ListOfParcels;

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
            var bagWithParcelsFromDb = new App.BLL.DTO.BagWithParcels()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
                ListOfParcels = (ICollection<App.BLL.DTO.Parcel>?)bagWithParcels.ListOfParcels
            };

            _bll.BagWithParcels.Add(bagWithParcelsFromDb);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithParcels",
                new
                {
                    id = bagWithParcels.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                bagWithParcels);
        }

        // DELETE: api/BagWithParcels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBagWithParcels(Guid id)
        {
            var bagWithParcels = await _bll.BagWithParcels.FindAsync(id);
            if (bagWithParcels == null)
            {
                return NotFound();
            }

            _bll.BagWithParcels.Remove(bagWithParcels);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
