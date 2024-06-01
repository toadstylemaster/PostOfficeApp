using App.BLL.Contracts;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Api controller for Shipments
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        public ShipmentsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/shipments
        /// <summary>
        /// Get all shipment entities.
        /// </summary>
        /// <returns>List of all shipments</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Shipment>), 200)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Shipment>> GetShipments()
        {
            var _shipmentMapper = new ShipmentMapper(_mapper);
            return (await _bll.Shipments.GetShipments()).Select(x => _shipmentMapper.Map(x)!);
        }

        // GET: api/Shipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(Guid id)
        {
            var _shipmentMapper = new ShipmentMapper(_mapper);
            try
            {
                return _shipmentMapper.Map(await _bll.Shipments.GetShipment(id))!;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        // GET: api/shipments/Bags
        /// <summary>
        /// Get all bag entities.
        /// </summary>
        /// <returns>List of all bags</returns>
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Bag>), 200)]
        [Route("Bags/GetBags")]
        [HttpGet("Bags/GetBags")]
        public async Task<IEnumerable<Bag>> GetBags()
        {
            var _bagMapper = new BagMapper(_mapper);

            try
            {
                var finalList = (await _bll.BagWithLetters.GetAllBagWithLettersAsBags()).Select(b => _bagMapper.Map(b)!).ToList();
                finalList.AddRange((await _bll.BagWithParcels.GetAllBagWithParcelsAsBags()).Select(b => _bagMapper.Map(b)!));
                return finalList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "afdjksaljfdsalkjkfldsajklfjsdlkfsjadklfsdajklf");
            }
        }

        // PUT: api/Shipments/5/true
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update shipment entity with list of bags. Find entity via parameter id and update it with list of bag numbers
        /// </summary>
        /// <param name="id">Supply shipment entity id you want to change.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("finalize/{id}")]
        [HttpPut("finalize/{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FinalizeShipment(Guid id)
        {
            try
            {
                await _bll.Shipments.FinalizeShipment(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Shipments/5/Bags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update shipment entity with list of bags. Find entity via parameter id and update it with list of bags
        /// </summary>
        /// <param name="id">Supply shipment entity id you want to change.</param>
        /// <param name="bags">Supply list of bags of bag entities you want to add to shipment entity.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("{id}/Bags")]
        [HttpPut("{id}/Bags")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBagsToShipment(Guid id, [FromBody] List<Bag> bags)
        {
            var _bagMapper = new BagMapper(_mapper);
            var _shipmentMapper = new ShipmentMapper(_mapper);

            try
            {
                var shipment = await _bll.Shipments.GetShipment(id);
                if (shipment == null || shipment.IsFinalized)
                {
                    return BadRequest("No shipment with such id or shipment has already been finalized!");
                }

                var finalList = (await _bll.BagWithLetters.AddBagWithLettersToShipment(bags.Select(x => _bagMapper.Map(x)!).ToList(), shipment)).ToList();
                finalList.AddRange((await _bll.BagWithParcels.AddBagWithParcelsToShipment(bags.Select(x => _bagMapper.Map(x)!).ToList(), shipment)).ToList());

                shipment.ListOfBags = finalList.ToList();

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Shipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new shipment entity. Add it to database.
        /// </summary>
        /// <param name="shipment">Supply shipment entity you want to add to database</param>
        /// <returns>Status code 201</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
        {
            var _shipmentMapper = new ShipmentMapper(_mapper);
            var newShipment = new Shipment();
            try
            {
                newShipment = _shipmentMapper.Map(_bll.Shipments.PostShipment(_shipmentMapper.Map(shipment)!));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetShipment",
                new
                {
                    id = newShipment!.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                newShipment);
        }


        // DELETE: api/Shipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(Guid id)
        {
            try
            {
                await _bll.Shipments.DeleteShipmentFromDb(id);

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
