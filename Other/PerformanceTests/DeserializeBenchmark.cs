using BenchmarkDotNet.Attributes;
using Stride.Core.Yaml.Serialization;

namespace PerformanceTests
{
    [MemoryDiagnoser]
    public class DeserializeBenchmark
    {
        [Benchmark]
        public void DeserializeScalar()
        {
            var yamlTest = new YamlTest();
            var sut = new Serializer();
            var result = sut.Deserialize(yamlTest.YamlFile("test2.yaml"), typeof(object));
        }

        [Benchmark]
        public void DeserializeScalarWithSpan()
        {
            var yamlTest = new YamlTest();
            var sut = new Serializer();
            var result = sut.Deserialize(yamlTest.YamlFile("test2.yaml"), typeof(object));
        }
    }
}
