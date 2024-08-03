using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nulah.PoIMan.Api.Middleware;
using Nulah.PoIMan.Domain.Features;
using Nulah.PoIMan.Domain.Interfaces;

namespace Nulah.PoIMan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureController : ControllerBase
{
	private readonly IFeatureRepository _featureRepository;

	public FeatureController(IFeatureRepository featureRepository)
	{
		_featureRepository = featureRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("List")]
	public async Task<ActionResult<List<FeatureBase>>> ListFeatures()
	{
		return Ok(await _featureRepository.GetFeatures());
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("Create")]
	public async Task<ActionResult<FeatureBase>> CreateFeature([FromBody] FeatureBase featureBase)
	{
		return Ok(await _featureRepository.CreateFeature(featureBase));
	}

	[Authorize(ApiKeyAuthenticationOptions.PolicyName)]
	[HttpGet]
	[Route("ListProtected")]
	public async Task<ActionResult<List<FeatureBase>>> ListFeaturesProtected()
	{
		return Ok(await _featureRepository.GetFeatures());
	}
}