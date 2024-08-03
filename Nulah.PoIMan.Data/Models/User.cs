using Microsoft.EntityFrameworkCore;

namespace Nulah.PoIMan.Data.Models;

[Index(nameof(Token), IsUnique = true)]
internal class User : BaseEntity
{
	public string Name { get; set; } = null!;
	public string Token { get; set; } = null!;
}