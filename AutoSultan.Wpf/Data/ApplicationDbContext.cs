using Microsoft.EntityFrameworkCore;
using AutoSultan.Wpf.Models;

namespace AutoSultan.Wpf.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}
