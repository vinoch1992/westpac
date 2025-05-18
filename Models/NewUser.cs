namespace westpac.Models;

public class NewUser
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public string Language { get; set; } = "en_US";
}