using Cms.Contracts.Rights;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Rights;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Rights
{
    public class GroupGrain : Grain, IGroupGrain, IGroupManagerGrain
    {
        ILogger<GroupGrain> logger;
        IPersistentState<Group> persistent;

        public GroupGrain(ILogger<GroupGrain> logger, IPersistentState<Group> persistent)
        {
            this.logger = logger;
            this.persistent = persistent;
        }

        public override async Task OnActivateAsync()
        {
            if (!persistent.RecordExists)
            {
                persistent.State.Id = this.GetPrimaryKeyString();
            }

            await base.OnActivateAsync();
        }

        #region IGroupGrain
        public Task<string> GetId()
        {
            return Task.FromResult(persistent.State.Id);
        }

        public Task<string> GetName()
        {
            return Task.FromResult(persistent.State.Name);
        }

        public Task<UserRights> GetRights()
        {
            return Task.FromResult(persistent.State.Rights);
        }
        #endregion

        #region IGroupManagerGrain
        public Task UpdateName(string name)
        {
            logger.LogInformation($"Group:{persistent.State.Id} changed name from {persistent.State.Name} to {name}");
            persistent.State.Name = name;
            return persistent.WriteStateAsync();
        }

        public Task UpdateRights(UserRights rights)
        {
            logger.LogInformation($"Group:{persistent.State.Id} changed rights.");
            persistent.State.Rights = rights;
            return persistent.WriteStateAsync();
        }  
        #endregion
    }
}
