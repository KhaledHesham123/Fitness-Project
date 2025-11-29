using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Data
{
    public class NutritionDbContext : DbContext

    {
        public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options)
        {
        }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<UserNutritionProfile> UserNutritionProfiles { get; set; }

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
            modelBuilder.Entity<Ingredient>()
                        .HasData(new Ingredient { Id = 1, Name = "Oats", Quantity = "1", MealId = 1 },
                                 new Ingredient { Id = 2, Name = "Milk", Quantity = "1", MealId = 1 },
                                 new Ingredient { Id = 3, Name = "Banana", Quantity = "1", MealId = 1 }
                        );
            modelBuilder.Entity<Meal>()
                .HasData(
                    new Meal
                    {
                        Id = 1,
                        Name = "Spaghetti Bolognese",
                        Description = "A classic Italian pasta dish with rich meat sauce.",
                        VideoUrl = "https://example.com/spaghetti-bolognese",
                        MealType = Domain.Enums.MealType.Dinner,
                        DifficultyLevel = Domain.Enums.DifficultyLevel.Intermediate,
                        Calories = 600,
                        Protein = 25.50,
                        Carbohydrates = 75.00,
                        Fat = 20.00,
                        ImageUrl = "https://example.com/images/spaghetti-bolognese.jpg"
                    }
                );
        }

    }
}
