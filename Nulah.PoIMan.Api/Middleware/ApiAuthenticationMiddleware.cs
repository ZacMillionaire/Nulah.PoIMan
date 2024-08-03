using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Nulah.PoIMan.Data;
using Nulah.PoIMan.Domain.Interfaces;

namespace Nulah.PoIMan.Api.Middleware;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
	public const string PolicyName = "ApiTokenBearer";
	public string ApiKey { get; set; }
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
	private readonly IUserRepository _userRepository;
	private string? _failReason;

	public ApiKeyAuthenticationHandler(
		IOptionsMonitor<ApiKeyAuthenticationOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		IUserRepository userRepository)
		: base(options, logger, encoder)
	{
		_userRepository = userRepository;
	}

	public const string AuthenticationHandlerName = "ApiTokenBearer";

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		// Read token from HTTP request header
		string authorizationHeader = Request.Headers["Authorization"]!;
		if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
		{
			_failReason = "No API Token";
			return AuthenticateResult.Fail(_failReason);
		}

		// Remove "Bearer" to get pure token data
		var token = authorizationHeader["Bearer ".Length..];
		if (string.IsNullOrWhiteSpace(token))
		{
			_failReason = "No API Token";
			return AuthenticateResult.Fail(_failReason);
		}

		var existingUser = await _userRepository.GetUserByToken(token);
		if (existingUser == null)
		{
			_failReason = "Invalid API Token";
			return AuthenticateResult.Fail(_failReason);
		}

		try
		{
			var claims = new[] { new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()) };

			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);
			return AuthenticateResult.Success(ticket);
		}
		catch (Exception ex)
		{
			//oops
			return AuthenticateResult.Fail(ex);
		}
	}


	protected override Task HandleChallengeAsync(AuthenticationProperties properties)
	{
		Response.StatusCode = 401;

		if (_failReason != null)
		{
			Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = _failReason;
		}

		return Task.CompletedTask;
	}
}