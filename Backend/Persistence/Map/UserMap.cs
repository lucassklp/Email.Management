using Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Email.Management.Persistence.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Email).HasMaxLength(30);
            builder.Property(x => x.Password).HasMaxLength(130);
            builder.Property(x => x.Token);

            builder.HasIndex(x => new { x.Email, x.Password });
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
