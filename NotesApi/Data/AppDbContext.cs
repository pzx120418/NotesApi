using Microsoft.EntityFrameworkCore;
using NotesApi.Models;
using System.Collections.Generic;

namespace NotesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
    }
}
