using System.ComponentModel.DataAnnotations;

namespace AutoSultan.Wpf.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Username { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    public string? DisplayName { get; set; }
}
