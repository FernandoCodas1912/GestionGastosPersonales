

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> entity)
        {
            entity.HasKey(x => x.Id);

            entity
                .Property(x => x.Name)
                .IsRequired();

            entity
                .Property (x => x.Description)
                .IsRequired();

            entity
                .HasMany(x => x.Expenses)
                .WithOne(x => x.ExpenseCategory)
                .HasForeignKey(x => x.ExpenseCategoryId);

            entity
                .HasOne(x => x.User)
                .WithMany(x => x.ExpenseCategories)
                .HasForeignKey(x => x.UserId);

        }
    }
}
