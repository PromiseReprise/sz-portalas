namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class Bendruomenė
	{
		[Key]
        public int BendruomenėsId { get; set; }

		[StringLength(450)]
		public string NaudotojoId { get; set; }

		public int ŽaidimoId { get; set; }

		[StringLength(450)]
		public string Pavadinimas { get; set; }

		[StringLength(2000)]
		public string Aprašymas { get; set; }
	}
}
