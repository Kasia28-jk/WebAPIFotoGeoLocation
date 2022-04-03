using FotoGeoLocationWebApplication.Entities;
using System.Linq;

namespace FotoGeoLocationWebApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            // Look for any uzytkownicy
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
         
            var users = new User[]
            {
            new User{UserName="Admin", Password = "60n74Xyh5DUYKXww+mpB9W6YhvZU4rAbeOtsnHTaIkM=", Enabled = true },
            new User{UserName="user1", Password = "wMCsNB7cDow48icLmxJp+wToMvuXsE+vJTNDho/VyPo=",Enabled = false},
            };

            foreach (var u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

        }
    }
}
