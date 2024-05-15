namespace StaloŽaidimųPortalas.Data
{
	using Microsoft.EntityFrameworkCore;
	using StaloŽaidimųPortalas.Models.Entities;

	public class AplikacijosDbContext: DbContext
	{
        public AplikacijosDbContext(DbContextOptions<AplikacijosDbContext> options): base(options)
        {
            
        }

        public DbSet<StaloŽaidimas> StaloŽaidimai { get; set; }

		public DbSet<Skelbimas> Skelbimai { get; set; }

		public DbSet<SkelbimoNarys> SkelbimųNariai { get; set; }

		public DbSet<Bendruomenė> Bendruomenė { get; set; }

		public DbSet<BendruomenėsĮrašas> BendruomenėsĮrašas { get; set; }

		public DbSet<BendruomenėsNarys>	BendruomenėsNarys { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<SkelbimoNarys>()
				.HasKey(sn => new { sn.SkelbimoId, sn.NaudotojoId });

			modelBuilder.Entity<BendruomenėsNarys>()
				.HasKey(sn => new { sn.BendruomenėsId, sn.NaudotojoId });
		}
	}
}
