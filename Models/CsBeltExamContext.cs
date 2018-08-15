using Microsoft.EntityFrameworkCore;
namespace CsBeltExam.Models
{
	public class CsBeltExamContext : DbContext
	{
	// base() calls the parent class' constructor passing the "options" parameter along
	public CsBeltExamContext(DbContextOptions<CsBeltExamContext> options) : base(options) { }
	// public DbSet<User> users {get;set;}
	}
}