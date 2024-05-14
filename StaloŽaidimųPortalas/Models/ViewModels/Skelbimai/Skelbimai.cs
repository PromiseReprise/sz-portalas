namespace StaloŽaidimųPortalas.Models.ViewModels.Skelbimai
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class Skelbimai
	{
		public ŽaidimoSkelbimas Skelbimas { get; set; }

		public ŽaidimųSkelbimųSąrašas SkelbimųSąrašas { get; set; }

		public List<StaloŽaidimai> Žaidimai { get; set; }

		public List<Partneris> Partneriai { get; set; }

		public class ŽaidimoSkelbimas
		{
			public int SkelbimoId { get; set; }

			[DisplayName("Pavadinimas")]
			[Required(ErrorMessage = "Pavadinimas yra privalomas"), StringLength(250, ErrorMessage = "Maksimalus simbolių skaičius 250")]
			public string Pavadinimas { get; set; }

			public string NaudotojoId { get; set; }

			public string NaudotojoVardas { get; set; }

			[Required(ErrorMessage = "Žaidimas yra privalomas")]
			[MustBeGreaterThanZero(ErrorMessage = "Pasirinkite stalo žaidimą.")]
			public int ŽaidimoId { get; set; }

			[DisplayName("Stalo žaidimo pavadinimas")]
			public string ŽaidimoPavadinimas { get; set; }

			[DisplayName("Skelbimo aprašymas")]
			[StringLength(2000, ErrorMessage = "Maksimalus simbolių skaičius 2000")]
			public string Aprašymas { get; set; }

			public bool ArAutorius { get; set; }

			public bool ArPartneris { get; set; }
		}

		public class ŽaidimųSkelbimųSąrašas
		{
			public List<ŽaidimoSkelbimas> Skelbimai { get; set; }

			public int? DabartinisPuslapis { get; set; }

			public int? PuslapioDydis { get; set; }

			public int? VisoPuslapių { get; set; }
		}

		public class StaloŽaidimai
		{
			public int ŽaidimoId { get; set; }

			public string Pavadinimas { get; set; }

			public string Kategorija { get; set; }
		}

		public class Partneris
		{
			public string PartnerioId { get; set; }

			public string PartnerioSlapyvardis { get; set; }
		}

		public class MustBeGreaterThanZeroAttribute : ValidationAttribute
		{
			public override bool IsValid(object value)
			{
				if (value is int intValue)
				{
					return intValue > 0;
				}
				return false;
			}
		}
	}
}
