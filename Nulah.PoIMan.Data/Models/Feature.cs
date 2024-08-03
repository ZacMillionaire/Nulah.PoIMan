using Nulah.PoIMan.Domain.Enums;

namespace Nulah.PoIMan.Data.Models;

internal class Feature : BaseEntity
{
	public FeatureType Type { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string Name { get; set; }
	public User CreatedBy { get; set; }
	public List<Tag> Tags { get; set; } = new();
}