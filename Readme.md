# Supported Patterns
## Wildcards:
  * — matches any sequence of characters
  ? — matches a single character
## Logical Operators:
  and — both expressions must be present
  or — at least one of the expressions must be present
## Grouping:
  Use parentheses () to group conditions
## Pattern Examples:
  "Error:* and database" — searches for lines containing both "Error" and "database".
  "(Error:* or Warning:*) and (space or disk)" — searches for lines containing "Error" or "Warning" and "space" or "disk".


# Benchmark test results

| Method               | Mean         | Error        | StdDev       |
|--------------------- |-------------:|-------------:|-------------:|
| SearchErrorPattern   |     21.43 ns |     0.358 ns |     0.525 ns |
| SearchComplexPattern | 19,707.22 ns | 2,531.693 ns | 7,344.900 ns |

## Guide to Run Benchmark Tests

To run the benchmark tests, execute the following commands:

```bash
cd LogSearcherBenchmarks
```

``` bash
dotnet run -c Release
