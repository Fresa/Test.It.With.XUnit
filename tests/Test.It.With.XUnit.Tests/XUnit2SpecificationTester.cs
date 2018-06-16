using Xunit.Abstractions;

namespace Test.It.With.XUnit.Tests
{
    public class XUnit2SpecificationTester : XUnit2Specification
    {
        public XUnit2SpecificationTester(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            OutputHelper = testOutputHelper;
        }

        protected ITestOutputHelper OutputHelper { get; }
    }
}