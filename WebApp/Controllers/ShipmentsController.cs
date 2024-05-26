using Microsoft.AspNetCore.Mvc;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Authorization;
using App.Public.DTO.v1;
using Asp.Versioning;
using Base.Contracts.Domain;
using AutoMapper;

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
        public async Task<IEnumerable<Shipment>> GetShipments()
        {

            return await _bll.Shipments.GetShipments();
        }

        // GET: api/Shipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(Guid id)
        {
            try
            {
                var shipment = await _bll.Shipments.GetShipment(id);
                return shipment;
            }
            catch(Exception ex) 
            {
                return NotFound();
            }

        }

        // PUT: api/Shipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update shipment entity with list of bags. Find entity via parameter id and update it with list of bag numbers
        /// </summary>
        /// <param name="id">Supply shipment entity id you want to change.</param>
        /// <param name="shipment">Supply shipment entity with updated values.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("~/{id}")]
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

            try
            {
                await _bll.Shipments.PutShipment(id, shipment);
            }catch(ArgumentException ex) 
            {
                return NotFound(ex.Message);    
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(ex.Message);
            }

            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Shipments/5/true
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update shipment entity with list of bags. Find entity via parameter id and update it with list of bag numbers
        /// </summary>
        /// <param name="id">Supply shipment entity id you want to change.</param>
        /// <param name="isFinalized">Boolean if shipment should be finalized.</param>
        /// <returns>404 if shipment with given id is not found or 204, if changes were successful</returns>
        [Route("~/{id}/{isFinalized}")]
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutShipment(Guid id, bool isFinalized)
        {
            try
            {
                await _bll.Shipments.PutShipment(id, isFinalized);
            }catch (ArgumentException ex)
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
            try
            {
                _bll.Shipments.PostShipment(shipment);
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
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
