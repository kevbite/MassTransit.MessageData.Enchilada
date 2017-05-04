using System;
using Enchilada.Configuration;
using Enchilada.Infrastructure;

namespace MassTransit.MessageData.Enchilada
{
    public static class EnchiladaMessageDataRepositoryFactoryExtentions
    {
        public static EnchiladaMessageDataRepository Create(this IEnchiladaMessageDataRepositoryFactory factory,
            IEnchiladaAdapterConfiguration enchiladaAdapterConfiguration)
        {
            var enchiladaConfiguration = new EnchiladaConfiguration()
            {
                Adapters = new[]
                {
                    enchiladaAdapterConfiguration
                }
            };

            var enchiladaFileProviderResolver = new EnchiladaFileProviderResolver(enchiladaConfiguration);

            var baseUri = new Uri($"enchilada://{enchiladaAdapterConfiguration.AdapterName}");

            return factory.Create(enchiladaFileProviderResolver, baseUri);
        }
    }
}