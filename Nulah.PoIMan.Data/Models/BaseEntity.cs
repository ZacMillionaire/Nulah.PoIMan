using System.ComponentModel.DataAnnotations;

namespace Nulah.PoIMan.Data.Models;

internal class BaseEntity
{
	[Key]
	public int Id { get; set; }

	//https://www.npgsql.org/efcore/modeling/concurrency.html?tabs=data-annotations
	[Timestamp]
	[ConcurrencyCheck]
	public uint Version { get; set; }

	public DateTimeOffset CreatedUtc { get; internal set; }
	public DateTimeOffset UpdatedUtc { get; internal set; }
}