﻿using System.IO;

namespace MassTransit.MessageData.Enchilada
{
    public interface IFileNameCreator
    {
        string Create();
    }
}