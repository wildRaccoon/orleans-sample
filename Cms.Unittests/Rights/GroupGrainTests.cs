using System.Threading.Tasks;
using Cms.Contracts.Rights;
using Cms.Core.Rights;
using Cms.Orls.Core.Rights;
using FluentAssertions;
using Orleans.TestKit;
using Xunit;

namespace Cms.Unittests.Rights
{
    public class GroupGrainTests : TestKitBase
    {
        [Fact]
        public async Task NewGroup()
        {
            var groupState = new Group();

            Silo.SetupState<Group>(groupState, false);

            var group = await Silo.CreateGrainAsync<GroupGrain>("gid");

            await group.UpdateName("name");
            await group.UpdateRights(UserRights.MaxValue);

            var id = await group.GetId();
            id.Should().Equals("gid");

            var name = await group.GetName();
            name.Should().Equals("name");

            var rights = await group.GetRights();
            rights.Should().Equals(UserRights.MaxValue);
        }

        [Fact]
        public async Task UpdateGroup()
        {
            var groupState = new Group()
            { 
                Id = "gid",
                Name = "old name",
                Rights = UserRights.MinValue
            };

            Silo.SetupState<Group>(groupState, true);

            var group = await Silo.CreateGrainAsync<GroupGrain>("gid");

            await group.UpdateName("new name");
            await group.UpdateRights(UserRights.MaxValue);

            var id = await group.GetId();
            id.Should().Equals("gid");

            var name = await group.GetName();
            name.Should().Equals("new name");

            var rights = await group.GetRights();
            rights.Should().Equals(UserRights.MaxValue);
        }

    }
}
