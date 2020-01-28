# Distancify.SerilogExtensions

This project contains a couple of extensions and helpers that are commonly used among all our projects using Serilog.

## How to use

### Install

```
Install-Package Distancify.SerilogExtensions
```

### Profiling Example

This project contains a simple profiler that will include the time elapsed between log messages.

Use it like this:

```csharp
var logger = this.Log().Profile("My Profiling");
var r = new Random();

for (int i = 0; i < 10; i++)
{
    Thread.Sleep(r.Next(1000));
    logger.Information("Example message " + i);
}
```

Then, if you're using [Seq](https://getseq.net), you can simply query the elapsed timings based on your profiler name:

```
select percentile(ElapsedMs, 95) as ElapsedMs from stream where Profiler = 'My Profiling' group by @Message order by ElapsedMs desc
```

And it will show up like this:

![](profiling-example.png)

### Logging incidents

At Distancify, some log messages should trigger alarms to our on-call staff. While this can be configured in the log server, it's
often easier to make sure incidents are created from the code.

In order to standardize how we log incidents, there's an extension called `ForIncident` which is used like this:

```csharp
// Simple incident:
this.Log().ForIncident(IncidentPriority.P2).Error("Something went wrong")

// Incident around a specific entity (in this case an order), for collecting all messages around the same incident:
this.Log().ForIncident(IncidentPriority.P2, incidentId: order.Id).Error("Something went wrong")

// Sometimes, it could be good to group different types of log messages as the same type of incident, for example for reference to a standard operating proceedure. The following two messages belong to the same incident:
this.Log().ForIncident(IncidentPriority.P2, incidentType: "ORDER_EXPORT_FAIL", incidentId: order.Id).Error("Could not find product")
this.Log().ForIncident(IncidentPriority.P2, incidentType: "ORDER_EXPORT_FAIL", incidentId: order.Id).Error("Could not find customer")
```

## Publishing

The project is built on AppVeyor and set up to automatically push any release branch to NuGet.org. To create a new release, create a new branch using the following convention: `release/v<Major>.<Minor>`. AppVeyor will automatically append the build number.

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Authors

See the list of [contributors](https://github.com/distancify/Distancify.LitiumAddOns.Foundation/graphs/contributors) who participated in this project.

## License

This project is licensed under the LGPL v3 License - see the [LICENSE](LICENSE) file for details