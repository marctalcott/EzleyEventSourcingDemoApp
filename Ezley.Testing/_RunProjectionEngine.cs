using System.Threading.Tasks;
using Ezley.Events;
using Ezley.Projections;
using Ezley.ProjectionStore;
using Xunit;

namespace Ezley.Testing
{
    public class RunProjectionEngine
    {
        private TestConfig _testConfig = new TestConfig();
        
        [Fact]
        public async Task RegisterProjectionsAsync()
        {
            var eventTypeResolver = new EventTypeResolver();
            var viewRepo = new CosmosDBViewRepository(
                _testConfig.EndpointUri, 
                _testConfig.AuthKey,
                _testConfig.Database,
                _testConfig.ViewContainer);

            var projectionEngine = new CosmosDBProjectionEngine(eventTypeResolver,
                viewRepo,
                _testConfig.EndpointUri, _testConfig.AuthKey, _testConfig.Database,
                _testConfig.EventContainer, _testConfig.LeasesContainer);
            
            projectionEngine.RegisterProjection(new UserProjection());
            projectionEngine.RegisterProjection(new TenantProjection());
            projectionEngine.RegisterProjection(new ServiceSubscriberProjection());
            projectionEngine.RegisterProjection(new ActiveServiceSubscribersProjection());
            projectionEngine.RegisterProjection(new InactiveServiceSubscribersProjection());
            
            await projectionEngine.StartAsync("UnitTests");
            await Task.Delay(-1);
        }
    }
}