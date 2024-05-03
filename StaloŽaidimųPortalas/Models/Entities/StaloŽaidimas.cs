namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class StaloŽaidimas
	{
		[Key]
        public int ŽaidimoId { get; set; }

		[StringLength(250)]
        public string Pavadinimas { get; set; }

		[StringLength(250)]
		public string Kategorija { get; set; }

		public int? ŽaidėjųKiekisMin { get; set; }

		public int? ŽaidėjųKiekisMax { get; set; }
	}
}
