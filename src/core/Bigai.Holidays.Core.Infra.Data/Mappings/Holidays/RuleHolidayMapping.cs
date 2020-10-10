using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Infra.Data.Mappings.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bigai.Holidays.Core.Infra.Data.Mappings.Holidays
{
    /// <summary>
    /// <see cref="RuleHolidayMapping"/> Maps a <see cref="RuleHoliday"/> to a database table.
    /// </summary>
    public class RuleHolidayMapping : EntityMapping<RuleHoliday>
    {
        public override void ConfigureEntity(EntityTypeBuilder<RuleHoliday> builder)
        {
            #region Mappings

            builder.ToTable("RulesHolidays");

            builder.Property(c => c.CountryId)
                .IsRequired();
            
            builder.Property(c => c.StateId)
                .IsRequired(false);

            builder.Property(c => c.CountryIsoCode)
                .HasColumnType($"varchar(3)")
                .IsRequired();

            builder.Property(c => c.StateIsoCode)
                .HasColumnType($"varchar(6)")
                .IsRequired(false);

            builder.Property(c => c.CityId)
                .HasColumnType($"varchar(32)")
                .IsRequired(false);
            
            builder.Property(c => c.CityCode)
                .HasColumnType($"varchar(32)")
                .IsRequired(false);
            
            builder.Property(c => c.CityName)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);

            builder.Property(c => c.HolidayType)
                .IsRequired()
                .HasConversion(
                    p => p.Key,
                    p => HolidayType.GetById(p));
            
            builder.Property(c => c.NativeHolidayName)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.EnglishHolidayName)
                .HasColumnType($"varchar(100)")
                .IsRequired();

            builder.Property(c => c.Month)
                .IsRequired(false);
            
            builder.Property(c => c.Day)
                .IsRequired(false);
            
            builder.Property(c => c.Optional)
                .IsRequired();
            
            builder.Property(c => c.BussinessRule)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.Comments)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);
            
            builder.Property(c => c.ComposeKey)
                .HasColumnType($"varchar(32)")
                .IsRequired();

            #endregion

            #region Index

            builder.HasIndex(c => c.CountryIsoCode)
                .IsUnique(false);

            #endregion

        }
    }
}
