using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MassTransit.MessageData.Enchilada.FunctionalTests.Tests
{
    [TestFixture]
    public class SendingABigMessageTest
    {
        private readonly ConcurrentBag<BigTestMessage> _bigTestMessages = new ConcurrentBag<BigTestMessage>();
        private IBusControl _busControl;
        private byte[] _expectedBlob;
        private IMessageDataRepository _messageDataRepository;
        private BusHandle _busHandle;

        [OneTimeSetUp]
        public async Task GivenARunningBusThatIsListeningToABigTestMessageThatIsUsingEnchiladaMessageDataRepository()
        {
            _messageDataRepository = MessageDataRepositoryFactory.Create();
            _busControl = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("test-" + new Guid().ToString(), ep =>
                {
                    ep.Handler<BigTestMessage>((context) => {
                        _bigTestMessages.Add(context.Message);
                        return Task.FromResult(0);
                    });

                    ep.UseMessageData<BigTestMessage>(_messageDataRepository);
                });
            });

            _busHandle = await _busControl.StartAsync();
            _busHandle.Ready.Wait();
        }

        [SetUp]
        public async Task WhenPublishingABigTestMessage()
        {
            _expectedBlob = new byte[]
            {
                111, 2, 234, 23, 23, 234, 235
            };

            var bigTestMessage = new BigTestMessage
            {
                Blob = await _messageDataRepository.PutBytes(_expectedBlob)
            };

            await _busControl.Publish(bigTestMessage);
        }

        [Test]
        public async Task ThenBigTestMessageContainsData()
        {
            BigTestMessage actual = null;

            Assert.That(() => (actual = _bigTestMessages.SingleOrDefault()), Is.Not.Null.After(1000, 100));

            Assert.That(await actual.Blob.Value, Is.EqualTo(_expectedBlob));
        }

        [OneTimeTearDown]
        public async Task Kill()
        {
            await _busHandle.StopAsync();
        }
    }
}