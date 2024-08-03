using Nulah.PoIMan.Domain.Enums;

namespace Nulah.PoIMan.Domain.Features;

public class Cafe : FeatureBase
{
	public Cafe(double latitude, double longitude, string name) : base(FeatureType.Cafe, latitude, longitude, name)
	{
	}
}