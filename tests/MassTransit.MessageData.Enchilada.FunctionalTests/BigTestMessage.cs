using System;

namespace MassTransit.MessageData.Enchilada.FunctionalTests
{
    public class BigTestMessage
    {
        public MessageData<byte[]> Blob { get; set; }
    }
}
