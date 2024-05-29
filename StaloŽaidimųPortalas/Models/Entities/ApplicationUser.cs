namespace StaloŽaidimųPortalas.Models.Entities
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using System.ComponentModel.DataAnnotations;

	public class ApplicationUser : IdentityUser
	{
		[StringLength(450)]
		public string ContactInformation { get; set; }
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}
