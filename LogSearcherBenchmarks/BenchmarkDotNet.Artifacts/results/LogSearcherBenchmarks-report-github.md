```

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.4651/22H2/2022Update)
13th Gen Intel Core i7-13700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method               | Mean     | Error    | StdDev   | Median   |
|--------------------- |---------:|---------:|---------:|---------:|
| SearchErrorPattern   | 22.58 ns | 0.441 ns | 0.542 ns | 22.47 ns |
| SearchComplexPattern | 42.22 ns | 2.231 ns | 6.107 ns | 44.36 ns |
