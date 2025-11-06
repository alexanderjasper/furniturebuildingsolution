using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FurnitureBuildingSolution.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<DbBookcase> Bookcases { get; set; }
        public DbSet<DbCompartment> Compartments { get; set; }
        public DbSet<DbPlate> Plates { get; set; }
        public DbSet<DbMaterial> Materials { get; set; }
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbOrder> Orders { get; set; }
        public DbSet<DbAddress> Addresses { get; set; }
        public DbSet<DbOrderItem> OrderItems { get; set; }
        public DbSet<DbEmail> Emails { get; set; }
        public DbSet<DbProductCategory> ProductCategories { get; set; }
        public DbSet<DbStandardModel> StandardModels { get; set; }

        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries<BaseDbEntity>().Where(entityEntry => entityEntry.State == EntityState.Added).ToList();
            var now = DateTime.Now;
            AddedEntities.ForEach(entityEntry =>
            {
                entityEntry.Entity.Created = now;
                entityEntry.Entity.Modified = now;
            });

            var ModifiedEntities = ChangeTracker.Entries<BaseDbEntity>().Where(entityEntry => entityEntry.State == EntityState.Modified).ToList();
            ModifiedEntities.ForEach(entityEntry =>
            {
                entityEntry.Entity.Modified = now;
            });

            var DeletedEntities = ChangeTracker.Entries<BaseDbEntity>().Where(entityEntry => entityEntry.State == EntityState.Deleted).ToList();
            DeletedEntities.ForEach(entityEntry =>
            {
                entityEntry.State = EntityState.Unchanged;
                entityEntry.Entity.Deleted = now;
            });

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAddress>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbBookcase>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbCompartment>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbMaterial>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbEmail>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbOrder>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbOrderItem>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbPlate>().HasQueryFilter(entity => entity.Deleted == null)
            .HasOne<DbCompartment>(p => p.ParentCompartment)
            .WithMany(c => c.InternalPlates);
            modelBuilder.Entity<DbUser>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbProductCategory>().HasQueryFilter(entity => entity.Deleted == null);
            modelBuilder.Entity<DbStandardModel>().HasQueryFilter(entity => entity.Deleted == null);
        }
    }
}