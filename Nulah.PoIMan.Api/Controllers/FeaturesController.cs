using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nulah.PoIMan.Api.Middleware;
using Nulah.PoIMan.Domain.Features;
using Nulah.PoIMan.Domain.Interfaces;

namespace Nulah.PoIMan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FeaturesController : ControllerBase
{
	private readonly IFeatureRepository _featureRepository;

	public FeaturesController(IFeatureRepository featureRepository)
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

	[Authorize(ApiKeyAuthenticationOptions.PolicyName)]
	[HttpPost]
	[Route("Create")]
	public async Task<ActionResult<FeatureBase>> CreateFeature([FromBody] FeatureBase featureBase)
	{
		var userId = HttpContext.User.Claims
			.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
			?.Value;

		// If we've reached this point then technically this shouldn't be possible, however I also trust nothing
		if (string.IsNullOrWhiteSpace(userId))
		{
			return BadRequest("Invalid User");
		}

		return Ok(await _featureRepository.CreateFeature(featureBase, int.Parse(userId)));
	}
}