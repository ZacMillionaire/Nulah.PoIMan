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

	public async Task<FeatureBase> CreateFeature(FeatureBase featureBase, int userId)
	{
		var creatingUser = _context.Users.FirstOrDefault(x => x.Id == userId);
		if (creatingUser == null)
		{
			throw new UserNotFoundException(userId);
		}

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

		newFeature.CreatedBy = creatingUser;

		_context.Features.Add(newFeature);
		await _context.SaveChangesAsync();

		return ConvertFeatureToType(newFeature);
	}

	public async Task<List<FeatureBase>> GetFeatures()
	{
		var existingFeatures = await _context.Features.ToListAsync();

		return existingFeatures.Select(ConvertFeatureToType).ToList();
	}

	private FeatureBase ConvertFeatureToType(Feature feature)
	{
		FeatureBase featureDto;
		switch (feature.Type)
		{
			case FeatureType.Feature:
				featureDto = new FeatureBase(FeatureType.Feature, feature.Latitude, feature.Longitude, feature.Name);
				break;
			case FeatureType.Shop:
				featureDto = new Shop(feature.Latitude, feature.Longitude, feature.Name);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return featureDto;
	}
}

public class UserNotFoundException : Exception
{
	public UserNotFoundException(int userId)
		: base($"No user found with user Id: {userId}")
	{
	}
}