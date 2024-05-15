namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class BendruomenėsĮrašas
	{
		[Key]
        public int ĮrašoId { get; set; }

		public int BendruomenėsId { get; set; }

		[StringLength(2000)]
		public string Įrašas { get; set; }
	}
}
