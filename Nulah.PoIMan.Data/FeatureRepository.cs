using Microsoft.EntityFrameworkCore;
using Nulah.PoIMan.Data.Models;
using Nulah.PoIMan.Domain.Enums;
using Nulah.PoIMan.Domain.Features;
using Nulah.PoIMan.Domain.Interfaces;

namespace Nulah.PoIMan.Data;

public class FeatureRepository : IFeatureRepository
{
	private readonly PoIManDbContext _context;

	public FeatureRepository(PoIManDbContext context)
	{
		_context = context;
	}

	public async Task<FeatureBase> CreateFeature(FeatureBase featureBase)
	{
		Feature newFeature;
		switch (featureBase.Type)
		{
			case FeatureType.Feature:
				newFeature = new Feature()
				{
					Latitude = featureBase.Latitude,
					Longitude = featureBase.Longitude,
					Name = featureBase.Name,
					Type = FeatureType.Feature
				};
				break;
			case FeatureType.Shop:
				newFeature = new ShopFeature()
				{
					Latitude = featureBase.Latitude,
					Longitude = featureBase.Longitude,
					Name = featureBase.Name,
					Type = FeatureType.Shop
				};
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		_context.Features.Add(newFeature);
		await _context.SaveChangesAsync();

		return new Shop(newFeature.Latitude, newFeature.Longitude, newFeature.Name)
		{
		};
	}

	public async Task<List<FeatureBase>> GetFeatures()
	{
		var existingFeatures = await _context.Features.ToListAsync();

		return new List<FeatureBase>()
		{
			new Shop(-27.47473, 153.02723, "test"),
			new Shop(-27.47399, 153.02654, "test 2")
		};
	}
}