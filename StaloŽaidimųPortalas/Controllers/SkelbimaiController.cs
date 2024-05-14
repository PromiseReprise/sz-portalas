namespace StaloŽaidimųPortalas.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using StaloŽaidimųPortalas.Data;
    using StaloŽaidimųPortalas.Models.Entities;
	using StaloŽaidimųPortalas.Models.ViewModels.Skelbimai;
	using System.Linq;
	using System.Security.Claims;
	using static StaloŽaidimųPortalas.Models.ViewModels.Skelbimai.Skelbimai;

	[Authorize]
    public class SkelbimaiController : Controller
	{
		private readonly AplikacijosDbContext _dbContext;
		private readonly AutentikacijosDbContext _autentikacijosDbContext;
		private readonly IMapper _mapper;

		public SkelbimaiController(AplikacijosDbContext dbContext, AutentikacijosDbContext autentikacijosDbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_autentikacijosDbContext = autentikacijosDbContext;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GaukSkelbimus(int puslapioNumeris = 1, int puslapioDydis = 2)
		{
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var visoSkelbimų = await _dbContext.Skelbimai.CountAsync();
			var staloŽaidimai = await _dbContext.StaloŽaidimai.ToListAsync();
			var priregistruotiSkelbimai = await _dbContext.SkelbimųNariai.Where(m => m.NaudotojoId == vartotojoId).Select(m => m.SkelbimoId).ToListAsync();

			var staloŽaidimųSąrašas = _mapper.Map<List<Skelbimai.StaloŽaidimai>>(staloŽaidimai);

			var sąrašas = new Skelbimai
			{
				SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas {},
				Žaidimai = staloŽaidimųSąrašas
			};

			if (visoSkelbimų > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoSkelbimų / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var skelbimai = await _dbContext.Skelbimai
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.Join(_dbContext.StaloŽaidimai,
						skelbimas => skelbimas.ŽaidimoId,
						žaidimas => žaidimas.ŽaidimoId,
						(skelbimas, žaidimas) => new
						{
							Skelbimas = skelbimas,
							Žaidimas = žaidimas
						})
					.Select(joinResult => new Skelbimai.ŽaidimoSkelbimas
					{
						SkelbimoId = joinResult.Skelbimas.SkelbimoId,
						Pavadinimas = joinResult.Skelbimas.Pavadinimas,
						NaudotojoId = joinResult.Skelbimas.NaudotojoId,
						NaudotojoVardas = null,
						ŽaidimoId = joinResult.Žaidimas.ŽaidimoId,
						ŽaidimoPavadinimas = joinResult.Žaidimas.Pavadinimas,
						Aprašymas = joinResult.Skelbimas.Aprašymas,
						ArAutorius = joinResult.Skelbimas.NaudotojoId == vartotojoId,
						ArPartneris = priregistruotiSkelbimai.Contains(joinResult.Skelbimas.SkelbimoId)
					})
					.ToListAsync();

				var skelbimųSąrašas = _mapper.Map<List<Skelbimai.ŽaidimoSkelbimas>>(skelbimai);

				sąrašas.SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas
				{
					Skelbimai = skelbimųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("Skelbimai", sąrašas);
		}

		[HttpPost]
		public async Task<IActionResult> PridėkSkelbimą(Skelbimai.ŽaidimoSkelbimas patalpinamasSkelbimas)
		{
			if (ModelState.IsValid)
			{
				var naujasSkelbimas = new Skelbimas
				{
					Pavadinimas = patalpinamasSkelbimas.Pavadinimas,
					NaudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
					ŽaidimoId = patalpinamasSkelbimas.ŽaidimoId,
					Aprašymas = patalpinamasSkelbimas.Aprašymas
				};

				_dbContext.Skelbimai.Add(naujasSkelbimas);
				await _dbContext.SaveChangesAsync();
			}

			return PartialView("_SkelbimoĮvedimas", patalpinamasSkelbimas);
		}

		[HttpGet]
		public async Task<IActionResult> GaukNaudotojoSkelbimus(int puslapioNumeris = 1, int puslapioDydis = 2)
		{
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var visoSkelbimų = await _dbContext.Skelbimai.Where(m => m.NaudotojoId == vartotojoId).CountAsync();
			var staloŽaidimai = await _dbContext.StaloŽaidimai.ToListAsync();

			var staloŽaidimųSąrašas = _mapper.Map<List<Skelbimai.StaloŽaidimai>>(staloŽaidimai);

			var sąrašas = new Skelbimai
			{
				SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas { },
				Žaidimai = staloŽaidimųSąrašas
			};

			if (visoSkelbimų > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoSkelbimų / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var skelbimai = await _dbContext.Skelbimai
					.Where(m => m.NaudotojoId == vartotojoId)
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.Join(_dbContext.StaloŽaidimai,
						skelbimas => skelbimas.ŽaidimoId,
						žaidimas => žaidimas.ŽaidimoId,
						(skelbimas, žaidimas) => new
						{
							Skelbimas = skelbimas,
							Žaidimas = žaidimas
						})
					.Select(joinResult => new Skelbimai.ŽaidimoSkelbimas
					{
						SkelbimoId = joinResult.Skelbimas.SkelbimoId,
						Pavadinimas = joinResult.Skelbimas.Pavadinimas,
						NaudotojoId = joinResult.Skelbimas.NaudotojoId,
						NaudotojoVardas = null,
						ŽaidimoId = joinResult.Žaidimas.ŽaidimoId,
						ŽaidimoPavadinimas = joinResult.Žaidimas.Pavadinimas,
						Aprašymas = joinResult.Skelbimas.Aprašymas,
						ArAutorius = true
					})
					.ToListAsync();

				var skelbimųSąrašas = _mapper.Map<List<Skelbimai.ŽaidimoSkelbimas>>(skelbimai);

				sąrašas.SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas
				{
					Skelbimai = skelbimųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("NaudotojoSkelbimai", sąrašas);
		}

		[HttpGet]
		public async Task<IActionResult> GaukSkelbimusKaipPartnerio(int puslapioNumeris = 1, int puslapioDydis = 2)
		{
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var priregistruotiSkelbimai = await _dbContext.SkelbimųNariai.Where(m => m.NaudotojoId == vartotojoId).Select(m => m.SkelbimoId).ToListAsync();

			var sąrašas = new Skelbimai
			{
				SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas { }
			};

			if (priregistruotiSkelbimai.Count() > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)priregistruotiSkelbimai.Count() / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var skelbimai = await _dbContext.Skelbimai
					.Where(m => priregistruotiSkelbimai.Contains(m.SkelbimoId))
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.Join(_dbContext.StaloŽaidimai,
						skelbimas => skelbimas.ŽaidimoId,
						žaidimas => žaidimas.ŽaidimoId,
						(skelbimas, žaidimas) => new
						{
							Skelbimas = skelbimas,
							Žaidimas = žaidimas
						})
					.Select(joinResult => new Skelbimai.ŽaidimoSkelbimas
					{
						SkelbimoId = joinResult.Skelbimas.SkelbimoId,
						Pavadinimas = joinResult.Skelbimas.Pavadinimas,
						NaudotojoId = joinResult.Skelbimas.NaudotojoId,
						NaudotojoVardas = null,
						ŽaidimoId = joinResult.Žaidimas.ŽaidimoId,
						ŽaidimoPavadinimas = joinResult.Žaidimas.Pavadinimas,
						Aprašymas = joinResult.Skelbimas.Aprašymas
					})
					.ToListAsync();

				var skelbimųSąrašas = _mapper.Map<List<Skelbimai.ŽaidimoSkelbimas>>(skelbimai);

				sąrašas.SkelbimųSąrašas = new Skelbimai.ŽaidimųSkelbimųSąrašas
				{
					Skelbimai = skelbimųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("SkelbimaiKaipPartnerio", sąrašas);
		}

		[HttpGet]
		public IActionResult GaukSkelbimą(int id)
		{
			var skelbimas = _dbContext.Skelbimai.Find(id);

			if (skelbimas != null)
			{
				return Json(skelbimas);
			}

			return StatusCode(400);
		}

		[HttpPost]
		public async Task<IActionResult> RedaguokSkelbimą(Skelbimai.ŽaidimoSkelbimas redaguojamasSkelbimas)
		{
			if (ModelState.IsValid)
			{
				var dabartinisUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var skelbimas = _dbContext.Skelbimai.FirstOrDefault(m => m.SkelbimoId == redaguojamasSkelbimas.SkelbimoId);

				if (skelbimas != null)
				{
					if (skelbimas.NaudotojoId == dabartinisUserId)
					{
						skelbimas.Pavadinimas = redaguojamasSkelbimas.Pavadinimas;
						skelbimas.ŽaidimoId = redaguojamasSkelbimas.ŽaidimoId;
						skelbimas.Aprašymas = redaguojamasSkelbimas.Aprašymas;

						await _dbContext.SaveChangesAsync();
					} 
					else
					{
						Forbid();
					}
				}
			}

			return PartialView("_SkelbimoRedagavimas", redaguojamasSkelbimas);
		}

		[HttpPost]
		public async Task<IActionResult> PašalinkSkelbimą(int id)
		{
			var skelbimas = await _dbContext.Skelbimai.FindAsync(id);
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (skelbimas != null)
			{
				if (skelbimas.NaudotojoId == vartotojoId)
				{
					_dbContext.Skelbimai.Remove(skelbimas);
					await _dbContext.SaveChangesAsync();
				}
				else
				{
					Forbid();
				}
			}

			return RedirectToAction(nameof(GaukNaudotojoSkelbimus));
		}

		[HttpPost]
		public async Task<IActionResult> PatalpinkSkelbimoNarį(int id)
		{
			var naujasNarys = new SkelbimoNarys
			{
				SkelbimoId = id,
				NaudotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
			};

			_dbContext.SkelbimųNariai.Add(naujasNarys);
			await _dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(GaukSkelbimus));
		}

		[HttpPost]
		public async Task<IActionResult> PašalinkSkelbimoNarį(int id)
		{
			var vartotojoId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var skelbimoNarys = await _dbContext.SkelbimųNariai.FirstOrDefaultAsync(m => m.SkelbimoId == id && m.NaudotojoId == vartotojoId);

			if (skelbimoNarys != null)
			{
				_dbContext.SkelbimųNariai.Remove(skelbimoNarys);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(GaukNaudotojoSkelbimus));
		}

		[HttpGet]
		public IActionResult GaukSkelbimoPartnerius(int id)
		{
			var partneriųId = _dbContext.SkelbimųNariai
				.Where(m => m.SkelbimoId == id)
				.Select(m => m.NaudotojoId)
				.ToList();

			if (partneriųId != null && partneriųId.Any())
			{
				List<Partneris> partneriai = _autentikacijosDbContext.Users
					.Where(u => partneriųId.Contains(u.Id))
					.Select(u => new Partneris
					{
						PartnerioId = u.Id,
						PartnerioSlapyvardis = u.UserName
					})
					.ToList();

				return Json(partneriai);
			}

			return Json(new List<Partneris>());
		}

		[HttpGet]
		public async Task<IActionResult> GaukStaloŽaidimus()
		{
			var staloŽaidimai = await _dbContext.StaloŽaidimai.ToListAsync();
			return Json(staloŽaidimai);
		}
	}
}
