using System.Text.Json.Serialization;
using Nulah.PoIMan.Domain.Enums;

namespace Nulah.PoIMan.Domain.Features;

/// <summary>
/// Primary return type for all features - enables future extended feature types
/// </summary>
public class FeatureBase
{
	public FeatureType Type { get; }
	public double Latitude { get; }
	public double Longitude { get; }
	public string Name { get; }

	public FeatureBase(FeatureType type, double latitude, double longitude, string name)
	{
		Type = type;
		Latitude = latitude;
		Longitude = longitude;
		Name = name;
	}
}