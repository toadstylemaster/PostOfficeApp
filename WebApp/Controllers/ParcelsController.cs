using App.BLL.Contracts;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        public ParcelsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Parcel entities.
        /// </summary>
        /// <returns>List of all parcels</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Parcel>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Parcel>> GetParcels()
        {
            var _parcelMapper = new ParcelMapper(_mapper);
            return (await _bll.Parcels.GetParcels()).Select(x => _parcelMapper.Map(x)!);
        }

        // GET: api/Parcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parcel>> GetParcel(Guid id)
        {
            var _parcelMapper = new ParcelMapper(_mapper);
            try
            {
                return _parcelMapper.Map(await _bll.Parcels.GetParcel(id))!;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Parcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new parcel entity. Add it to database.
        /// </summary>
        /// <param name="parcel">Supply parcel entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Parcel>> PostParcel(Parcel parcel)
        {
            var newParcel = new Parcel();
            var _parcelMapper = new ParcelMapper(_mapper);
            try
            {
                newParcel = _parcelMapper.Map(_bll.Parcels.PostParcel(_parcelMapper.Map(parcel)!));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetParcel",
                new
                {
                    id = newParcel!.Id,
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
