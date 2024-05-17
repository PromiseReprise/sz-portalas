namespace StaloŽaidimųPortalas.Controllers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Data;
	using StaloŽaidimųPortalas.Models.Entities;
	using StaloŽaidimųPortalas.Models.ViewModels.Bendruomenės;
	using System.Security.Claims;

	public class BendruomenėsController : Controller
	{
		private readonly AplikacijosDbContext _dbContext;
		private readonly AutentikacijosDbContext _autentikacijosDbContext;
		private readonly IMapper _mapper;

		public BendruomenėsController(AplikacijosDbContext dbContext, AutentikacijosDbContext autentikacijosDbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_autentikacijosDbContext = autentikacijosDbContext;
			_mapper = mapper;
		}

		[HttpGet]
        public async Task<IActionResult> GaukBendruomenes(int puslapioNumeris = 1, int puslapioDydis = 10)
        {
			var visoBendruomenių = await _dbContext.Bendruomenė.CountAsync();

			var sąrašas = new Bendruomenės
			{
				Sąrašas = new Bendruomenės.BendruomeniųSąrašas { }
			};

			if (visoBendruomenių > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoBendruomenių / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var bendruomenės = await _dbContext.Bendruomenė
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.Join(_dbContext.StaloŽaidimai,
						bendruomenė => bendruomenė.ŽaidimoId,
						žaidimas => žaidimas.ŽaidimoId,
						(bendruomenė, žaidimas) => new
						{
							Bendruomenė = bendruomenė,
							Žaidimas = žaidimas
						})
					.Select(joinResult => new Bendruomenės.Bendruomenė
					{
						BendruomenėsId = joinResult.Bendruomenė.BendruomenėsId,
						NaudotojoId = joinResult.Bendruomenė.NaudotojoId,
						Autorius = null,
						ŽaidimoId = joinResult.Žaidimas.ŽaidimoId,
						ŽaidimoPavadinimas = joinResult.Žaidimas.Pavadinimas,
						Pavadinimas = joinResult.Bendruomenė.Pavadinimas,
						Aprašymas = joinResult.Bendruomenė.Aprašymas
					})
					.ToListAsync();

				var naudotojoIds = bendruomenės.Select(m => m.NaudotojoId).ToList();

				var userIdToUserNameMap = await _autentikacijosDbContext.Users
					.Where(u => naudotojoIds.Contains(u.Id))
					.Select(u => new { UserId = u.Id, UserName = u.UserName })
					.ToDictionaryAsync(u => u.UserId, u => u.UserName);

				var bendruomenėsSuAutorium = bendruomenės.Select(b =>
				{
					string autorius = userIdToUserNameMap.ContainsKey(b.NaudotojoId) ? userIdToUserNameMap[b.NaudotojoId] : null;

					return new Bendruomenės.Bendruomenė
					{
						BendruomenėsId = b.BendruomenėsId,
						NaudotojoId = b.NaudotojoId,
						Autorius = autorius,
						ŽaidimoId = b.ŽaidimoId,
						ŽaidimoPavadinimas = b.ŽaidimoPavadinimas,
						Pavadinimas = b.Pavadinimas,
						Aprašymas = b.Aprašymas
					};
				})
				.ToList();

				var bendruomeniųSąrašas = _mapper.Map<List<Bendruomenės.Bendruomenė>>(bendruomenėsSuAutorium);

				sąrašas.Sąrašas = new Bendruomenės.BendruomeniųSąrašas
				{
					Bendruomenės = bendruomeniųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("Bendruomenės", sąrašas);
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PridėkBendruomenę(Bendruomenės.Bendruomenė patalpinamaBendruomenė)
		{
			if (ModelState.IsValid)
			{
				var naujaBendruomenė = new Bendruomenė
				{
					Pavadinimas = patalpinamaBendruomenė.Pavadinimas,
					NaudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
					ŽaidimoId = patalpinamaBendruomenė.ŽaidimoId,
					Aprašymas = patalpinamaBendruomenė.Aprašymas
				};

				_dbContext.Bendruomenė.Add(naujaBendruomenė);
				await _dbContext.SaveChangesAsync();

				var naujasBendruomenėsNarys = new BendruomenėsNarys
				{
					BendruomenėsId = naujaBendruomenė.BendruomenėsId,
					NaudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
					ArAdministratorius = true,
					ArAktyvus = true
				};

				_dbContext.BendruomenėsNarys.Add(naujasBendruomenėsNarys);
				await _dbContext.SaveChangesAsync();
			}

			return PartialView("_BendruomenėsĮvedimas", patalpinamaBendruomenė);
		}

		[HttpGet]
		public async Task<IActionResult> GaukBendruomenėsĮrašus(int id, int puslapioNumeris = 1, int puslapioDydis = 10)
		{
			var bendruomenėsInformacija = await _dbContext.Bendruomenė.Where(m => m.BendruomenėsId == id).FirstAsync();
			var visoĮrašų = await _dbContext.BendruomenėsĮrašas.Where(m => m.BendruomenėsId == id).CountAsync();

			var naudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var bendruomenėsNariai = await _dbContext.BendruomenėsNarys.Where(m => m.BendruomenėsId == id && m.ArAktyvus == true && m.NaudotojoId == naudotojoId).FirstOrDefaultAsync();

			var arNarys = false;
			var arAdministratorius = false;

			var bendruomenėsŽaidimas = await _dbContext.StaloŽaidimai.Where(m => m.ŽaidimoId == bendruomenėsInformacija.ŽaidimoId).FirstOrDefaultAsync();

			if (bendruomenėsNariai != null)
			{
				arNarys = true;

				if (bendruomenėsNariai.ArAdministratorius == true) arAdministratorius = true;
			}

			var sąrašas = new KonkretiBendruomenė
			{
				BendruomenėsId = id,
				BendruomenėsPavadinimas = bendruomenėsInformacija.Pavadinimas,
				BendruomenėsŽaidimas = bendruomenėsŽaidimas.Pavadinimas,
				NaudotojoId = naudotojoId,
				ArBendruomenėsNarys = arNarys,
				ArBendruomenėsAdministratorius = arAdministratorius,
				Įrašai = new KonkretiBendruomenė.BendruomenėsĮrašai { }
			};

			if (visoĮrašų > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoĮrašų / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var įrašai = await _dbContext.BendruomenėsĮrašas
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.Where(m => m.BendruomenėsId == id)
					.Select(m => new KonkretiBendruomenė.Įrašas
					{
						ĮrašoId = m.ĮrašoId,
						BendruomenėsId = m.BendruomenėsId,
						NaudotojoId = m.NaudotojoId,
						Autorius = null,
						Tekstas = m.Įrašas
					})
					.ToListAsync();

				var naudotojoIds = įrašai.Select(m => m.NaudotojoId).ToList();

				var userIdToUserNameMap = await _autentikacijosDbContext.Users
					.Where(u => naudotojoIds.Contains(u.Id))
					.Select(u => new { UserId = u.Id, UserName = u.UserName })
					.ToDictionaryAsync(u => u.UserId, u => u.UserName);

				var įrašaiSuAutorium = įrašai.Select(b =>
				{
					string autorius = userIdToUserNameMap.ContainsKey(b.NaudotojoId) ? userIdToUserNameMap[b.NaudotojoId] : null;

					return new KonkretiBendruomenė.Įrašas
					{
						ĮrašoId = b.ĮrašoId,
						BendruomenėsId = b.BendruomenėsId,
						NaudotojoId = b.NaudotojoId,
						Autorius = autorius,
						Tekstas = b.Tekstas
					};
				})
				.ToList();

				var įrašųSąrašas = _mapper.Map<List<KonkretiBendruomenė.Įrašas>>(įrašaiSuAutorium);

				sąrašas.Įrašai = new KonkretiBendruomenė.BendruomenėsĮrašai
				{
					Įrašai = įrašųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("Bendruomenė", sąrašas);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PridėkBendruomenėsĮraša(KonkretiBendruomenė.Įrašas patalpinamasĮrašas)
		{
			if (ModelState.IsValid)
			{
				var naujasĮrašas = new BendruomenėsĮrašas
				{
					BendruomenėsId = patalpinamasĮrašas.BendruomenėsId,
					NaudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
					Įrašas = patalpinamasĮrašas.Tekstas
				};

				_dbContext.BendruomenėsĮrašas.Add(naujasĮrašas);
				await _dbContext.SaveChangesAsync();
			}

			return PartialView("_BendruomenėsĮrašoĮvedimas", patalpinamasĮrašas);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PridėkNarį(int bendruomenėsId, string naudotojoId)
		{
			var bendruomenė = await _dbContext.Bendruomenė.FindAsync(bendruomenėsId);
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (bendruomenė != null)
			{
				if (vartotojoId == naudotojoId)
				{
					var arYraToksNarys = await _dbContext.BendruomenėsNarys.Where(m => m.BendruomenėsId == bendruomenėsId && m.NaudotojoId == naudotojoId).FirstOrDefaultAsync();

					if (arYraToksNarys != null)
					{
						arYraToksNarys.ArAktyvus = true;

						_dbContext.BendruomenėsNarys.Update(arYraToksNarys);
						await _dbContext.SaveChangesAsync();
					}
					else
					{
						var bendruomenėsNarys = new BendruomenėsNarys
						{
							BendruomenėsId = bendruomenėsId,
							NaudotojoId = vartotojoId,
							ArAdministratorius = false,
							ArAktyvus = true
						};

						_dbContext.BendruomenėsNarys.Add(bendruomenėsNarys);
						await _dbContext.SaveChangesAsync();
					}

					return Ok();
				}
				else
				{
					Forbid();
				}
			}

			return NotFound();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AtšaukNarį(int bendruomenėsId, string naudotojoId)
		{
			var bendruomenėsNarys = await _dbContext.BendruomenėsNarys.Where(m => m.BendruomenėsId == bendruomenėsId && m.NaudotojoId == naudotojoId && m.ArAktyvus == true).FirstOrDefaultAsync();
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (bendruomenėsNarys != null)
			{
				if (vartotojoId == naudotojoId)
				{
					bendruomenėsNarys.ArAktyvus = false;

					_dbContext.BendruomenėsNarys.Update(bendruomenėsNarys);
					await _dbContext.SaveChangesAsync();

					return Ok();
				}
				else
				{
					Forbid();
				}
			}

			return NotFound();
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> BendruomenėsRedagavimas(int bendruomenėsId, string naudotojoId)
		{
			var arNaudotojasAdministratorius = await _dbContext.BendruomenėsNarys.Where(m => m.BendruomenėsId == bendruomenėsId && m.NaudotojoId == naudotojoId && m.ArAdministratorius == true).FirstOrDefaultAsync();

			if (arNaudotojasAdministratorius == null) return Forbid();

			var bendruomenėsDuomenys = await _dbContext.Bendruomenė.Where(m => m.BendruomenėsId == bendruomenėsId).FirstOrDefaultAsync();

			var bendruomenė = new Bendruomenės.Bendruomenė
			{
				BendruomenėsId = bendruomenėsDuomenys.BendruomenėsId,
				ŽaidimoId = bendruomenėsDuomenys.ŽaidimoId,
				Pavadinimas = bendruomenėsDuomenys.Pavadinimas,
				Aprašymas = bendruomenėsDuomenys.Aprašymas
			};

			return View("BendruomenėsRedagavimas", bendruomenė);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> RedaguokBendruomenę(Bendruomenės.Bendruomenė redaguojamaBendruomenė)
		{
			if (ModelState.IsValid)
			{
				var dabartinisUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var bendruomenė = _dbContext.Bendruomenė.FirstOrDefault(m => m.BendruomenėsId == redaguojamaBendruomenė.BendruomenėsId);

				if (bendruomenė != null)
				{
					bendruomenė.Pavadinimas = redaguojamaBendruomenė.Pavadinimas;
					bendruomenė.NaudotojoId = dabartinisUserId;
					bendruomenė.ŽaidimoId = redaguojamaBendruomenė.ŽaidimoId;
					bendruomenė.Aprašymas = redaguojamaBendruomenė.Aprašymas;

					await _dbContext.SaveChangesAsync();

					return RedirectToAction(nameof(GaukBendruomenėsĮrašus), new { id = redaguojamaBendruomenė.BendruomenėsId });
				}
			}

			return View("BendruomenėsRedagavimas", redaguojamaBendruomenė);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> PašalinkĮrašą(int įrašoId, string naudotojoId)
		{
			var įrašas = await _dbContext.BendruomenėsĮrašas.FindAsync(įrašoId);
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (įrašas != null)
			{
				if (vartotojoId == naudotojoId)
				{
					_dbContext.BendruomenėsĮrašas.Remove(įrašas);
					await _dbContext.SaveChangesAsync();

					return Ok();
				}
				else
				{
					Forbid();
				}
			}

			return NotFound();
		}
	}
}
