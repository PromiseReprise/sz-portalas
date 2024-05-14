namespace StaloŽaidimųPortalas.Models.ViewModels
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class StaloŽaidimai
    {
        public StaloŽaidimas Žaidimas { get; set; }

        public StaloŽaidimųSąrašas ŽaidimųSąrašas { get; set; }

		public class StaloŽaidimas
		{
			public int ŽaidimoId { get; set; }

			[DisplayName("Pavadinimas")]
			[Required(ErrorMessage = "Privalomas laukas"), StringLength(250, ErrorMessage = "Maksimalus simbolių skaičius 250")]
			public string Pavadinimas { get; set; }

			[DisplayName("Kategorija")]
			[Required(ErrorMessage = "Privalomas laukas"), StringLength(250, ErrorMessage = "Maksimalus simbolių skaičius 250")]
			public string Kategorija { get; set; }

			[DisplayName("Minimalus žaidėjų kiekis")]
			[Range(0, int.MaxValue, ErrorMessage = "Prašome įvesti skaičių")]
			public int? ŽaidėjųKiekisMin { get; set; }

			[DisplayName("Maksimalus žaidėjų kiekis")]
			[Range(0, int.MaxValue, ErrorMessage = "Prašome įvesti skaičių")]
			public int? ŽaidėjųKiekisMax { get; set; }
		}

		public class StaloŽaidimųSąrašas
		{
			public List<StaloŽaidimas> Žaidimai { get; set; }

			public int? DabartinisPuslapis { get; set; }

			public int? PuslapioDydis { get; set; }

			public int? VisoPuslapių { get; set; }
		}
	}
}
