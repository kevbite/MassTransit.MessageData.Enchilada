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

            var adapter = new FilesystemAdapterConfiguration()
            {
                AdapterName = "filesystem",
                Directory = directory
            };

            var resolver = new EnchiladaFileProviderResolver(new EnchiladaConfiguration()
            {
                Adapters = new[] {adapter}
            });

            var enchiladaMessageDataRepository = new EnchiladaMessageDataRepositoryFactory()
                                                        .Create(resolver, new Uri("enchilada://filesystem"));

            return enchiladaMessageDataRepository;
        }
    }
}