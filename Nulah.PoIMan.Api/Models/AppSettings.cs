namespace Nulah.PoIMan.Api.Models;

public class AppSettings
{
	public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
	public string Postgres { get; set; }
}