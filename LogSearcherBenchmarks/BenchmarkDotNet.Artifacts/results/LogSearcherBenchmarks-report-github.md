```

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.4651/22H2/2022Update)
13th Gen Intel Core i7-13700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method               | Mean         | Error        | StdDev       |
|--------------------- |-------------:|-------------:|-------------:|
| SearchErrorPattern   |     21.43 ns |     0.358 ns |     0.525 ns |
| SearchComplexPattern | 19,707.22 ns | 2,531.693 ns | 7,344.900 ns |
