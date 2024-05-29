namespace StaloŽaidimųPortalas.Controllers
{
	using AutoMapper;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Data;
	using StaloŽaidimųPortalas.Models.Entities;
	using StaloŽaidimųPortalas.Models.ViewModels;
	using StaloŽaidimųPortalas.Models.ViewModels.Naudotojai;
	using System;

	public class NaudotojaiController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AutentikacijosDbContext _autentikacijosDbContext;
		private readonly IMapper _mapper;

		public NaudotojaiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, AutentikacijosDbContext autentikacijosDbContext, IMapper mapper) 
		{ 
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_autentikacijosDbContext = autentikacijosDbContext;
			_mapper = mapper;
		}

		public IActionResult Registracija()
		{
			return View("Registracija");
		}

		[HttpPost]
		public async Task<IActionResult> Registruok(Registracija registracijosDuomenys)
		{
			if (ModelState.IsValid)
			{
				var naudotojas = new ApplicationUser()
				{
					Email = registracijosDuomenys.ElPaštas,
					UserName = registracijosDuomenys.Slapyvardis
				};

				var atsakymas = await _userManager.CreateAsync(naudotojas, registracijosDuomenys.Slaptažodis);

				if (atsakymas.Succeeded)
				{
					await _signInManager.SignInAsync(naudotojas, false);

					return RedirectToAction("Index", "Home");
				}

				foreach (var klaida in atsakymas.Errors) 
				{
					ModelState.AddModelError("", klaida.Description);
				}
			}

			return View("Registracija");
		}

        public IActionResult Prisijungimas()
        {
            return View("Prisijungimas");
        }

		[HttpPost]
        public async Task<IActionResult> Prisijunk(Prisijungimas prisijungimoDuomenys)
        {
			if (ModelState.IsValid)
			{
                var atsakymas = await _signInManager.PasswordSignInAsync(userName: prisijungimoDuomenys.Slapyvardis, password: prisijungimoDuomenys.Slaptažodis, isPersistent: false, lockoutOnFailure: false);

                if (atsakymas.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Slapyvardis arba slaptažodis neteisingas");
            }

            return View("Prisijungimas");
        }

        public async Task<IActionResult> Atsijunk()
        {
			await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profilis()
        {
			var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			var naudotojas = await _autentikacijosDbContext.Users
				.Where(u => u.Id == id)
				.Select(u => new Naudotojas
				{
					NaudotojoId = u.Id,
					Slapyvardis = u.UserName,
					KontaktinėInformacija = u.ContactInformation
				})
				.FirstOrDefaultAsync();

			return View("NaudotojoSąsaja", naudotojas);
        }

		[HttpPost]
		public async Task<IActionResult> RedaguokInformaciją(Naudotojas model)
		{
			if (!ModelState.IsValid)
			{
				return View("NaudotojoSąsaja", model);
			}

			var naudotojas = await _autentikacijosDbContext.Users.FindAsync(model.NaudotojoId);
			if (naudotojas == null)
			{
				return View("NaudotojoSąsaja", model);
			}

			naudotojas.UserName = model.Slapyvardis;
			naudotojas.ContactInformation = model.KontaktinėInformacija;

			if (!string.IsNullOrWhiteSpace(model.Slaptažodis) && !string.IsNullOrWhiteSpace(model.SlaptažodžioPatvirtinimas))
			{
				var passwordValidators = _userManager.PasswordValidators;
				foreach (var validator in passwordValidators)
				{
					var validationResult = await validator.ValidateAsync(_userManager, naudotojas, model.Slaptažodis);
					if (!validationResult.Succeeded)
					{
						foreach (var error in validationResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return View("NaudotojoSąsaja", model);
					}
				}
				await _userManager.RemovePasswordAsync(naudotojas);
				await _userManager.AddPasswordAsync(naudotojas, model.Slaptažodis);
			}

			_autentikacijosDbContext.Update(naudotojas);
			await _autentikacijosDbContext.SaveChangesAsync();

			ViewBag.Success = true;

			return View("NaudotojoSąsaja", model);
		}

		[Authorize(Roles = "SysAdmin, Admin")]
		public async Task<IActionResult> Admin(int puslapioNumeris = 1, int puslapioDydis = 10)
		{
			var visoNaudotojųBeRolės = await _autentikacijosDbContext.Users
										.Where(c => !_autentikacijosDbContext.UserRoles
											.Select(b => b.UserId).Distinct()
											.Contains(c.Id)
										).CountAsync();

			var sąrašas = new NaudotojųSąrašas
			{
				Naudotojai = new NaudotojųSąrašas.NaudotojųDuomenųSąrašas { }
			};

			if (visoNaudotojųBeRolės > 0)
			{
				var visoPuslapių = (int)Math.Ceiling((double)visoNaudotojųBeRolės / puslapioDydis);

				if (puslapioNumeris < 1)
				{
					puslapioNumeris = 1;
				}
				else if (puslapioNumeris > visoPuslapių)
				{
					puslapioNumeris = visoPuslapių;
				}

				int kiekPraleisti = (puslapioNumeris - 1) * puslapioDydis;

				var naudotojai = await _autentikacijosDbContext.Users
					.Where(c => !_autentikacijosDbContext.UserRoles
						.Select(b => b.UserId).Distinct()
						.Contains(c.Id))
					.Skip(kiekPraleisti)
					.Take(puslapioDydis)
					.ToListAsync();

				var naudotojųSąrašas = naudotojai.Select(u => new NaudotojųSąrašas.NaudotojoDuomenys
				{
					Id = u.Id,
					Slapyvardis = u.UserName,
					ElPaštas = u.Email
				}).ToList();

				sąrašas.Naudotojai = new NaudotojųSąrašas.NaudotojųDuomenųSąrašas
				{
					NaudotojoDuomenys = naudotojųSąrašas,
					DabartinisPuslapis = puslapioNumeris,
					PuslapioDydis = puslapioDydis,
					VisoPuslapių = visoPuslapių
				};
			}

			return View("Admin", sąrašas);
		}

		[HttpPost]
		public async Task<IActionResult> SkirkAdmin(string id)
		{
			var naudotojas = await _autentikacijosDbContext.Users.FindAsync(id);

			if (naudotojas != null)
			{
				var naujaRolė = new IdentityUserRole<string>
				{
					UserId = id,
					RoleId = "2"
				};

				await _autentikacijosDbContext.UserRoles.AddAsync(naujaRolė);
				await _autentikacijosDbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Admin));
		}
	}
}
