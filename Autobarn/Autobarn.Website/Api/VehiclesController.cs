using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;

namespace Autobarn.Website.Api {
	[Route("api/[controller]")]
	[ApiController]
	public class VehiclesController(AutobarnDbContext db, ILogger<VehiclesController> logger)
		: ControllerBase {
		// GET: api/Vehicles
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
			=> await db.Vehicles
				.Include(v => v.Model)
				.ThenInclude(m => m.Make)
				.ToListAsync();

		// GET: api/Vehicles/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Vehicle>> GetVehicle(string id) {
			var vehicle = await db.Vehicles
				.Include(v => v.Model)
				.ThenInclude(m => m.Make)
				.FirstOrDefaultAsync(v => v.Registration == id);
			if (vehicle == null) return NotFound();
			return vehicle;
		}

		// PUT: api/Vehicles/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutVehicle(string id, Vehicle vehicle) {
			if (id != vehicle.Registration) {
				return BadRequest();
			}

			db.Entry(vehicle).State = EntityState.Modified;

			try {
				await db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) {
				if (!VehicleExists(id)) {
					return NotFound();
				} else {
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Vehicles
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Vehicle>> PostVehicle(VehicleDto dto) {
			var model = await db.Models.FirstOrDefaultAsync(m => m.Code == dto.ModelCode);
			if (model == null) return BadRequest($"Sorry, we don't have a car called {dto.ModelCode}");

			var vehicle = new Vehicle {
				Model = model,
				Registration = dto.Registration,
				Year = dto.Year,
				Color = dto.Color
			};

			db.Vehicles.Add(vehicle);
			try {
				await db.SaveChangesAsync();
			}
			catch (DbUpdateException) {
				if (VehicleExists(vehicle.Registration!)) return Conflict();
				throw;
			}
			logger.LogInformation($"Created vehicle: {vehicle}");
			return CreatedAtAction("GetVehicle", new { id = vehicle.Registration }, vehicle);
		}

		// DELETE: api/Vehicles/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteVehicle(string id) {
			var vehicle = await db.Vehicles.FindAsync(id);
			if (vehicle == null) {
				return NotFound();
			}

			db.Vehicles.Remove(vehicle);
			await db.SaveChangesAsync();

			return NoContent();
		}

		private bool VehicleExists(string id) {
			return db.Vehicles.Any(e => e.Registration == id);
		}
	}
}
