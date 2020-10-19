using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quests.Server.Models;
using Quests.Shared.Entities.Models;
using Quests.Shared.VM;

namespace Quests.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestStep> QuestSteps { get; set; }
        public DbSet<QuestCategory> QuestCategories { get; set; }
        public DbSet<MyQuest> MyQuests { get; set; }
        public DbSet<MyQuestStep> MyQuestSteps { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

    }
}
