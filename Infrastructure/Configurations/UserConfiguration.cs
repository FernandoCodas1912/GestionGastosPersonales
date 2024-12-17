using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(x => x.Id);

            entity
                .Property(x => x.FullName)
                .IsRequired();

            entity
                .Property(x => x.Email)
                .IsRequired();

            entity
                .Property(x => x.Password)
                .IsRequired();

            entity
                .Property(x => x.IsBlocked)
                .HasDefaultValue(false);

            entity
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasMany(x => x.Expenses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            entity
                .HasMany(x => x.ExpenseCategories)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);



        }
    }
}
