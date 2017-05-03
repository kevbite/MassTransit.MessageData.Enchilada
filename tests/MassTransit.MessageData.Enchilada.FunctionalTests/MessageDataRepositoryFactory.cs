using System;
using System.IO;
using Enchilada.Configuration;
using Enchilada.Filesystem;
using Enchilada.Infrastructure;

namespace MassTransit.MessageData.Enchilada.FunctionalTests
{
    public static class MessageDataRepositoryFactory
    {
        public static IMessageDataRepository Create()
        {
            var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + "\\";
            var enchiladaConfiguration = new EnchiladaConfiguration()
            {
                Adapters = new IEnchiladaAdapterConfiguration[]
                {
                    new FilesystemAdapterConfiguration()
                    {
                        AdapterName = "filesystem",
                        Directory = directory,
                    }
                }
            };

            var enchiladaFileProviderResolver = new EnchiladaFileProviderResolver(enchiladaConfiguration);

            var baseUri = new Uri("enchilada://filesystem");

            var enchiladaMessageDataRepository = new EnchiladaMessageDataRepository(enchiladaFileProviderResolver, new GuidFileNameCreator(), new UriCreator(baseUri) );

            return enchiladaMessageDataRepository;
        }
    }
}