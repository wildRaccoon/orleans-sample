using Cms.Contracts.Rights;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Rights;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Rights
{
    public class GroupGrain : Grain<Group>, IGroupGrain, IGroupManagerGrain
    {
        ILogger<GroupGrain> logger;

        public GroupGrain(ILogger<GroupGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        #region IGroupGrain
        public Task<string> GetId()
        {
            return Task.FromResult(State.Id);
        }

        public Task<string> GetName()
        {
            return Task.FromResult(State.Name);
        }

        public Task<UserRights> GetRights()
        {
            return Task.FromResult(State.Rights);
        }
        #endregion

        #region IGroupManagerGrain
        public Task UpdateName(string name)
        {
            logger.LogInformation($"Group:{State.Id} changed name from {State.Name} to {name}");
            State.Name = name;
            return WriteStateAsync();
        }

        public Task UpdateRights(UserRights rights)
        {
            logger.LogInformation($"Group:{State.Id} changed rights.");
            State.Rights = rights;
            return WriteStateAsync();
        }  
        #endregion
    }
}
