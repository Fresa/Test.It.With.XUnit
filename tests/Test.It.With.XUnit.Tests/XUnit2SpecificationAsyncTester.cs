using Xunit.Abstractions;

namespace Test.It.With.XUnit.Tests
{
    public class XUnit2SpecificationTesterAsync : XUnit2SpecificationAsync
    {
        public XUnit2SpecificationTesterAsync(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            OutputHelper = testOutputHelper;
        }

        protected ITestOutputHelper OutputHelper { get; }
    }
}