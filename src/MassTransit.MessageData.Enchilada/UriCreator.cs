using System;

namespace MassTransit.MessageData.Enchilada
{
    public class UriCreator : IUriCreator
    {
        private readonly Uri _baseUri;

        public UriCreator(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri Create(string name)
        {
            return new Uri(_baseUri, name);
        }
    }
}