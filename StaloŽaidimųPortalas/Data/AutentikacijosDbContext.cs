namespace StaloŽaidimųPortalas.Data
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

	public class AutentikacijosDbContext: IdentityDbContext
	{
        public AutentikacijosDbContext(DbContextOptions<AutentikacijosDbContext> options): base(options)
        {
            
        }
	}
}
