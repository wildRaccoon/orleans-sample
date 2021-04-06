using Moq;
using Orleans.Runtime;
using Orleans.TestKit;

namespace Cms.Unittests
{
    public static class TestKitSiloExtension
    {
        public static void SetupState<T>(this TestKitSilo Silo,T state, bool exists = true)
        {
            var mockState = new Mock<IPersistentState<T>>(MockBehavior.Loose);
            mockState.SetupGet(o => o.State).Returns(state);
            mockState.SetupGet(o => o.RecordExists).Returns(exists);

            Silo.AddService(mockState.Object);
        }
    }
}
