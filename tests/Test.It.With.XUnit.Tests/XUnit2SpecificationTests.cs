using System.Diagnostics;
using FakeItEasy;
using Test.It.With.XUnit;
using Xunit;
using Xunit.Abstractions;

namespace Given_an_xunit_output_helper
{
    public class When_writing_to_trace : XUnit2Specification
    {
        public When_writing_to_trace() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override void When()
        {
            Trace.WriteLine("Testing");
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => TestOutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
