namespace StaloŽaidimųPortalas.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class SkelbimoNarys
	{
		public int SkelbimoId { get; set; }

		[StringLength(450)]
		public string NaudotojoId { get; set; }
	}
}
