# Yaapii.Http

[![EO principles respected here](https://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)
[![Build status](https://ci.appveyor.com/api/projects/status/m8xpbtmd873o4vu9/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/yaapii-http/branch/master)

Object oriented http client. C# Port of [Vatavuk's verano-http](https://github.com/Vatavuk/verano-http).

1. [Creating Requests](#creating-requests)
    * [Specifying a Request Method](#specifying-a-request-method)
    * [Specifying an Address](#specifying-an-address)
    * [Request Headers](#request-headers)
    * [Request Bodies](#request-bodies)
    * [Serialization and Deserialization](#serialization-and-deserialization)
2. [Sending Requests](#sending-requests)
    * [Setting up an AspNetCoreWire](#setting-up-an-aspnetcorewire)
    * [Communicating with a Wire](#communicating-with-a-wire)
3. [Handling Responses](#handling-responses)
    * [Extracting Data from a Response](#extracting-data-from-a-response)
    * [Response Verification](#response-verification)
4. [Unit Testing](#unit-testing)
    * [Fake Classes](#fake-classes)
    * [HttpMock](#httpmock)

## Creating Requests
Requests are created from separate parts like a method, an address to send it to, headers or a body. For Example:
```csharp
new Request(
    new Method("get"),
    new Address("https://example.com"),
    new Header("Accept", "text/html"),
    new Body("some request body")
)
```
The order of these parts is irrelevant.

### Specifying a Request Method
There is a request part that specifies a method:
```csharp
new Request(
    new Method("get"),
    new Address("https://example.com")
)
```
Alternatively, there are request classes that come with a predefined method:
```csharp
new Get(
    new Address("https://example.com")
)
```

### Specifying an Address
An address can either be specified as a whole, using the ```Address``` request part, or parts of a URI can be added separately:
```csharp
new Get(
    new Address("https://example.com/this/is/a/path?aQueryParam=someValue&anotherQueryParam=anotherValue#someFragment")
)
new Get(
    new Scheme("https"),
    new Host("example.com"),
    new Path("this/is/a/path"),
    new QueryParam("aQueryParam", "someValue"),
    new QueryParam("anotherQueryParam", "anotherValue"),    // query params can be added individually
    new QueryParams(                                        // or multiple at once
        "aQueryParam", "someValue",
        "anotherQueryParam", "anotherValue"
    ),
    new Fragment("someFragment")
)
```

These are the URI parts that can be added individually:
```
http://user@example.com:8080/this/is/a/path?key=value&key2=value2#someFragment
\__/   \__/ \_________/ \__/ \____________/ \___________________/ \__________/
  |     |        |       |          |                 |                |
Scheme User     Host    Port       Path          QueryParams        Fragment
```
Only scheme and host are required to form a functioning URI.

Request classes may have constructors that specifically take a URI (or its string representation), so you don't have to add it as a request part:
```csharp
new Get("https://example.com")
// is equivalent to:
new Get(
    new Address("https://example.com")
)
```

### Request Headers
Headers can be added individually or in bulk. To add several values to the same header, just reuse the same header key:
```csharp
new Get(
    new Header("User-Agent", "Yaapii.Http"),
    new Header("Accept", "text/html"),
    new Header("Accept", "text/plain")
)
new Get(
    new Headers(
        "User-Agent", "Yaapii.Http",
        "Accept", "text/html",
        "Accept", "text/plain"
    )
)
```

There are also header classes with a predefined header key:
```csharp
new Get(
    new Accept("text/html")
)
// is equivalent to
new Get(
    new Header("Accept", "text/html")
)
```

Some also create the header value for you:
```csharp
new Get(
    new BasicAuth("user", "password")
)
// is equivalent to
new Get(
    new Header("Authorization", "Basic dXNlcjpwYXNzd29yZA==") // where "dXNlcjpwYXNzd29yZA==" is "user:password" in base 64
)
```

### Request Bodies
You can add a body to a request using the appropriate request part:
```csharp
new Post(
    new Body("insert request body here")
)
```

Some body ctors also set a value for the 'Content-Type' header:
```csharp
new Post(
    new Body(new JObject())
)
// is equivalent to
new Post(
    new ContentType("application/json"),
    new Body(new JObject().ToString())
)
```
```csharp
new Post(
    new Body(new XMLCursor("<root></root>"))
)
// is equivalent to
new Post(
    new ContentType("application/xml"),
    new Body(new XMLCursor("<root></root>").AsNode().ToString())
)
```

You can also use specific body classes :
* ```TextBody``` will set the content type header to ```text/plain```,
* ```HtmlBody``` will set the content type header to ```text/html```,
* ```FormParam``` or ```FormParams``` will set the content type header to ```application/x-www-form-urlencoded```.

### Serialization and Deserialization
The body class allow the serialization of certain data formats into text. These include:
* ```Body(IXML xml)``` uses ```IXML``` (see Yaapii.Xml),
* ```Body(JToken json)``` uses ```JToken``` (see Newtonsoft.Json),
* ```Body(IJSON json)``` uses ```IJSON``` (see Yaapii.JSON),
* ```Body(IInput content)``` uses ```IInput``` (Yaapii.Atoms).

## Sending Requests
### Setting up an AspNetCoreWire
The ```AspNetCoreWire``` relies on a ```System.Net.Http.HttpClient``` to send http requests.
The ```HttpClient``` should be reused when possible, according to https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/.
For that reason, ```AspNetCoreClients``` exists to manage reuseable http clients.

To make sure they are reused, **only use one instance of ```AspNetCoreClients``` in your application** 
and pass it down to where it is needed via dependency injection.

For example, when setting up a ```Microsoft.AspNetCore.WebHost```, add an instance of ```AspNetCoreClients``` to dependency injection of the ```WebHost```:
```csharp
var host = WebHost.CreateDefaultBuilder();
host.ConfigureServices(svc =>
{
    svc.Add(
        new ServiceDescriptor(
            typeof(IAspHttpClients),
            (prov) => new AspNetCoreClients(),
            ServiceLifetime.Singleton
        )
    );
});
```

At the place where you want to send a request, retrieve the instance of ```AspNetCoreClients``` from dependency injection and pass it to the ctor of ```AspNetCoreWire```.
This way ```AspNetCoreWire``` will use an existing ```HttpClient``` whenever possible.

### Communicating with a Wire
You can create a ```Response``` that encapsulates an ```IWire``` and will send the request when it becomes necessary:
```csharp
var response =
    new Response(
        new AspNetCoreWire(
            new AspNetCoreClients() // should normally be passed down via dependency injection
        ),
        new Get("https://example.com")
    );
var status = new Status.Of(response);
// no request sent yet due to lazy initialization
var statusInt = status.AsInt(); // request sent
```

To send a request immediately, you can trigger the lazy initialization like this:
```csharp
var response =
    new Response(
        new AspNetCoreWire(
            new AspNetCoreClients() // should normally be passed down via dependency injection
        ),
        new Get("https://example.com")
    );
response.GetEnumerator();
```

Alternatively, you can ask a wire directly for a response:
```csharp
var response =
    new AspNetCoreWire(
        new AspNetCoreClients() // should normally be passed down via dependency injection
    ).Response(
        new Get("https://example.com")
    );
```
Calling ```IWire.Reponse(request)``` will send the request immediately and return the response.

There is a wire decorator to add extra parts to every request:
```csharp
new Refined(
    new AspNetCoreWire(
        new AspNetCoreClients() // should normally be passed down via dependency injection
    ),
    new Header("User-Agent", "Yaapii.Http") // will be added to every request
).Response(new Get("https://example.com"))
```

## Handling Responses
### Extracting Data from a Response
Most request part classes (bodies and headers) also have a ```*.Of``` sub class that can be used to extract information from a response.
Header classes will return ```IEnumerable<string>```, since a header can have multiple values.
For Example:
```csharp
var response =
    new Response(
        new AspNetCoreWire(
            new AspNetCoreClients() // should normally be passed down via dependency injection
        ),
        new Get("https://example.com")
    );
var status = new Status.Of(response) // implements INumber, see Yaapii.Http.AtomsTemp
var contentType = new ContentType.Of(response) // implements IEnumerable<string>
var server = new Header.Of("Server") // implements IEnumerable<string>
var body = new Body.Of(response) // implements IInput, see Yaapii.Atoms
```

### Response Verification
The ```IVerification``` interface allows to check if a response meets certain criteria. There is also a wire decorator, that will run a given verification for each response coming from the wire:
```csharp
new Verified(
    new AspNetCoreWire(
        new AspNetCoreClients() // should normally be passed down via dependency injection
    ),
    new Verification(
        res => new Status.Of(res).AsInt() == 200,
        res => new ApplicationException("something went wrong")
    )
).Response(new Get("https://example.com"))
```
There is a less verbose way to verify the status code:
```csharp
new Verified(
    new AspNetCoreWire(
        new AspNetCoreClients() // should normally be passed down via dependency injection
    ),
    new ExpectedStatus(200)
).Response(new Get("https://example.com"))
```

## Unit Testing
### Fake Classes
A lot of classes can be tested without sending actual http requests. A fake wire is available to pretend sending requests and receiving responses in unit tests:
```csharp
new FkWire(req =>
{
    // ... do something with the request and return a response
    return new Response.Of(200, "OK");
}).Response(
    // ... your request here
)
```

### HttpMock
Should you need to test with actual http requests, a mock server is provided through ```HttpMock```.
It encapsulates [jrharmon's MockHttpServer](https://github.com/jrharmon/MockHttpServer) in a way that allows it to process incoming requests using an ```IWire```.
It's an ```IScalar``` (see Yaapii.Http.AtomsTemp) returning a ```MockServer``` (see MockHttpServer). 
Calling ```HttpMock.Value()``` for the first time will initialize the server.
It can either use one wire to handle all requests, regardless of the requested path, or respond to specific paths with a different wire for each path.
```csharp
using( var server =
    new HttpMock(1337,
        new FkWire() // will handle all paths
    ).Value()
)
{
    // ... testing code goes here
}
using( var server =
    new HttpMock(1337,
        new Kvp.Of<IWire>(""
            new FkWire(200, "OK") // will handle http://localhost:1337/
        ),
        new Kvp.Of<IWire>("bad/request"
            new FkWire(400, "Bad Request") // will handle http://localhost:1337/bad/request
        ),
        new Kvp.Of<IWire>("coffee"
            new FkWire(418, "I'm a teapot") // will handle http://localhost:1337/coffee
        )
    ).Value()
)
{
    // ... testing code goes here
}
```

**Always dispose the ```HttpMock``` or the ```MockServer``` it returned!**

No need to dispose both, disposing ```HttpMock``` will just dispose the ```MockServer``` returned by ```HttpMock.Value()```.
This means calling ```HttpMock.Dispose()``` will do the same thing as calling ```HttpMock.Value().Dispose```.
If not disposed, it will keep running in the background, listening for requests, keeping the port occupied.
The easiest way to do this is to put either ```new HttpMock``` or ```HttpMock.Value()``` in a using block as follows:
```csharp
using( var httpMock =
    new HttpMock(1337,
        new FkWire()
    )
)
{
    var server = httpMock.Value(); // this would start the server
    // ... testing code goes here
} // this disposes the server

using( var server =
    new HttpMock(1337,
        new FkWire()
    ).Value() // start the server immediately
)
{
    var port = server.Port;
    // ... testing code goes here
} // this disposes the server
```

**Do not give an ```AspNetCoreWire``` to ```HttpMock```!**

```HttpMock``` is for receiving requests, not sending them. 
Using a wire that sends requests to handle the requests received by ```HttpMock``` would cause each received request to just be sent again, causing an infinite loop.
