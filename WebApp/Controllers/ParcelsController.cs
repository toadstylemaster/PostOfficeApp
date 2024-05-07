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
    public class ParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public ParcelsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: api/parcels
        /// <summary>
        /// Get all parcel entities.
        /// </summary>
        /// <returns>List of all parcels</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Parcel>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parcel>>> GetParcels()
        {
            var res = (await _bll.Parcels.AllAsync())
                .Select(x => new App.Public.DTO.v1.Parcel()
                {
                    Id = x.Id,
                    ParcelNumber = x.ParcelNumber,
                    RecipientName = x.RecipientName,
                    DestinationCountry = x.DestinationCountry,
                    Weight = x.Weight,
                    Price = x.Price,
                })
                .ToList();
            return res;
        }

        // GET: api/Parcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parcel>> GetParcel(Guid id)
        {
            var parcel = await _bll.Parcels.FindAsync(id);

            if (parcel == null)
            {
                return NotFound();
            }

            var parcelFromDb = new App.Public.DTO.v1.Parcel()
            {
                Id = parcel.Id,
                ParcelNumber = parcel.ParcelNumber,
                RecipientName = parcel.RecipientName,
                DestinationCountry = parcel.DestinationCountry,
                Weight = parcel.Weight,
                Price = parcel.Price,
            };

            return parcelFromDb;
        }

        // PUT: api/Parcels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update parcel entity.
        /// </summary>
        /// <param name="id">Supply parcel entity id you want to change.</param>
        /// <param name="parcel">Supply parcel entity with updated values.</param>
        /// <returns>404 if parcel with given id is not found or 204, if changes were successful</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutParcel(Guid id, Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return BadRequest();
            }

            var parcelFromDb = await _bll.Parcels.FindAsync(id);
            if (parcelFromDb == null)
            {
                return NotFound();
            }

            parcelFromDb.Id = parcel.Id;
            parcelFromDb.ParcelNumber = parcel.ParcelNumber;
            parcelFromDb.RecipientName = parcel.RecipientName;
            parcelFromDb.DestinationCountry = parcel.DestinationCountry;
            parcelFromDb.Weight = parcel.Weight;
            parcelFromDb.Price = parcel.Price;


            _bll.Parcels.Update(parcelFromDb);

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Parcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new parcel entity. Add it to database.
        /// </summary>
        /// <param name="parcel">Supply parcel entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Parcel>> PostParcel(Parcel parcel)
        {
            var parcelFromDb = new App.BLL.DTO.Parcel()
            {
                Id = parcel.Id,
                ParcelNumber = parcel.ParcelNumber,
                RecipientName = parcel.RecipientName,
                DestinationCountry = parcel.DestinationCountry,
                Weight = parcel.Weight,
                Price = parcel.Price,
            };

            _bll.Parcels.Add(parcelFromDb);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetParcel",
                new
                {
                    id = parcel.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                parcel);
        }

        // DELETE: api/Parcels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcel(Guid id)
        {
            var parcel = await _bll.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }

            _bll.Parcels.Remove(parcel);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
