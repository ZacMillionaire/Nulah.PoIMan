namespace Nulah.PoIMan.Domain.Exceptions;

public class UserNotFoundException : Exception
{
	public UserNotFoundException(int userId)
		: base($"No user found with user Id: {userId}")
	{
	}
}