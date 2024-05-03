namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class Skelbimas
	{
		[Key]
        public int SkelbimoId { get; set; }

		[StringLength(250)]
		public string Pavadinimas { get; set; }

		[StringLength(450)]
		public string NaudotojoId { get; set; }

		public int ŽaidimoId { get; set; }

		[StringLength(2000)]
		public string Aprašymas { get; set; }
	}
}
