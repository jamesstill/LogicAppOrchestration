using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SourceApi.Models;

namespace SourceApi.Repository
{
    public class WidgetMap : IEntityTypeConfiguration<Widget>
    {
        public void Configure(EntityTypeBuilder<Widget> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable<Widget>("Widget");
            builder.HasKey(i => i.Id).IsClustered(false);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Name);
            builder.Property(i => i.Color);
        }
    }
}
