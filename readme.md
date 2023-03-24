[![EO principles respected here](https://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)
[![Build status](https://ci.appveyor.com/api/projects/status/m8xpbtmd873o4vu9/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/yaapii-http/branch/master)

## Repo Guidelines

Mainly responsible for this repository is [DFU398](https://github.com/DFU398).
Please request a review in every single PR from him. 

He will try to review the PRs within **1 week** and merge applied PRs within **2 weeks** with a new release. Main review day is thursday.

# Yaapii.Http

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
    * [Yaapii.Http.Mock](#yaapii.http.mock)
        * [Mocking different Paths](#mocking-different-paths)
        * [HttpMock](#httpmock)
        * [HTTPS testing with HttpMock](#https-testing-with-httpmock)
5. [Strong naming and third party libraries](#strong-naming-and-third-party-libraries)

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
    new Body(new JObject().ToString(), "application/json")
)
// or
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
    new Body(new XMLCursor("<root></root>").AsNode().ToString(), "application/xml")
)
// or
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

For Deserialization, see [Extracting Data from a Response](#extracting-data-from-a-response)

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
response.Head(); // or: response.Body();
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
var status = new Status.Of(response); // implements INumber, see Yaapii.Atoms
var contentTypes = new ContentType.Of(response); // implements IEnumerable<string>
var contentType = new ContentType.FirstOf(response); // implements IText, see Yaapii.Atoms
var methods = new Header.Of("Allow"); // implements IEnumerable<string>
var method = new Header.FirstOf("Allow"); // implements IText, see Yaapii.Atoms
var body = new Body.Of(response); // implements IInput, see Yaapii.Atoms
```

If you need the body in a format other than ```IInput```, there are special body classes, that do the conversion for you:
* ```new TextBody.Of(response)``` does something similar to ```new TextOf(new Body.Of(response))```, but automatically selects the encoding based on the Content-Type header (default is UTF-8, if none is specified)
* ```new XmlBody.Of(reponse)``` does the same as ```new XMLCursor(new Body.Of(response))```
* ```new JsonBody.Of(reponse)``` does the same as ```new JSONOf(new Body.Of(response))```
* ```new BytesBody.Of(reponse)``` does the same as ```new BytesOf(new Body.Of(response))```

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

### Yaapii.Http.Mock

For versions up to 6.0.0, the following functionalities were included in the Yaapii.Http NuGet package. For newer versions, Yaapii.Http.Mock is released as its own NuGet package.

#### Mocking different Paths

```MatchingWire``` allows to mock the behavior of a real web server more closely by routing requests to different wires, depending on the content of the requests.
This is done using wire templates (classes implementing ```ITemplate```), that encapsulate a check for certain criteria that requests have to meet,
and a wire that should be used to respond to these requests. 
```MatchingWire``` finds the first template that applies to a request and asks it for a response to that request.
The simplest use for this is returning different responses for different paths:
```csharp
new MatchingWire( // implements IWire
    new Match( // implements ITemplate
        new Path("/regular-path"),
        new FkWire(200, "OK")
    ),
    new Match(
        "/regular-path",    // does the same as the first template, but is less verbose
        new FkWire(200, "OK")
    ),
    new Match(
        "/top-secret",
        new FkWire(403, "Forbidden")
    )
)
```
Current ```ITemplate``` implementations are:
* ```Match```, applies to a request, if the request matches all specified request parts, e.g. path, method, headers, query params, etc.
* ```Conditional``` applies to a request, if a given function returns true for the request.

See the ```HttpMock``` example code below for more examples of how to use these templates.

#### HttpMock
Should you need to test with actual http requests, a mock server is provided through ```HttpMock```.
It allows it to process incoming requests using an ```IWire```.
In versions up to 6.0.0, it used [jrharmon's MockHttpServer](https://github.com/jrharmon/MockHttpServer).
In newer versions, it uses an ASP.NET Kestrel server.
It's an ```IScalar``` (see Yaapii.Atoms) returning an ```IWebHost``` (see Microsoft.AspNetCore.Hosting). 
Calling ```HttpMock.Value()``` for the first time will initialize the server.

If an error occurs while processing a request, it responds with a status 500 and writes the exception and its stacktrace into the body of the response.

It can either use one wire to handle all requests, regardless of the requested path, or respond to specific paths with a different wire for each path.
Routing is done using the ```MatchingWire``` and templates described above.
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
        new Match("", 
            new FkWire(200, "OK") // will handle http://localhost:1337/
        ),
        new Match("bad/request",
            new FkWire(400, "Bad Request") // will handle http://localhost:1337/bad/request
        ),
        new Match("coffee",
            new Method("brew"), // see RFC 2324, not supported by AspNetCoreWire
            new FkWire(418, "I'm a teapot") // will handle http://localhost:1337/coffee, if the method is "BREW"
        ),
        new Match(
            new Parts.Joined(
                new Body("important data"),
                new Method("put")
            ),
            new FkWire(200, "OK") // will handle PUT requests with the specified body, if they didn't match an earlier template
        ),
        new Conditional(
            req => new LengthOf<string>(new BearerTokenAuth.Of(req)).Value() > 0,
            new FkWire(200, "OK")  // will handle requests that have any bearer token authorization header, if they didn't match an earlier template
        )
    ).Value()
)
{
    // ... testing code goes here
}
```

**Always dispose the ```HttpMock``` or the ```IWebHost``` it returned!**

No need to dispose both, disposing ```HttpMock``` will just dispose the ```IWebHost``` returned by ```HttpMock.Value()```.
This means calling ```HttpMock.Dispose()``` will do the same thing as calling ```HttpMock.Value().Dispose()```.
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
    // ... testing code goes here
} // this disposes the server
```

**Do not give an ```AspNetCoreWire``` to ```HttpMock```!**

```HttpMock``` is for receiving requests, not sending them. 
Using a wire that sends requests to handle the requests received by ```HttpMock``` would cause each received request to just be sent again, causing an infinite loop.

#### HTTPS testing with HttpMock

```HttpMock``` can optionally be configured to listen for HTTPS requests instead of HTTP requests:
```csharp
using (
    new HttpMock(1337,
        new FkWire(),
        useHttps: true
    ).Value()
)
{
    // ... testing code goes here
}
```

The HTTPS server will use the default developer certificate. If no default certificate is configured, an exception will be thrown.

.NET comes with a [command line tool to manage these certificates](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs).
To create and configure a new self-signed default certificate, run the following commands in powershell and click yes on the confirmation prompt:
```
dotnet dev-certs https
dotnet dev-certs https --trust
```

# Strong naming and third party libraries

This package uses [strong naming](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/strong-naming). There are some downsides to that, but the way we use this library, the benefit outweighs the cost.
Unfortunately, some third party libraries we are using (currently [StephenCleary/AsyncEx](https://github.com/StephenCleary/AsyncEx) and some of its dependencies) aren't strong named.
Therefore we have to build and strong-name the assemblies ourselves, instead of using NuGet Packages.
The code and the licenses of those libraries can be found in the "ThirdParty" folder.
