using Nulah.PoIMan.Domain.Users;

namespace Nulah.PoIMan.Domain.Interfaces;

public interface IUserRepository
{
	Task<PublicUser?> GetUserByToken(string token);
	Task<string> RegisterUser(string name);
}