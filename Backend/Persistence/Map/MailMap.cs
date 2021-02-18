using Email.Management.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Email.Management.Persistence.Map
{
    public class MailMap : IEntityTypeConfiguration<Mail>
    {
        public void Configure(EntityTypeBuilder<Mail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Host).HasMaxLength(150).IsRequired();
            builder.Property(x => x.EmailAddress).HasMaxLength(150).IsRequired();
            builder.Property(x => x.EnableSsl).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Password).HasColumnType("TEXT");
            builder.Property(x => x.Port).HasDefaultValue(587);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Mails)
                .HasForeignKey(x => x.UserId);

        }
    }
}
