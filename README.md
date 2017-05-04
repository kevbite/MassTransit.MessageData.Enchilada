# MassTransit.MessageData.Enchilada
MassTransit with Enchilada - Yum!

This package allows you to send big message and not care about where the data is stored. Storing big payloads separate from you message bus messages stops your system getting clogged up.

[![install from nuget](http://img.shields.io/nuget/v/MassTransit.MessageData.Enchilada.svg?style=flat-square)](https://www.nuget.org/packages/MassTransit.MessageData.Enchilada)
[![downloads](http://img.shields.io/nuget/dt/MassTransit.MessageData.Enchilada.svg?style=flat-square)](https://www.nuget.org/packages/MassTransit.MessageData.Enchilada)


### Getting Started
`MassTransit.MessageData.Enchilada` can be installed via the package manager console by executing the following commandlet:

```powershell

PM> Install-Package MassTransit.MessageData.Enchilada

```

or by using the dotnet cli:
```bash

$ dotnet add package MassTransit.MessageData.Enchilada

```

Once we have the package installed, we can create a `EnchiladaMessageDataRepository` using the `EnchiladaMessageDataRepositoryFactory`:

```csharp

var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()) + "\\";

var adapter = new FilesystemAdapterConfiguration()
{
    AdapterName = "filesystem",
    Directory = directory
};

var messageDataRepository = new EnchiladaMessageDataRepositoryFactory()
                                    .Create(adapter);

```

Or we can support multiple adapters:

```csharp

var resolver = new EnchiladaFileProviderResolver(new EnchiladaConfiguration()
{
    Adapters = new[] {adapter}
});

var messageDataRepository = new EnchiladaMessageDataRepositoryFactory()
                            .Create(resolver, new Uri("enchilada://filesystem"));

```

#### Supporting other storage adapters

Enchilada supports many diffrent storage adapters, a few of these are _Azure blob storage_, _ftp_, _file system_. 

Check out main [Enchilada project](https://github.com/sparkeh9/Enchilada) on github on how to configure these. 

#### Sending a Big Message

Once we have our message data repository we can now send big messages. Given we have a `BigMessage` that has a  `BigPayload` property of a type of `MessageData<byte[]>`:

```csharp

public class BigMessage
{
    public string Name { get; set; }

    public int Age { get; set; }

    public MessageData<byte[]> BigPayload { get; set; }
}

```

When creating the message we need to call our message data repository that we've created above to put the big pay load into our configured storage, this will then passes back a `MessageData<byte[]>`:

```csharp
var bytes = new byte[] {3, 1, 0, 4, 5, 5, 8 };

var bigPayload = await repository.PutBytes(bytes);

var message = new BigMessage
{
    Name = "Bob",
    Age = 80,
    BigPayload =  bigPayload
};

```

We can then publish/send it like any other MassTransit message:

```csharp

busControl.Publish(message);

```

#### Receiving a Big Message

To receive a message with a big pay load we need to configure our endpoint to use the repository for a given message type:

```csharp

var busControl = MassTransit.Bus.Factory.CreateUsingInMemory(cfg =>
{
    cfg.ReceiveEndpoint("enchiladas_on_the_bus", ep =>
    {
        // Normal Receive Endpoint Config...

        ep.UseMessageData<BigMessage>(messageDataRepository);
    });
});

```

Then, with the magic wiring from MassTransit we can consume the message inside a consumer with the following:
```csharp

public class BigMessageConsumer : IConsumer<BigMessage>
{
    public async Task Consume(ConsumeContext<BigMessage> context)
    {
        var bigPayload = await context.Message.BigPayload.Value;

        // Do something with the big payload...
    }
}

```

## Cleaning up Expired Data

There is currently no way to automatically clean up expired data. The `EnchiladaMessageDataRepository` actually completly ignores any TTLs you pass it, so you might need to hire a sys admin to press <kbd>Ctrl</kbd>+<kbd>A</kbd>,<kbd>Shift</kbd>+<kbd>Del</kbd>, <kbd>Alt</kbd>+<kbd>Y</kbd> every week 😉.

## ToDo

* TTLs on payload

## Contribute

1. Fork
1. Hack!
1. Pull Request