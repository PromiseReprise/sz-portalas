namespace StaloŽaidimųPortalas.Controllers
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using StaloŽaidimųPortalas.Models.ViewModels.Naudotojai;

	public class NaudotojaiController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public NaudotojaiController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
		{ 
			_userManager = userManager;
			_signInManager = signInManager;
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
				var naudotojas = new IdentityUser()
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
    }
}
