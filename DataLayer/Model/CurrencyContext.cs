using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Model
{
    public partial class CurrencyContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CurrencyContext()
        {
            if (_configuration is null)
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public CurrencyContext(DbContextOptions<CurrencyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExchangeRateHistory> ExchangeRateHistories { get; set; } = null!;
        public virtual DbSet<ExchangeRateHistoryDetail> ExchangeRateHistoryDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conn = _configuration.GetConnectionString("ConnectionString_Currency");
                optionsBuilder.UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRateHistory>(entity =>
            {
                entity.ToTable("ExchangeRateHistory");

                entity.Property(e => e.BaseCurrency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DownloadTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.DownloadedByUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExchangeRateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExchangeRateHistoryDetail>(entity =>
            {
                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExchangeRate).HasColumnType("money");

                entity.HasOne(d => d.ExchangeRateHistory)
                    .WithMany(p => p.ExchangeRateHistoryDetails)
                    .HasForeignKey(d => d.ExchangeRateHistoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExchangeR__Excha__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
