using Nulah.PoIMan.Domain.Features;

namespace Nulah.PoIMan.Domain.Interfaces;

public interface IFeatureRepository
{
	public Task<List<FeatureBase>> GetFeatures();

	public Task<FeatureBase> CreateFeature(FeatureBase featureBase, int userId);
}