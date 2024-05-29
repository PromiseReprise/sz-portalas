using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StaloŽaidimųPortalas.Models.ViewModels.Naudotojai
{
	public class Naudotojas
	{
		public string NaudotojoId { get; set; }

		[Required(ErrorMessage = "Privalomas laukas")]
		[DisplayName("Slapyvardis")]
		[StringLength(256)]
		public string Slapyvardis { get; set; }

		[DisplayName("Kontaktinė informacija")]
		[StringLength(450)]
		public string KontaktinėInformacija { get; set; }

		[DataType(DataType.Password)]
		[DisplayName("Slaptažodis")]
		public string Slaptažodis { get; set; }

		[DataType(DataType.Password)]
		[DisplayName("Pakartotinas slaptažodis")]
		[Compare(nameof(Slaptažodis), ErrorMessage = "Abu slaptažodžiai turi sutapti!")]
		public string SlaptažodžioPatvirtinimas { get; set; }
	}
}
