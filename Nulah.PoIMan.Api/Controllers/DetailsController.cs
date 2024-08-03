using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nulah.PoIMan.Domain.Enums;

namespace Nulah.PoIMan.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class DetailsController : ControllerBase
{
	[HttpGet]
	[Route("[action]")]
	public ActionResult<List<string>> FeatureTypes()
	{
		return Ok(Enum.GetValues<FeatureType>());
	}
}