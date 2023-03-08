using EFCore.BulkExtensions;
using EFCore.BulkExtensions.OwnedTypeTest;
using Microsoft.EntityFrameworkCore;

Console.WriteLine($"[{DateTime.Now}] Starting application");

using var db = new DatabaseContext();

Console.WriteLine($"[{DateTime.Now}] Running EF Migrations");

db.Database.Migrate();

Console.WriteLine($"[{DateTime.Now}] Deleting existing entities");

var existingFamilies = db.Contacts.ToList();
db.BulkDelete(existingFamilies);

Console.WriteLine($"[{DateTime.Now}] Existing entities deleted");

Console.WriteLine($"[{DateTime.Now}] Creating new entities");

var people = new List<Person>();

for (var i = 1; i <= 100; i++)
{
    var person = new Person
    {
        FirstName = "John",
        LastName =  "Smith",
        PhysicalAddress = new Address
        {
            StreetAddress = $"301 S Main St",
            CityStateZip = $"Lillington, NE 27546"
        },
        MailingAddress = new Address
        {
            StreetAddress = $"1239 9th Ave",
            CityStateZip = $"San Francisco, CA 94122"
        },
    };

    people.Add(person);
}

db.BulkInsert(people);

var businesses = new List<Business>();

for (var i = 1; i <= 100; i++)
{
    var business = new Business
    {
        BusinessName = "Acme Corporation",
        PhysicalAddress = new Address
        {
            StreetAddress = $"1940 Lancaster Dr",
            CityStateZip = $"Salem, OR 97301"
        },
        MailingAddress = new Address
        {
            StreetAddress = $"5601 Brodie Ln #940",
            CityStateZip = $"Austin, TX 78745"
        },
    };

    businesses.Add(business);
}

db.BulkInsert(businesses);

Console.WriteLine($"[{DateTime.Now}] Finished");