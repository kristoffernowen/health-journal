using HealthJournal.Api.Domain.Journal;

namespace HealthJournal.Api
{
    public static class FakeUserProvider 
    {
        public static JournalUser LoggedInDummy()
        {
            return new JournalUser
            {
                Id = Guid.Parse("6c23bc95-9c5f-4ff6-888c-3b2eccf766f2"),
                ExtUserId = "dummy-user"
            };
        }
    }
}
