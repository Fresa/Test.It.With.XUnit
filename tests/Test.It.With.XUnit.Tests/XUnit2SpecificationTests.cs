using System.Diagnostics;
using FakeItEasy;
using Test.It.With.XUnit.Tests;
using Xunit;
using Xunit.Abstractions;

namespace Given_an_xunit_output_helper
{
    public class When_writing_to_trace : XUnit2SpecificationTester
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
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_writing_to_input : XUnit2SpecificationTester
    {
        private InputWriter _inputWriter;

        public When_writing_to_input() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override void Given()
        {
            _inputWriter = new InputWriter();
            _inputWriter.OnOutput += TestInputHelper.Write;
        }

        protected override void When()
        {
            _inputWriter.WriteLine("Testing");
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_writing_to_output : XUnit2SpecificationTester
    {
        public When_writing_to_output() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override void When()
        {
            TestOutputHelper.WriteLine("Testing");
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
