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
    public class BagWithLettersController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public BagWithLettersController(IAppBLL bll)
        {
            _bll = bll;
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
        public async Task<ActionResult<IEnumerable<BagWithLetters>>> GetBagWithLetters()
        {
            var res = (await _bll.BagWithLetters.AllAsync())
                .Select(x => new App.Public.DTO.v1.BagWithLetters()
                {
                    Id = x.Id,
                    BagNumber = x.BagNumber,
                    CountOfLetters = x.CountOfLetters,
                    Weight = x.Weight,
                    Price = x.Price,
                })
                .ToList();
            return res;
        }

        // GET: api/BagWithLetters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BagWithLetters>> GetBagWithLetters(Guid id)
        {
            var bagWithLetters = await _bll.BagWithLetters.FindAsync(id);

            if (bagWithLetters == null)
            {
                return NotFound();
            }

            var bagWithLettersFromDb = new App.Public.DTO.v1.BagWithLetters()
            {
                Id = bagWithLetters.Id,
                BagNumber = bagWithLetters.BagNumber,
                CountOfLetters = bagWithLetters.CountOfLetters,
                Weight = bagWithLetters.Weight,
                Price = bagWithLetters.Price,
            };

            return bagWithLettersFromDb;
        }

        // PUT: api/BagWithLetters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update bagWithLetters entity with list of Letters.
        /// </summary>
        /// <param name="id">Supply parcel entity id you want to change.</param>
        /// <param name="bagWithLetters">Supply bagWithLetters entity with updated values.</param>
        /// <returns>404 if bagWithLetters with given id is not found or 204, if changes were successful</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBagWithLetters(Guid id, BagWithLetters bagWithLetters)
        {
            if (id != bagWithLetters.Id)
            {
                return BadRequest();
            }


            var bagWithLettersFromDb = await _bll.BagWithLetters.FindAsync(id);
            if (bagWithLettersFromDb == null)
            {
                return NotFound();
            }

            bagWithLettersFromDb.Id = bagWithLetters.Id;
            bagWithLettersFromDb.BagNumber = bagWithLetters.BagNumber;
            bagWithLettersFromDb.CountOfLetters = bagWithLetters.CountOfLetters;
            bagWithLettersFromDb.Weight = bagWithLetters.Weight;
            bagWithLettersFromDb.Price = bagWithLetters.Price;

            return NoContent();
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
            var bagWithLettersFromDb = new App.BLL.DTO.BagWithLetters()
            {
                Id = bagWithLetters.Id,
                BagNumber = bagWithLetters.BagNumber,
                CountOfLetters = bagWithLetters.CountOfLetters,
                Weight = bagWithLetters.Weight,
                Price = bagWithLetters.Price,
            };

            _bll.BagWithLetters.Add(bagWithLettersFromDb);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetBagWithLetters",
                new
                {
                    id = bagWithLetters.Id,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                bagWithLetters);
        }

        // DELETE: api/BagWithLetters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBagWithLetters(Guid id)
        {
            var bagWithLetters = await _bll.BagWithLetters.FindAsync(id);
            if (bagWithLetters == null)
            {
                return NotFound();
            }

            _bll.BagWithLetters.Remove(bagWithLetters);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
