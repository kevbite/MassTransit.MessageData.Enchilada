# MassTransit.MessageData.Enchilada
MassTransit with Enchilada - Yum!

This package allows you to send big messages and not care about where the data is stored. Storing big payloads separate from your message bus messages stops your system from getting clogged up.

[![install from nuget](http://img.shields.io/nuget/v/MassTransit.MessageData.Enchilada.svg?style=flat-square)](https://www.nuget.org/packages/MassTransit.MessageData.Enchilada)
[![downloads](http://img.shields.io/nuget/dt/MassTransit.MessageData.Enchilada.svg?style=flat-square)](https://www.nuget.org/packages/MassTransit.MessageData.Enchilada)


### Getting Started
`MassTransit.MessageData.Enchilada` can be installed via the package manager console by executing the following commandlet:

```powershell

PM> Install-Package MassTransit.MessageData.Enchilada

```

or by using the dotnet CLI:
```bash

$ dotnet add package MassTransit.MessageData.Enchilada

```

Once the package is installed, create an `EnchiladaMessageDataRepository` using the `EnchiladaMessageDataRepositoryFactory`:

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

Support for multiple adapters is also available:

```csharp

var resolver = new EnchiladaFileProviderResolver(new EnchiladaConfiguration()
{
    Adapters = new[] {adapter}
});

var messageDataRepository = new EnchiladaMessageDataRepositoryFactory()
                            .Create(resolver, new Uri("enchilada://filesystem"));

```

#### Supporting other storage adapters

Enchilada supports many diffrent storage adapters, including _Azure blob storage_, _FTP_ and _file system_. 

Check out the main [Enchilada project](https://github.com/sparkeh9/Enchilada) on github on how to configure these. 

#### Sending a Big Message

Once the message data repository is created, big messages can be easily sent. A `BigMessage` has a  `BigPayload` property this is of type `MessageData<byte[]>`:

```csharp

public class BigMessage
{
    public string Name { get; set; }

    public int Age { get; set; }

    public MessageData<byte[]> BigPayload { get; set; }
}

```

When creating a message, the message data repository that we've created above needs to be called in order to put the big payload into our configured storage, that will then pass back a `MessageData<byte[]>`:

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

It can then be published/sent like any other MassTransit message:

```csharp

busControl.Publish(message);

```

#### Receiving a Big Message

To receive a message with a big payload the endpoint needs to be configure to use the repository for a given message type:

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

Then, with the magic wiring from MassTransit the message can be consumed inside a consumer with the following:
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

There is currently no way to automatically clean up expired data. The `EnchiladaMessageDataRepository` actually completely ignores any TTLs passed to it, so you might need to hire a sys admin to press <kbd>Ctrl</kbd>+<kbd>A</kbd>,<kbd>Shift</kbd>+<kbd>Del</kbd>, <kbd>Alt</kbd>+<kbd>Y</kbd> every week 😉.

## ToDo

* TTLs on payload

## Contribute

1. Fork
1. Hack!
1. Pull Request