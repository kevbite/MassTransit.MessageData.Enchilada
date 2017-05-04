using System;
using Enchilada.Infrastructure.Interface;

namespace MassTransit.MessageData.Enchilada
{
    public class EnchiladaMessageDataRepositoryFactory : IEnchiladaMessageDataRepositoryFactory
    {
        public EnchiladaMessageDataRepository Create(IEnchiladaFilesystemResolver resolver, Uri baseUri)
        {
            var enchiladaMessageDataRepository = new EnchiladaMessageDataRepository(resolver,
                new GuidFileNameCreator(), new UriCreator(baseUri));

            return enchiladaMessageDataRepository;
        }
    }
}
