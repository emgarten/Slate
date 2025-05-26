# Slate Utils

A small collection of helper utilities for .NET projects.

This package currently includes:

- **ExceptionUtils** – helpers for working with `Exception` objects
- **PatternUtils** – convert wildcard patterns to regular expressions
- **TaskUtils** – run asynchronous work in parallel with optional throttling

## ExceptionUtils

`ExceptionUtils` helps extract readable messages and flatten nested exceptions.

```csharp
try
{
    // code that may throw
}
catch (Exception ex)
{
    // Prints "[System.InvalidOperationException] Something went wrong"
    Console.WriteLine(ExceptionUtils.GetExceptionMessage(ex));
}
```

To get to the original exception in an `AggregateException` or a nested
`TargetInvocationException`, call `Unwrap`:

```csharp
Exception root = ExceptionUtils.Unwrap(ex);
```

## PatternUtils

`PatternUtils.WildcardToRegex` converts wildcard expressions like `*.txt` or
`file-??.log` into a `Regex`.

```csharp
var regex = PatternUtils.WildcardToRegex("*.txt", ignoreCase: true);
bool match = regex.IsMatch("readme.TXT"); // true
```

## TaskUtils

`TaskUtils` runs asynchronous tasks in parallel while preserving the order of
results. Concurrency can be limited using the `maxThreads` parameter.

Run tasks without caring about return values:

```csharp
var tasks = new List<Func<Task>>
{
    () => ProcessAsync("a"),
    () => ProcessAsync("b"),
};
await TaskUtils.RunAsync(tasks, maxThreads: 4);
```

Run tasks and get the results in the original order:

```csharp
var work = new List<Func<Task<int>>>
{
    () => GetDataAsync(1),
    () => GetDataAsync(2),
};
int[] results = await TaskUtils.RunAsync(work, maxThreads: 4);
```

You can also provide a custom processing function that transforms each task's
result before it is placed into the output array.

```csharp
int[] doubled = await TaskUtils.RunAsync(
    work,
    maxThreads: 4,
    useTaskRun: true,
    process: async t => (await t) * 2,
    token: CancellationToken.None);
```


