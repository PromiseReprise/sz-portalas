namespace StaloŽaidimųPortalas.Controllers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Data;
	using StaloŽaidimųPortalas.Models.Entities;
	using StaloŽaidimųPortalas.Models.ViewModels;

	[Authorize]
	public class StaloŽaidimaiController : Controller
	{
		private readonly AplikacijosDbContext _dbContext;
		private readonly IMapper _mapper;

		public StaloŽaidimaiController(AplikacijosDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GaukSąrašą(int puslapioNumeris = 1, int puslapioDydis = 2)
		{
			var visoŽaidimų = await _dbContext.StaloŽaidimai.CountAsync();

			var sąrašas = new StaloŽaidimai
			{
				ŽaidimųSąrašas = new StaloŽaidimai.StaloŽaidimųSąrašas {}
			};

			if (visoŽaidimų > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoŽaidimų / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var staloŽaidimai = await _dbContext.StaloŽaidimai
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.ToListAsync();

				var staloŽaidimųSąrašas = _mapper.Map<List<StaloŽaidimai.StaloŽaidimas>>(staloŽaidimai);

				sąrašas.ŽaidimųSąrašas = new StaloŽaidimai.StaloŽaidimųSąrašas
				{
					Žaidimai = staloŽaidimųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("Sąrašas", sąrašas);
		}

		[HttpPost]
		public async Task<IActionResult> PridėkŽaidimą(StaloŽaidimai staloŽaidimas)
		{
			if (ModelState.IsValid)
			{
					var žaidimas = new StaloŽaidimas
					{
						Pavadinimas = staloŽaidimas.Žaidimas.Pavadinimas,
						Kategorija = staloŽaidimas.Žaidimas.Kategorija,
						ŽaidėjųKiekisMin = staloŽaidimas.Žaidimas.ŽaidėjųKiekisMin,
						ŽaidėjųKiekisMax = staloŽaidimas.Žaidimas.ŽaidėjųKiekisMax
					};

					_dbContext.StaloŽaidimai.Add(žaidimas);
					await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(GaukSąrašą));
		}

		[HttpGet]
		public IActionResult GaukŽaidimą(int id)
		{
			var žaidimas = _dbContext.StaloŽaidimai.Find(id);

			if (žaidimas != null)
			{
				return Json(žaidimas);
			}

			return StatusCode(400);
		}

		[HttpPost]
		public IActionResult RedaguokŽaidimą(StaloŽaidimai.StaloŽaidimas staloŽaidimas)
		{
			if (ModelState.IsValid)
			{
				var žaidimas = _dbContext.StaloŽaidimai.FirstOrDefault(m => m.ŽaidimoId == staloŽaidimas.ŽaidimoId);

				if (žaidimas != null)
				{
					žaidimas.Pavadinimas = staloŽaidimas.Pavadinimas;
					žaidimas.Kategorija = staloŽaidimas.Kategorija;
					žaidimas.ŽaidėjųKiekisMin = staloŽaidimas.ŽaidėjųKiekisMin;
					žaidimas.ŽaidėjųKiekisMax = staloŽaidimas.ŽaidėjųKiekisMax;

					_dbContext.SaveChanges();

					return RedirectToAction(nameof(GaukSąrašą));
				}

				return RedirectToAction(nameof(GaukSąrašą));
			}

			return PartialView("_StaloŽaidimoRedagavimas", staloŽaidimas);
		}

		[HttpPost]
		public async Task<IActionResult> PašalinkŽaidimą(int id)
		{
			var žaidimas = await _dbContext.StaloŽaidimai.FindAsync(id);

			if (žaidimas != null)
			{
				_dbContext.StaloŽaidimai.Remove(žaidimas);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(GaukSąrašą));
		}
	}
}
