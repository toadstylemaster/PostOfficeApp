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
    public class BagWithParcelsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        public BagWithParcelsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
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
            var _bagMapper = new BagWithParcelsMapper(_mapper);
            return (await _bll.BagWithParcels.GetBagWithParcels()).Select(x => _bagMapper.Map(x)!);
        }

        // GET: api/BagWithParcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BagWithParcels>> GetBagWithParcelsById(Guid id)
        {
            var _bagWithParcelsMapper = new BagWithParcelsMapper(_mapper);
            try
            {
                return _bagWithParcelsMapper.Map(await _bll.BagWithParcels.FindAsync(id, true))!;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get all bagWithParcels entities that are linked with given shipment entity.
        /// </summary>
        /// <returns>List of all Parcels that are linked with given shipment</returns>
        [Route("Bags/byShipments")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BagWithParcels>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.BagWithParcels>>> GetBagWithParcelsByShipment(Guid shipmentId)
        {
            var _bagMapper = new BagWithParcelsMapper(_mapper);
            return (await _bll.BagWithParcels.GetBagWithParcelsByShipmentId(shipmentId)).Select(x => _bagMapper.Map(x)!).ToList() ?? new List<BagWithParcels>();
        }

        // PUT: api/Shipments/5/PutBags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update bagWithParcels with parcels. Find entity via parameter id and update it with.
        /// </summary>
        /// <param name="id">Supply bagWithParcels entity id you want to change.</param>
        /// <param name="parcels">Supply list of parcel entities you want to add to bagWithParcels entity.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("{id}/Parcels")]
        [HttpPut("{id}/Parcels")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutParcelsToBag(Guid id, List<Parcel> parcels)
        {
            var _parcelMapper = new ParcelMapper(_mapper);
            var _bagWithParcelsMapper = new BagWithParcelsMapper(_mapper);
            try
            {
                var bagWithParcels = await _bll.BagWithParcels.FindAsync(id, true);
                if (bagWithParcels == null || bagWithParcels.ShipmentId == null)
                {
                    return BadRequest("No bag with parcels with such id!");
                }
                var shipment = await _bll.Shipments.FindAsync((Guid)bagWithParcels.ShipmentId, true);
                if (shipment != null && shipment.IsFinalized)
                {
                    return BadRequest("No bag with parcels with such id!");
                }

                var finalList = await _bll.Parcels.PutParcelsToBagWithParcels(parcels.Select(x => _parcelMapper.Map(x)!).ToList(), bagWithParcels!);

                bagWithParcels.ListOfParcels = finalList.ToList();
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
        /// <param name="bagWithParcels">Supply bagWithParcels entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BagWithParcels>> PostBagWithParcels(BagWithParcels bagWithParcels)
        {
            var _bagMapper = new BagWithParcelsMapper(_mapper);
            var newBagWithParcels = new BagWithParcels();
            try
            {
                newBagWithParcels = _bagMapper.Map(_bll.BagWithParcels.PostBagWithParcels(_bagMapper.Map(bagWithParcels)!));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithParcelsById",
                new
                {
                    id = newBagWithParcels!.Id,
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
