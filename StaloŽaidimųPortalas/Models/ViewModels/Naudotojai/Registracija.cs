namespace StaloŽaidimųPortalas.Models.ViewModels.Naudotojai
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class Registracija
	{
		[Required(ErrorMessage = "Privalomas laukas")]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Elektroninio pašto adresas")]
        public string ElPaštas { get; set; }

		[Required(ErrorMessage = "Privalomas laukas")]
		[DisplayName("Slapyvardis")]
		public string Slapyvardis { get; set; }

		[Required(ErrorMessage = "Privalomas laukas")]
		[DataType(DataType.Password)]
		[DisplayName("Slaptažodis")]
		public string Slaptažodis { get; set; }

		[Required(ErrorMessage = "Privalomas laukas")]
		[DataType(DataType.Password)]
		[DisplayName("Pakartotinas slaptažodis")]
		[Compare(nameof(Slaptažodis), ErrorMessage = "Abu slaptažodžiai turi sutapti!")]
		public string SlaptažodžioPatvirtinimas { get; set; }
	}
}
