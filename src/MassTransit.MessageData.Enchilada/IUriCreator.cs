using System;

namespace MassTransit.MessageData.Enchilada
{
    public interface IUriCreator
    {
        Uri Create(string name);
    }
}