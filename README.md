# <img src="https://raw.githubusercontent.com/RobThree/IClock/master/logo.png" alt="Logo" width="32" height="32"> IClock
Provides a testable abstraction and alternative to `DateTime.Now` / `DateTime.UtcNow` and `DateTimeOffset.Now` / `DateTimeOffset.UtcNow`. Targets netstandard1.0 and higher.

## Why and how

When writing and testing (date)time-related code it is tempting to use any of the `DateTime.Now`, `DateTime.UtcNow`, `DateTimeOffset.Now` or `DateTimeOffset.UtcNow` properties. This, however, causes problems during (unit)testing. It makes your tests dependent on when your tests are run. This means your tests could pass on tuesdays and, without any changes, fail on wednesdays. Or only fail during nighttime or at any other time.

What you want is a clock that you can change at any time without any consequences and without having to change system time. This library provides interfaces and classes to handle just that. The `TestClock` is the clock which you can change freely without consequences to your system.

The basis for this library is the `ITimeProvider` interface which defines just one method: `GetTime()`. All "Clock" classes in this library implement this interface. Each of the clocks is described below. For all your code that requires (date)time information, make sure you use the `ITimeProvider` (or `IScopedTimeProvider`, see the `ScopedClock` for more information). Then, you can use one of the clocks (like the `LocalClock` or `UtcClock`) in your code and the `TestClock` in your unittests.

## Quickstart

In your code:

```c#
public class MyClass
{
    private readonly ITimeProvider _timeprovider;

    public MyClass(ITimeProvider timeProvider)
    {
        _timeprovider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public void Foo()
    {
        // Instead of writing:
        // var time = DateTime.Now;
        // Use:
        var time = _timeprovider.GetTime();
        // Do something
    }
}

var myclass = new MyClass(new UtcClock());
myClass.Foo();
//...
```

Or, even better, using Dependency Injection:

```c#
public void ConfigureServices(IServiceCollection services)
{
    // Register UtcClock as timeprovider
    services.AddScoped<ITimeProvider, UtcClock>();
    // ...
}
```

For usage in unittests, see the `TestClock` below.

## DateTimeOffset vs DateTime

IClock provides `DateTimeOffset` instead of `DateTime`. Why? Read this [excellent answer on StackOverflow](https://stackoverflow.com/a/14268167/215042) ([archived version](https://archive.is/6iv8z#answer-14268167)).

`DateTimeOffset` is great, however there are use-cases where you need to use `DateTime` as Local or UTC in which case you can use the `DateTimeOffset.LocalDateTime` and `DateTimeOffset.UtcDateTime` properties.

## LocalClock and UtcClock

These are the simplest `ITimeProvider`s. They provide, as their name suggests, the system's Local or UTC time.

### Example
```c#
var utctp = new UtcClock();
Console.WriteLine(utctp.GetTime());

var localtp = new LocalClock();
Console.WriteLine(localtp.GetTime());

```

## ScopedClock

This clock takes a 'snapshot' of the time when it is created and 'freezes' time during it's lifetime. This can be handy in cases where you need the same time during, for example, [handling a request](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#scoped) or transaction.

The `ScopedClock` takes either an `ITimeProvider`, `Func<DateTimeOffset>` or `DateTimeOffset` as constructor argument. The time from the provider, function or literal at the time of the `ScopedClock`'s construction will be the time the `ScopedClock` will provide when `GetTime()` is called.

### Example
```c#
public void ConfigureServices(IServiceCollection services)
{
    // Register UtcClock as timeprovider
    services.AddScoped<ITimeProvider, UtcClock>();
    // Register ScopedClock as scoped timeprovider
    services.AddScoped<IScopedTimeProvider, ScopedClock>();
    // ...
}
```

## ForwardOnlyClock

This clock is mostly a wrapper. It takes any `ITimeProvider` and ensures that time always 'moves forward'. This means that, for example, during DST changes time will appear to stand still while the time 'catches up' to the point at which the DST timechange happened. This can be of use for, as the example mentioned, DST changes but also for timesources (or `ITimeProviders`) that don't provide linear time.

### Example
```c#
var tc = new TestClock();
var fc = new ForwardOnlyClock(tc);

Console.WriteLine(fc.GetTime());    // Show time
tc.Adjust(TimeSpan.FromHours(-1));  // Set clock back 1 hour
Console.WriteLine(fc.GetTime());    // Show time, should be same as previous
```

## CustomClock

A simple wrapper `ITimeProvider` that takes a `Func<DateTimeOffset>` which it will then use whenever `GetTime()` is invoked. This can be used in situations where you already have a function that returns time but you need it as an `ITimeProvider`.

### Example
```c#
// Assume we have a GPS receiver that returns the time from the GPS signal and we need it as an `ITimeSource`.
var gpsclock = new CustomClock(() => MyGpsReceiver.CurrentTime);
Console.WriteLine(gpsclock.GetTime());  // Show time
```

## TestClock

This clock is primarily intended for use in unittesting scenarios and is the main reason for the existance of this library. See the 'Why and how' above. It can be initialized to any value (or it's default value of `2013-12-11 10:09:08.007+6:00`) and will only change when instructed. It has some conveniencemethods like `Set()` (set to a specific time), `Adjust()` (add/subtract time) and `Tick()` (advance time with a variable increment).

### Example
```c#
[TestMethod]
public void MyTest()
{
    var tc = new TestClock(new DateTime(1999, 12, 31, 21, 00, 00));
    var target = new PartyPlanner(tc);          // Pass our timeprovider

    Assert.IsTrue(target.IsPartyLike1999());
    tc.Adjust(TimeSpan.FromHours(4));           // Set clock ahead 4 hours
    Assert.IsFalse(target.IsPartyLike1999());
}
```

The `TestClock` provides a static method (`GetDeterministicRandomTime`, with an overload) that returns a (pseudo)random (date)time based on the caller method name (or a user supplied string) so tests that need to be repeatable (but maybe don't require a specific (date)time) can use this method of generating a (pseudo)random (date)time that is consistent over separate runs of the tests.

```c#
[TestMethod]
public void MyTest()
{
    var time = TestClock.GetDeterministicRandomTime();    // Returns 2004-07-02T18:10:46.2105328+00:00
}

[TestMethod]
public void AnotherTest()
{
    var time = TestClock.GetDeterministicRandomTime();    // Returns 1976-05-10T11:38:53.3889904+00:00
}
```


## Work in Progess. To do:

- [X] Readme with instructions, quickstart, examples and unittest examples etc.
- [ ] SHFB welcome page based on Readme
- [ ] Analyzer (See [here](https://github.com/dennisroche/DateTimeProvider#datetimeprovideranalyser-) and [here](https://github.com/Melchy/Clock#analyzer) for examples) to point out instances where `DateTime.Now`, `DateTime.UtcNow`, `DateTimeOffset.Now` or `DateTimeOffset.UtcNow` are used.

[![Build status](https://ci.appveyor.com/api/projects/status/cfocayl8qvi3d8cl)](https://ci.appveyor.com/project/RobIII/iclock) <a href="https://www.nuget.org/packages/IClock/"><img src="http://img.shields.io/nuget/v/IClock.svg?style=flat-square" alt="NuGet version" height="18"></a>

## License

Licensed under MIT license. See [LICENSE](https://raw.githubusercontent.com/RobThree/IClock/master/LICENSE) for details.

## Attribution

Icon made by [srip](https://www.flaticon.com/authors/srip) from [www.flaticon.com](https://www.flaticon.com/free-icon/wall-clock_899064?term=clock&page=1&position=15). Inspired by [dennisroche/DateTimeProvider](https://github.com/dennisroche/DateTimeProvider), [Melchy/Clock](https://github.com/Melchy/Clock) and [rbwestmoreland/system.clock](https://github.com/rbwestmoreland/system.clock) amongst others.