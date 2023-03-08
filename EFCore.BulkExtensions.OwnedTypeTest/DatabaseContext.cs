namespace EFCore.BulkExtensions.OwnedTypeTest
{
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Business> Businesses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OwnedTypeTest;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureContactEntity(modelBuilder);
            ConfigurePersonEntity(modelBuilder);
            ConfigureBusinessEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureContactEntity(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Contact>();

            builder.HasKey(p => p.Id);

            builder.OwnsOne(a => a.PhysicalAddress, a =>
            {
                a.Property(s => s.StreetAddress).HasMaxLength(50);
                a.Property(s => s.CityStateZip).HasMaxLength(100);
            });

            builder.OwnsOne(a => a.MailingAddress, a =>
            {
                a.Property(s => s.StreetAddress).HasMaxLength(50);
                a.Property(s => s.CityStateZip).HasMaxLength(100);
            });

            builder.HasDiscriminator<string>("Type")
                .HasValue<Business>("Business")
                .HasValue<Person>("Person");
        }

        private void ConfigurePersonEntity(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Person>();
            builder.Property(c => c.FirstName).HasMaxLength(50);
            builder.Property(c => c.LastName).HasMaxLength(50);
        }

        private void ConfigureBusinessEntity(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Business>();
            builder.Property(i => i.BusinessName).HasMaxLength(100);
        }
    }

    public abstract class Contact
    {
        public int Id { get; set; }
        public Address PhysicalAddress { get; set; }
        public Address MailingAddress { get; set; }

    }

    public class Person : Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Business : Contact
    {
        public string BusinessName { get; set; }
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string CityStateZip { get; set; }
    }
}