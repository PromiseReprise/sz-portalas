namespace StaloŽaidimųPortalas.Models.ViewModels.Bendruomenės
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class Bendruomenės
	{
		public Bendruomenė ŽaidimoBendruomenė { get; set; }

		public BendruomeniųSąrašas Sąrašas { get; set; }

		public class Bendruomenė
		{
			public int BendruomenėsId { get; set; }

			public string NaudotojoId { get; set; }

			[StringLength(450)]
			public string Autorius { get; set; }

			[Required(ErrorMessage = "Žaidimas yra privalomas")]
			[MustBeGreaterThanZero(ErrorMessage = "Pasirinkite stalo žaidimą.")]
			public int ŽaidimoId { get; set; }

			[DisplayName("Stalo žaidimo pavadinimas")]
			public string ŽaidimoPavadinimas { get; set; }

			[Required(ErrorMessage = "Pavadinimas yra privalomas"), StringLength(250, ErrorMessage = "Maksimalus simbolių skaičius 250")]
			public string Pavadinimas { get; set; }

			[StringLength(2000)]
			public string Aprašymas { get; set; }
		}

		public class BendruomeniųSąrašas
		{
			public List<Bendruomenė> Bendruomenės { get; set; }

			public int? DabartinisPuslapis { get; set; }

			public int? PuslapioDydis { get; set; }

			public int? VisoPuslapių { get; set; }
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
