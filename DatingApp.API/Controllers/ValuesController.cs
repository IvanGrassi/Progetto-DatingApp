using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    //http:localhost:500/api/values
    //dove values = [controller] = Values
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context; //underscore é puramente usato in questo progetto per scelta, metodo privato
        public ValuesController(DataContext context)
        {
            this._context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            //throw new Exception("Test Exception");
            //return new string[] { "value1", "value2" };
            var values = await _context.Values.ToListAsync();  //i valori vengono ottenuti come lista e contenuti nella variabile
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            //return "value";
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id); //recuperiamo il primo elemento che corrisponde all'id, altrimenti viene passato uno di default (evita exception)
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}