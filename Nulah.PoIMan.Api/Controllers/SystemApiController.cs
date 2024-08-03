using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nulah.PoIMan.Domain.Exceptions;
using Nulah.PoIMan.Domain.Interfaces;
using Nulah.PoIMan.Domain.Users;

namespace Nulah.PoIMan.Api.Controllers;

[ApiController]
[Route("System")]
public class SystemApiController : ControllerBase
{
	private readonly IUserRepository _userRepository;

	public SystemApiController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("[action]")]
	public async Task<ActionResult<string>> Register([FromBody] string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return BadRequest("Name is required");
		}

		try
		{
			var generatedToken = await _userRepository.RegisterUser(name);

			return Ok(generatedToken);
		}
		catch (UsernameInUseException uiuex)
		{
			return BadRequest("Name already exists");
		}
	}
}