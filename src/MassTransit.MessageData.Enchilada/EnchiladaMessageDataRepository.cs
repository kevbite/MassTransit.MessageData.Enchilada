using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Enchilada.Infrastructure.Interface;

namespace MassTransit.MessageData.Enchilada
{
    public class EnchiladaMessageDataRepository : IMessageDataRepository
    {
        private readonly IEnchiladaFilesystemResolver _enchiladaFilesystemResolver;
        private readonly IFileNameCreator _fileNameCreator;
        private readonly IUriCreator _uriCreator;

        public EnchiladaMessageDataRepository(IEnchiladaFilesystemResolver enchiladaFilesystemResolver, IFileNameCreator fileNameCreator, IUriCreator uriCreator)
        {
            _enchiladaFilesystemResolver = enchiladaFilesystemResolver;
            _fileNameCreator = fileNameCreator;
            _uriCreator = uriCreator;
        }

        public async Task<Stream> Get(Uri address, CancellationToken cancellationToken = new CancellationToken())
        {
            var reference = _enchiladaFilesystemResolver.OpenFileReference(address.AbsoluteUri);

            return await reference.OpenReadAsync()
                .ConfigureAwait(false);
        }

        public async Task<Uri> Put(Stream stream, TimeSpan? timeToLive = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var name = _fileNameCreator.Create();
            var address = _uriCreator.Create(name);

            var reference = _enchiladaFilesystemResolver.OpenFileReference(address.AbsoluteUri);

            using (var writeSteam = await reference.OpenWriteAsync()
                .ConfigureAwait(false))
            {
                await stream.CopyToAsync(writeSteam)
                    .ConfigureAwait(false);

                await stream.FlushAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            return address;
        }
    }
}
