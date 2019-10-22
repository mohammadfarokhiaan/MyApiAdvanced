using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Post:BaseEntity<Guid>
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        public Category Category { get; set; }
        public User Author { get; set; }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Description).IsRequired();
            builder.HasOne(c => c.Category).WithMany(p => p.Posts).HasForeignKey(p => p.CategoryId);
            builder.HasOne(a => a.Author).WithMany(p => p.Posts).HasForeignKey(a => a.AuthorId);
        }
    }
}