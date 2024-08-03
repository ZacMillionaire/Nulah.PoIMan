using Nulah.PoIMan.Domain.Enums;

namespace Nulah.PoIMan.Domain.Features;

public class Shop : FeatureBase
{
	public Shop(double latitude, double longitude, string name)
		: base(FeatureType.Shop, latitude, longitude, name)
	{
	}
}