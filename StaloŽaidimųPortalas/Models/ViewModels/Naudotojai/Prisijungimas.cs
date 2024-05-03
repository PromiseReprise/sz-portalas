namespace StaloŽaidimųPortalas.Models.ViewModels.Naudotojai
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public class Prisijungimas
	{
		[Required(ErrorMessage = "Privalomas laukas")]
		[DisplayName("Slapyvardis")]
        public string Slapyvardis { get; set; }

		[Required(ErrorMessage = "Privalomas laukas")]
		[DataType(DataType.Password)]
		[DisplayName("Slaptažodis")]
		public string Slaptažodis { get; set; }
	}
}
