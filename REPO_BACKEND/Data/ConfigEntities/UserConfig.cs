﻿using backnc.Data.POCOEntities;
using Microsoft.EntityFrameworkCore;

namespace backnc.Data.ConfigEntities
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.username).IsRequired().HasMaxLength(50);

            builder.Property(u => u.password).IsRequired();

            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId);
        }
    }
}
