using System;

namespace MassTransit.MessageData.Enchilada
{
    public class GuidFileNameCreator : IFileNameCreator
    {
        public string Create()
        {
            return Guid.NewGuid().ToString();
        }
    }
}