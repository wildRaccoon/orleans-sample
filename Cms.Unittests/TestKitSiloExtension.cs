using Moq;
using Orleans.Runtime;
using Orleans.TestKit;
using System.Reflection;

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

            var mockMapper = new Mock<IAttributeToFactoryMapper<PersistentStateAttribute>>();
            mockMapper.Setup(o => o.GetFactory(It.IsAny<ParameterInfo>(), It.IsAny<PersistentStateAttribute>())).Returns(context => mockState.Object);

            Silo.AddService(mockMapper.Object);
        }
    }
}
