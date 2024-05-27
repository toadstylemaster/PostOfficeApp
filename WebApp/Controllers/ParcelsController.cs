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
    public class ParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public ParcelsController(IAppBLL bll)
        {
            _bll = bll;
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
            var newParcel = new Parcel();
            try
            {
                newParcel = _bll.Parcels.PostParcel(parcel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetParcel",
                new
                {
                    id = newParcel.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                newParcel);
        }

        // DELETE: api/Parcels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcel(Guid id)
        {
            try
            {
                await _bll.Parcels.RemoveParcelFromDb(id);

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
