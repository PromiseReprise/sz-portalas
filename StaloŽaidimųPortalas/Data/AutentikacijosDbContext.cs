namespace StaloŽaidimųPortalas.Data
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Models.Entities;

	public class AutentikacijosDbContext: IdentityDbContext<ApplicationUser>
	{
        public AutentikacijosDbContext(DbContextOptions<AutentikacijosDbContext> options): base(options)
        {
            
        }
	}
}
