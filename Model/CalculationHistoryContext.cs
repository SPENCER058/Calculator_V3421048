using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Calculator_V3421048.Model
{
	internal class CalculationHistoryContext : DbContext
	{
		public DbSet<CalculationHistory> CalculationHistories { get; set; }

		public string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "calculationHistory.db");

		protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={path}");
	}
}
