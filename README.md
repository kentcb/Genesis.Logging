![Logo](Art/Logo150x150.png "Logo")

# Genesis.Logging

[![Build status](https://ci.appveyor.com/api/projects/status/uj2imiiui6flarih?svg=true)](https://ci.appveyor.com/project/kentcb/genesis-logging)

## What?

> All Genesis.* projects are formalizations of small pieces of functionality I find myself copying from project to project. Some are small to the point of triviality, but are time-savers nonetheless. They have a particular focus on performance with respect to mobile development, but are certainly applicable outside this domain.
 
**Genesis.Logging** is a simple library for application authors (_not_ library authors) to facilitate performance-focussed logging. It is delivered as a netstandard 1.0 binary.

## Why?

There are a plethora of logging solutions out there for .NET. **Genesis.Logging** differs in these key ways:

* it is highly focussed on performance. Generally speaking, you will not instigate any allocations when logging. Moreover, you can remove logging calls altogether via a compilation symbol.
* it has nothing to say about how log entries are consumed. Instead, it merely exposes an observable of `LogEntry` instances (which is a `struct`) and leaves it to consumers to decide what to do with log entries. This keeps the library simple and is ideal for mobile applications.

## Where?

The easiest way to get **Genesis.Logging** is via [NuGet](http://www.nuget.org/packages/Genesis.Logging/):

```PowerShell
Install-Package Genesis.Logging
```

## How?

Generally, your components would normally depend on `ILogger`. Implementations of `ILogger` can be obtained from an `ILoggerService`. For convenience (and because logging rarely needs to be unit tested), an ambient context is available via `LoggingService.Current`. By assigning an `ILoggerService` to this property, your application components can then obtain an `ILogger` simply by calling one of the static `LoggerService.GetLogger` methods.

**Genesis.Logging** comes with two implementations of `ILoggerService`:

* `DefaultLoggerService` : this is the default implementation that you will generally want to use.
* `NullLoggerService` : this is an implementation that does absolutely nothing.

Once you have an `ILogger`, logging against it is easy:

```C#
ILogger logger = ...;

logger.Debug("Some debug message.");

// a formatted message
logger.Debug("Some debug {0}.", "message");

// a message with an exception
logger.Debug(ex, "Something went wrong when comparing to {0}.", 42);
```

Overloads for the following log levels exist:

* `Debug`
* `Info`
* `Perf` (there is no exception support for performance logging)
* `Warn`
* `Error`

The `Perf` overloads are unique. They allow you to time a block of code. They do not log immediately but instead return a `PerformanceBlock` which you should dispose of when the timed code completes:

```C#
using (logger.Perf("Dividing by {0}.", divisor))
{
    var result = someNumber / divisor;
}
```

This will result in a message that includes timing information.

**IMPORTANT:** unless your consuming assembly defines the `LOGGING` compiler symbol, most logging calls will be elided from the built assembly. This makes it very simple to optimize out these calls for performance-critical scenarios, such as mobile applications and embedded development. Note that `Perf` calls cannot be completely elided because they return a value.

## Who?

**Genesis.Logging** is created and maintained by [Kent Boogaart](http://kent-boogaart.com). Issues and pull requests are welcome.