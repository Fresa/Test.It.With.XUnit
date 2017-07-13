using System;
using System.Diagnostics;
using System.Linq;
using FakeItEasy;
using Should.Fluent;
using Xunit;
using Xunit.Abstractions;

namespace Test.It.With.XUnit.Tests
{
    public class When_running_a_test_without_a_test_output_helper_and_writing_to_console : XUnit2Specification
    {
        protected override void When()
        {
            base.When();
            Console.WriteLine("Testing");
        }

        [Fact]
        public void It_should_have_added_a_console_trace_listener()
        {
            Trace.Listeners.OfType<ConsoleTraceListener>().Should().Count.AtLeast(1);
        }
    }

    public class When_running_a_test_with_a_test_output_helper_and_writing_to_console : XUnit2Specification
    {
        private static readonly ITestOutputHelper OutputHelper = A.Fake<ITestOutputHelper>();

        public When_running_a_test_with_a_test_output_helper_and_writing_to_console() 
            : base(OutputHelper)
        {
            
        }

        protected override void When()
        {
            base.When();
            Console.WriteLine("Testing");
        }

        [Fact]
        public void It_should_write_to_the_test_output_helper()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened();
        }

        [Fact]
        public void It_should_have_added_a_console_trace_listener()
        {
            Trace.Listeners.OfType<ConsoleTraceListener>().Should().Count.AtLeast(1);
        }
    }
}
