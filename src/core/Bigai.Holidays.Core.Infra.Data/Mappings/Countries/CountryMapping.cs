using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Infra.Data.Mappings.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bigai.Holidays.Core.Infra.Data.Mappings.Countries
{
    /// <summary>
    /// <see cref="CountryMapping"/> Maps a <see cref="Country"/> to a database table.
    /// </summary>
    public class CountryMapping : EntityMapping<Country>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Country> builder)
        {
            #region Mappings

            builder.ToTable("Countries");

            builder.Property(c => c.NumericCode)
                .HasColumnType($"varchar(32)")
                .IsRequired(false);

            builder.Property(c => c.CountryIsoCode2)
                .HasColumnType($"varchar(2)")
                .IsRequired();

            builder.Property(c => c.CountryIsoCode3)
                .HasColumnType($"varchar(3)")
                .IsRequired();
            
            builder.Property(c => c.Name)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.ShortName)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.LanguageCode)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.RegionName)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.SubRegionName)
                .HasColumnType($"varchar(100)")
                .IsRequired();
            
            builder.Property(c => c.IntermediateRegionName)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);

            builder.Property(c => c.RegionCode)
                .IsRequired();
            
            builder.Property(c => c.SubRegionCode)
                .IsRequired();
            
            builder.Property(c => c.IntermediateRegionCode)
                .IsRequired();

            builder.Property(c => c.PathCountryImage)
                .HasColumnType($"varchar(100)")
                .IsRequired(false);

            #endregion

            #region Index

            builder.HasIndex(c => c.CountryIsoCode2)
                .IsUnique();

            builder.HasIndex(c => c.CountryIsoCode3)
                .IsUnique();

            #endregion

            #region Relationships

            builder
                .HasMany(country => country.States)
                .WithOne(state => state.Country)
                .HasForeignKey(state => state.CountryId);

            builder
                .HasMany(country => country.RulesHolidays)
                .WithOne(ruleHoliday => ruleHoliday.Country)
                .HasForeignKey(ruleHoliday => ruleHoliday.CountryId);

            builder
                .HasMany(country => country.Holidays)
                .WithOne(holiday => holiday.Country)
                .HasForeignKey(holiday => holiday.CountryId);

            #endregion
        }
    }
}
