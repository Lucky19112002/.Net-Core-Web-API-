using CrudOperationIn.NetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrudOperationIn.NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;
        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get All Data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            return await _dbContext.Brands.ToListAsync();
        }

        //Get DAta by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        //Get data from frontend and save the data to database
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new { id = brand.ID }, brand);
        }

        //Update the data in database
        [HttpPut]
        public async Task<ActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        //Delete data from database
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            _dbContext.Brands.Remove(brand);

            await _dbContext.SaveChangesAsync();
            return Ok();        }
    }
}
