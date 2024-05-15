namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class BendruomenėsNarys
	{
		public int BendruomenėsId { get; set; }

		[StringLength(450)]
		public string NaudotojoId { get; set; }

		public bool ArAdministratorius { get; set; }

		public bool ArAktyvus {  get; set; }
	}
}
