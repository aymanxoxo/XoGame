using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using XoGame.Models;

namespace XoGame.Models
{
    public class XoContext : DbContext
    {
        public XoContext()
            : base("name=XoContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<XoContext>());
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<Match> Match { get; set; }
        
        public DbSet<PlayerScore> PlayerScores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>()
                .HasMany(e => e.Matches)
                .WithRequired(e => e.Table);

            modelBuilder.Entity<Table>()
                .HasRequired(e => e.Owner);

            modelBuilder.Entity<Table>()
                .HasMany(x => x.Players)
                .WithMany()
                .Map(x =>
                {
                    x.ToTable("TablePlayers");
                    x.MapLeftKey("TableId");
                    x.MapRightKey("PlayerId");
                });

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        }
    }
}