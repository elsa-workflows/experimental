using Elsa.Runtime.Contracts;
using Elsa.Runtime.ProtoActor.Actors;
using Elsa.Runtime.ProtoActor.HostedServices;
using Elsa.Runtime.ProtoActor.Services;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using Proto.Cluster;
using Proto.Cluster.Consul;
using Proto.Cluster.Partition;
using Proto.Cluster.PubSub;
using Proto.DependencyInjection;
using Proto.Remote;
using Proto.Remote.GrpcCore;
using Proto.Utils;

namespace Elsa.Runtime.ProtoActor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProtoActorWorkflowHost(this IServiceCollection services)
        {
            return services
                .AddHostedService<WorkflowServerHost>()
                .AddSingleton(sp =>
                {
                    var system = new ActorSystem()
                        .WithServiceProvider(sp)
                        .WithRemote(GetRemoteConfig())
                        //.WithCluster(GetClusterConfig())
                        ;

                    return system;
                })
                .AddSingleton<IStimulusDispatcher, ProtoActorStimulusDispatcher>()
                .AddSingleton<IWorkflowDefinitionDispatcher, ProtoActorWorkflowDefinitionDispatcher>()
                .AddTransient<WorkflowServerActor>()
                .AddTransient<WorkflowDefinitionActor>()
                .AddTransient<WorkflowInstanceActor>();
        }
        
        private static GrpcCoreRemoteConfig GetRemoteConfig() => GrpcCoreRemoteConfig
            .BindToLocalhost()
            .WithProtoMessages(Messages.MessagesReflection.Descriptor)
        ;
        
        private static ClusterConfig GetClusterConfig()
        {
            var clusterProvider = new ConsulProvider(new ConsulProviderConfig{});

            //use an empty store, no persistence
            var store = new EmptyKeyValueStore<Subscribers>();
            
            var clusterConfig =
                ClusterConfig
                    .Setup("MyCluster", clusterProvider, new PartitionIdentityLookup())
                    //.WithClusterKind("topic", Props.FromProducer(() => new TopicActor(store)))
                    //.WithPubSubBatchSize(2000)
                    ;
            return clusterConfig;
        }
    }
}