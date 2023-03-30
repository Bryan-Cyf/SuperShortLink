// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using SuperShortLink.Benchmark;
//var summary = BenchmarkRunner.Run<GenerateBanchmark>();
//Console.WriteLine(summary);

var summary = BenchmarkRunner.Run<GenerateApiBanchmark>();
Console.WriteLine(summary);