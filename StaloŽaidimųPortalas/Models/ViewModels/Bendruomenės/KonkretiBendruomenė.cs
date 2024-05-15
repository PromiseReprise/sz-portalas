namespace StaloŽaidimųPortalas.Models.ViewModels.Bendruomenės
{
	using System.ComponentModel.DataAnnotations;

	public class KonkretiBendruomenė
	{
		public int BendruomenėsId { get; set; }

		public string BendruomenėsPavadinimas { get; set; }

		public BendruomenėsĮrašai Įrašai { get; set; }

		public class BendruomenėsĮrašai
		{
			public List<Įrašas> Įrašai { get; set; }

			public int? DabartinisPuslapis { get; set; }

			public int? PuslapioDydis { get; set; }

			public int? VisoPuslapių { get; set; }
		}

		public class Įrašas
		{
			public int ĮrašoId {  get; set; }

			public int BendruomenėsId { get; set; }

			public string NaudotojoId {  get; set; }

			public string Autorius { get; set; }

			[Required(ErrorMessage = "Laukas yra privalomas")]
			[StringLength(2000, ErrorMessage = "Maksimalus simbolių skaičius 2000")]
			public string Tekstas {  get; set; }
		}
	}
}
