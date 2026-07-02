using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AutoSultan.Wpf.Data;

#nullable disable

namespace AutoSultan.Wpf.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("AutoSultan.Wpf.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("DisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Username")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.ToTable("Users");
            });
        }
    }
}
