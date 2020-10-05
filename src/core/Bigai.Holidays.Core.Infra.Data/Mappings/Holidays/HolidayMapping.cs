using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Infra.Data.Mappings.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bigai.Holidays.Core.Infra.Data.Mappings.Holidays
{
    /// <summary>
    /// <see cref="HolidayMapping"/> Maps a <see cref="Holiday"/> to a database table.
    /// </summary>
    public class HolidayMapping : EntityMapping<Holiday>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Holiday> builder)
        {
            #region Mappings

            builder.ToTable("Holidays");

            builder.Property(c => c.CountryId)
                .IsRequired();

            builder.Property(c => c.StateId)
                .IsRequired(false);

            builder.Property(c => c.CityId)
                .HasColumnType($"varchar(32)")
                .IsRequired(false);

            builder.Property(c => c.HolidayDate)
                .IsRequired();

            builder.Property(c => c.HolidayType)
                .IsRequired()
                .HasConversion(
                    p => p.Key,
                    p => HolidayType.GetById(p));

            builder.Property(c => c.Optional)
                .IsRequired();

            builder.Property(c => c.NativeDescription)
                .HasColumnType($"varchar(100)")
                .IsRequired();

            builder.Property(c => c.AlternativeDescription)
                .HasColumnType($"varchar(100)")
                .IsRequired();

            builder.Property(c => c.CountryCode)
                .HasColumnType($"varchar(3)")
                .IsRequired();

            builder.Property(c => c.StateCode)
                .HasColumnType($"varchar(2)")
                .IsRequired(false);
            
            builder.Property(c => c.CityName)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);

            builder.Property(c => c.CityCode)
                .HasColumnType($"varchar(32)")
                .IsRequired(false);

            builder.Property(c => c.ComposeKey)
                .HasColumnType($"varchar(32)")
                .IsRequired();

            #endregion

            #region Index

            builder.HasIndex(c => c.CountryCode)
                .IsUnique(false);

            #endregion

        }
    }
}
