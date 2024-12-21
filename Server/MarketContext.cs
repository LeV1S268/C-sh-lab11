using Microsoft.EntityFrameworkCore;
using Models;
using SQLitePCL; // Обязательное подключение

namespace marketContext
{
    public class MarketContext : DbContext
    {
        public DbSet<Ticker> Tickers => Set<Ticker>();
        public DbSet<Price> Prices => Set<Price>();
        public DbSet<TodaysCondition> todaysConditions => Set<TodaysCondition>();

        public MarketContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SQLitePCL.Batteries.Init(); // Инициализация SQLitePCL
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\aleks\\OneDrive\\Рабочий стол\\Прога\\СЕМ 3\\lab10\\bin\\Debug\\net8.0\\MarketDB.db;"); // Путь к БД
        }
    }
}
