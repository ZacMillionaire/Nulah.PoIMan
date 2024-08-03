namespace Nulah.PoIMan.Data.Models;

internal class Tag : BaseEntity
{
	public string Name { get; set; }
	public int Weight { get; set; }
	public User User { get; set; }
}