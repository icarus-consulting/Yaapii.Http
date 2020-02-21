# Yaapii.Http

[![Build status](https://ci.appveyor.com/api/projects/status/m8xpbtmd873o4vu9/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/yaapii-http/branch/master)

Object oriented http client. C# Port of [Vatavuk's verano-http](https://github.com/Vatavuk/verano-http).

1. [Creating Requests](#creating-requests)
    * [Specifying a Request Method](#specifying-a-request-method)
    * [Specifying an Address](#specifying-an-address)
    * [Request Headers](#request-headers)
    * [Request Bodies](#request-bodies)
    * [Serialization and Deserialization](#serialization-and-deserialization)
2. [Sending Requests](#sending-requests)
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

Some body classes also set a value for the 'Content-Type' header:
```csharp
new Post(
    new TextBody("insert request body here")
)
// is equivalent to
new Post(
    new ContentType("text/plain"),
    new Body("insert request body here")
)
```

Other examples of this are:
* ```HtmlBody``` will set the content type header to ```text/html```,
* ```XmlBody``` will set the content type header to ```application/xml```,
* ```JsonBody``` will set the content type header to ```application/json```,
* ```FormParam``` or ```FormParams``` will set the content type header to ```application/x-www-form-urlencoded```.

### Serialization and Deserialization
Some body classes allow the serialization/deserialization of certain data formats into/from text. These include:
* ```XmlBody``` / ```XmlBody.Of``` uses ```IXML``` (see Yaapii.Xml),
* ```JsonBody``` / ```JsonBody.Of``` uses ```IJSON``` (see Yaapii.Json),
* ```BytesBody``` / ```BytesBody.Of``` uses ```IBytes``` (Yaapii.Atoms). This allows the serialization/deserialization of any byte array into/from base 64 encoded text, so you can also transmit files this way.

## Sending Requests
Requests are sent over an ```IWire```.
You can create a ```Response``` that encapsulates an ```IWire``` and will send the request when it becomes necessary:
```csharp
var response =
    new Response(
        new AspNetCoreWire(),
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
        new AspNetCoreWire(),
        new Get("https://example.com")
    );
response.GetEnumerator();
```

Alternatively, you can ask a wire directly for a response:
```csharp
var response =
    new AspNetCoreWire().Response(
        new Get("https://example.com")
    );
```
Calling ```IWire.Reponse(request)``` will send the request immediately and return the response.

There is a wire decorator to add extra parts to every request:
```csharp
new Refined(
    new AspNetCoreWire(),
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
        new AspNetCoreWire(),
        new Get("https://example.com")
    );
var status = new Status.Of(response) // implements INumber, see Yaapii.Atoms
var contentType = new ContentType.Of(response) // implements IEnumerable<string>
var server = new Header.Of("Server") // implements IEnumerable<string>
var body = new Body.Of(response) // implements IText, see Yaapii.Atoms
var json = new JsonBody.Of(response) // implements IJSON, see Yaapii.Json. Fails if the body can not be parsed as json.
var xml = new XmlBody.Of(response) // implements IXML, see Yaapii.Xml. Fails if the body can not be parsed as xml.
```

### Response Verification
The ```IVerification``` interface allows to check if a response meets certain criteria. There is also a wire decorator, that will run a given verification for each response coming from the wire:
```csharp
new Verified(
    new AspNetCoreWire(),
    new Verification(
        res => new Status.Of(res).AsInt() == 200,
        res => new ApplicationException("something went wrong")
    )
).Response(new Get("https://example.com"))
```
There is a less verbose way to verify the status code:
```csharp
new Verified(
    new AspNetCoreWire(),
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
It's an ```IScalar``` (see Yaapii.Atoms) returning a ```MockServer``` (see MockHttpServer). 
Calling ```HttpMock.Value()``` for the first time will initialize the server.
It can either use one wire to handle all requests, regardless of the requested path, or respond to specific paths with a different wire for each path.
```csharp
using( var server =
    new HttpMock(
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
If the port is set to 0 or no port is specified (0 is the default in that case), [jrharmon's MockHttpServer](https://github.com/jrharmon/MockHttpServer) will find a random unused port.
This way you can ensure the port isn't already in use by something else, for example when running tests in parallel. It may trigger a firewall warning and may require admin rights, as described [here](https://github.com/jrharmon/MockHttpServer#usage).
You can get the port via ```HttpMock.Value().Port```.

**Always dispose the ```HttpMock``` or the ```MockServer``` it returned!**

No need to dispose both, disposing ```HttpMock``` will just dispose the ```MockServer``` returned by ```HttpMock.Value()```.
This means calling ```HttpMock.Dispose()``` will do the same thing as calling ```HttpMock.Value().Dispose```.
If not disposed, it will keep running in the background, listening for requests, keeping the port occupied.
The easiest way to do this is to put either ```new HttpMock``` or ```HttpMock.Value()``` in a using block as follows:
```csharp
using( var server =
    new HttpMock(
        new FkWire()
    )
)
{
    var port = server.Value().Port; // this would start the server
    // ... testing code goes here
} // this disposes the server

using( var server =
    new HttpMock(
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
