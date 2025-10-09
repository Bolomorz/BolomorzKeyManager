using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BolomorzKeyManager.Model;

public class UserAccount
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int UAID { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? HashParameter { get; set; }

    public List<Key> Keys { get; set; } = [];
    public List<Password> Passwords { get; set; } = [];
}

public class Key
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int KID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[]? EncryptedData { get; set; }

    [ForeignKey("Owner")] public int UAID { get; set; }
    public UserAccount? Owner { get; set; }
}

public class Password
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int PID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[]? EncryptedData { get; set; }

    [ForeignKey("Owner")] public int UAID { get; set; }
    public UserAccount? Owner { get; set; }
}