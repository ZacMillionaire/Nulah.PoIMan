using Microsoft.EntityFrameworkCore;
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
}