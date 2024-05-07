using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Authorization;
using App.Public.DTO.v1;
using Asp.Versioning;

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

        public ShipmentsController(IAppBLL bll)
        {
            _bll = bll;
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
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            var res = (await _bll.Shipments.AllAsync())
                .Select(x => new App.Public.DTO.v1.Shipment()
                {
                    Id = x.Id,
                    ShipmentNumber = x.ShipmentNumber,
                    Airport = x.Airport,
                    FlightNumber = x.FlightNumber,
                    FlightDate = x.FlightDate,
                    ListOfBags = x.ListOfBags ?? new List<string>()
                })
                .ToList();
            return res;
        }

        // GET: api/Shipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(Guid id)
        {
            var shipment = await _bll.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            var shipmentFromDb = new App.Public.DTO.v1.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = shipment.ListOfBags ?? new List<string>()
            };


            return shipmentFromDb;
        }

        // PUT: api/Shipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update shipment entity with list of bags. Find entity via parameter id and update it with list of bag numbers
        /// </summary>
        /// <param name="id">Supply shipment entity id you want to change.</param>
        /// <param name="shipment">Supply shipment entity with updated values.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutShipment(Guid id, App.Public.DTO.v1.Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest();
            }

            var shipmentFromDb = await _bll.Shipments.FindAsync(id);
            if (shipmentFromDb == null)
            {
                return NotFound();
            }

            shipmentFromDb.ShipmentNumber = shipment.ShipmentNumber;
            shipmentFromDb.Airport = shipment.Airport;
            shipmentFromDb.FlightNumber = shipment.FlightNumber;
            shipmentFromDb.FlightDate = shipment.FlightDate;
            shipmentFromDb.ListOfBags = shipment.ListOfBags;


            _bll.Shipments.Update(shipmentFromDb);

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
            var shipmentFromDb = new App.BLL.DTO.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = shipment.ListOfBags
            };

            _bll.Shipments.Add(shipmentFromDb);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetShipment", 
                new 
                {
                id = shipment.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
                }, 
                shipment);
        }

        // DELETE: api/Shipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(Guid id)
        {
            var shipment = await _bll.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _bll.Shipments.Remove(shipment);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
