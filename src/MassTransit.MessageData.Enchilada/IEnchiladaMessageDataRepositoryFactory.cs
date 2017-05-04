using System;
using Enchilada.Infrastructure.Interface;

namespace MassTransit.MessageData.Enchilada
{
    public interface IEnchiladaMessageDataRepositoryFactory
    {
        EnchiladaMessageDataRepository Create(IEnchiladaFilesystemResolver resolver, Uri baseUri);
    }
}