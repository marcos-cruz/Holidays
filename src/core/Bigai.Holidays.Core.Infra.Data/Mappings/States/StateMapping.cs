using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Infra.Data.Mappings.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bigai.Holidays.Core.Infra.Data.Mappings.States
{
    /// <summary>
    /// <see cref="StateMapping"/> Maps a <see cref="State"/> to a database table.
    /// </summary>
    public class StateMapping : EntityMapping<State>
    {
        public override void ConfigureEntity(EntityTypeBuilder<State> builder)
        {
            #region Mappings

            builder.ToTable("States");
            
            builder.Property(c => c.CountryId)
                .IsRequired();

            builder.Property(c => c.CountryIsoCode)
                .HasColumnType($"varchar(3)")
                .IsRequired();

            builder.Property(c => c.StateIsoCode)
                .HasColumnType($"varchar(6)")
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnType($"varchar(100)")
                .IsRequired();

            builder.Property(c => c.PathStateImage)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);

            #endregion

            #region Index

            builder.HasIndex(c => new { c.CountryIsoCode, c.StateIsoCode })
                .IsUnique(false);

            #endregion
        }
    }
}
