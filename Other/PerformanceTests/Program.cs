using PerformanceTests;
using Stride.Core.Yaml.Serialization;

Console.WriteLine("Hello, World!");

var yamlTest = new YamlTest();
var sut = new Serializer();
var result = sut.Deserialize(yamlTest.YamlFile("test2.yaml"), typeof(object));

Console.WriteLine(result);