using Microsoft.EntityFrameworkCore;
using NutritionService.Models;

namespace NutritionService.Data.Context
{
    public class NutritionDbContext : DbContext

    {
        public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options)
        {
        }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingredient>()
     .HasOne(i => i.Meal)             
     .WithMany(m => m.Ingredients)    
     .HasForeignKey(i => i.MealId)      
     .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Meal>()
     .Property(m => m.Name)
     .IsRequired()
     .HasMaxLength(200);

            modelBuilder.Entity<Meal>()
                .Property(m => m.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Quantity)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Meal>()
                .Property(m => m.Protein)
                .HasPrecision(10, 2); 

            modelBuilder.Entity<Meal>()
                .Property(m => m.Carbohydrates)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Meal>()
                .Property(m => m.Fat)
                .HasPrecision(10, 2);

        }

    }
}
