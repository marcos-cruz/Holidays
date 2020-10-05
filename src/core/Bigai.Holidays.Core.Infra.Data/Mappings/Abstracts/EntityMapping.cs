using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bigai.Holidays.Core.Infra.Data.Mappings.Abstracts
{
    /// <summary>
    /// <see cref="EntityMapping{TEntity}"/> represents the settings for mapping basic classes to database tables.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public abstract class EntityMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Apply settings for class <see cref="TEntity"/>.
        /// </summary>
        /// <param name="builder">Instância para aplicar configuração.</param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .IsRequired();

            builder.Property(entity => entity.Status)
                .IsRequired()
                .HasColumnType($"varchar({EntityStatus.SizeCodeStatus})")
                .HasConversion(
                    p => p.Key,
                    p => EntityStatus.GetById(p));

            builder.Property(entity => entity.RegisteredBy)
                .IsRequired();

            builder.Property(entity => entity.RegistrationDate)
                .IsRequired();

            builder.Property(entity => entity.ModifiedBy)
                .IsRequired(false);

            builder.Property(entity => entity.ModificationDate)
                .IsRequired(false);

            builder.Ignore(entity => entity.Action);

            ConfigureEntity(builder);
        }

        /// <summary>
        /// Apply the settings to the class that you inherited from <see cref="TEntity"/>.
        /// </summary>
        /// <param name="builder">Instância para aplicar configuração.</param>
        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
