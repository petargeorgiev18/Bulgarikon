using Bulgarikon.Data.Models;
using Bulgarikon.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulgarikon.Data.Configurations
{
    public class CivilizationConfiguration : IEntityTypeConfiguration<Civilization>
    {
        public void Configure(EntityTypeBuilder<Civilization> builder)
        {
            builder.HasData(
                new Civilization
                {
                    Id = SeededIds.Civ_Thracians,

                }
            );
        }
    }
}
