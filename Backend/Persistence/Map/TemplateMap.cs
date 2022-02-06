using Email.Management.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Email.Management.Persistence.Map
{
    public class TemplateMap : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Subject).HasMaxLength(50);
            builder.Property(x => x.Content).HasColumnType("MEDIUMTEXT");
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.IsHtml).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Templates)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Mail)
                .WithMany(x => x.Templates)
                .HasForeignKey(x => x.MailId);
        }
    }
}
