using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Nulah.PoIMan.Data.Models;

[Index(nameof(Token), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
internal class User : BaseEntity
{
	[MaxLength(150)]
	public string Name { get; set; } = null!;

	[MaxLength(60)]
	public string Token { get; set; } = null!;
}