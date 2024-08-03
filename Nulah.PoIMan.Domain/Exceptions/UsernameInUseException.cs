namespace Nulah.PoIMan.Domain.Exceptions;

public class UsernameInUseException : Exception
{
	public UsernameInUseException(string username) : base($"A user already exists with the name: {username}")
	{
	}
}