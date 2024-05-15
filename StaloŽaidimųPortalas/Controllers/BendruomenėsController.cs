namespace StaloŽaidimųPortalas.Controllers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Data;
	using StaloŽaidimųPortalas.Models.Entities;
	using StaloŽaidimųPortalas.Models.ViewModels.Bendruomenės;

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
	}
}
