using Microsoft.EntityFrameworkCore;
using Nulah.PoIMan.Data.Models;
using Nulah.PoIMan.Domain.Exceptions;
using Nulah.PoIMan.Domain.Interfaces;
using Nulah.PoIMan.Domain.Users;

namespace Nulah.PoIMan.Data;

public class UserRepository : IUserRepository
{
	private readonly PoIManDbContext _context;

	public UserRepository(PoIManDbContext context)
	{
		_context = context;
	}

	public async Task<PublicUser?> GetUserByToken(string token)
	{
		var existingUserByToken = await _context.Users.FirstOrDefaultAsync(x => x.Token == token);

		return existingUserByToken != null
			? new PublicUser()
			{
				Id = existingUserByToken.Id,
				Name = existingUserByToken.Name
			}
			: default;
	}

	public async Task<string> RegisterUser(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException(nameof(name));
		}

		var existingUserByName = await _context.Users.AnyAsync(x => x.Name == name);
		if (existingUserByName)
		{
			throw new UsernameInUseException(name);
		}

		var newUser = new User()
		{
			Name = name,
			Token = GenerateApiToken()
		};

		_context.Users.Add(newUser);

		await _context.SaveChangesAsync();

		return newUser.Token;
	}

	private string GenerateApiToken()
	{
		var g = Guid.NewGuid().ToByteArray();
		return $"poiman:{Convert.ToHexString(g[..8])}-{Convert.ToHexString(g[8..12])}-{Convert.ToHexString(g[12..16])}";
	}
}