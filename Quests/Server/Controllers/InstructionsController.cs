using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quests.Server.Data;
using Quests.Shared.Entities.Models;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstructionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Instructions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instruction>>> GetInstructions()
        {
            return await _context.Instructions.ToListAsync();
        }

        // GET: api/Instructions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instruction>> GetInstruction(int id)
        {
            var instruction = await _context.Instructions.FindAsync(id);

            if (instruction == null)
            {
                return NotFound();
            }

            return instruction;
        }

        // PUT: api/Instructions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstruction(int id, Instruction instruction)
        {
            if (id != instruction.Id)
            {
                return BadRequest();
            }

            _context.Entry(instruction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Instructions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Instruction>> PostInstruction(Instruction instruction)
        {
            _context.Instructions.Add(instruction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstruction", new { id = instruction.Id }, instruction);
        }

        // DELETE: api/Instructions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Instruction>> DeleteInstruction(int id)
        {
            var instruction = await _context.Instructions.FindAsync(id);
            if (instruction == null)
            {
                return NotFound();
            }

            _context.Instructions.Remove(instruction);
            await _context.SaveChangesAsync();

            return instruction;
        }

        private bool InstructionExists(int id)
        {
            return _context.Instructions.Any(e => e.Id == id);
        }
    }
}
