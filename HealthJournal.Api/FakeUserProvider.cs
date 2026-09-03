using HealthJournal.Api.Models;

namespace HealthJournal.Api
{
    public static class FakeUserProvider 
    {
        public static User LoggedInDummy()
        {
            return new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                ExtUserId = "dummy-user"
            };
        }
    }
}
