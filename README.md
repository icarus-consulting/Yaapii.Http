# Yaapii.Http

[![Build status](https://ci.appveyor.com/api/projects/status/m8xpbtmd873o4vu9/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/yaapii-http/branch/master)

Useful classes for working with HTTP Requests. Partitial C# port from [jcabi-http](https://github.com/jcabi/jcabi-http) by [jcabi](http://www.jcabi.com).
# Request Example's
## GET Request
```csharp
var res =
    new HttpRequest(
        new Yaapii.Http.Method.Get(),
        new Uri("http://fancy-domain.io/what-ever"),
        new System.TimeSpan(0, 0, 5)
    ).Response();

if(res.Status() == 200) {
    // read json string from response body
    var json = new TextOf(res.Body());
}
```

## POST Request
```csharp
var res =
        new HttpRequest(
                new Yaapii.Http.Method.Post(),
                new Uri("http://localhost:5001/entity/rename"),
                new Dictionary<string,string>(),
                new BytesOf("{'name': 'foobar'}"),
                new TimeSpan(0, 0, 5)
            )
            .Header("Content-Type","application/json")
            .Response();

if(res.Status() == 200)
{
    // read json string from response body
    var json = new TextOf(res.Body());
}
```