using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Test.It.Specifications;
using Test.It.With.XUnit;
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
            using (Test.It.Output.WriteToTrace())
            {
                Trace.WriteLine("Testing");
            }
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing"))
                .MustHaveHappened(1, Times.Exactly);
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
            _inputWriter.OnOutput += TestOutputHelper.Write;
        }

        protected override void When()
        {
            _inputWriter.WriteLine("Testing");
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(1, Times.Exactly);
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
            A.CallTo(() => OutputHelper.WriteLine("Testing"))
                .MustHaveHappened(1, Times.Exactly);
        }
    }

    public class When_writing_to_output_in_multiple_instances_at_the_same_time : Specification
    {
        private readonly ConcurrentBag<Test> _tests = new ConcurrentBag<Test>();
        private const int NumberOfTests = 1000;

        public When_writing_to_output_in_multiple_instances_at_the_same_time()
        {
            Setup();
        }

        protected override void When()
        {
            Parallel.Invoke(Enumerable.Range(0, NumberOfTests).Select<int, Action>(i => () =>
            {
                using (var test = new Test(i, new ListeningTestOutputHelper()))
                {
                    test.WriteLine("running");
                    _tests.Add(test);
                }
            }).ToArray());
        }

        [Fact]
        public void It_should_have_written_a_message_to_each_output_helper()
        {
            foreach (var test in _tests.Where(test => test.ListeningTestOutputHelper.MessagesWritten.Count != 1))
            {
                test.ListeningTestOutputHelper.MessagesWritten.Should()
                    .HaveCount(
                        1,
                        $"{test.TestNumber} has received {(test.ListeningTestOutputHelper.MessagesWritten.Count == 0 ? "''" : string.Join(", ", test.ListeningTestOutputHelper.MessagesWritten.Select(s => $"'{s}'")))}. Expected 'running'.");
            }
        }

        private class Test : XUnit2Specification
        {
            public int TestNumber { get; }
            public ListeningTestOutputHelper ListeningTestOutputHelper { get; }

            public Test(int testNumber, ListeningTestOutputHelper testOutputHelper) : base(testOutputHelper)
            {
                TestNumber = testNumber;
                ListeningTestOutputHelper = testOutputHelper;
            }

            public void WriteLine(string text)
            {
                TestOutputHelper.WriteLine(text);
            }
        }

        private class ListeningTestOutputHelper : ITestOutputHelper
        {
            public List<string> MessagesWritten { get; } = new List<string>();

            public void WriteLine(string message)
            {
                MessagesWritten.Add(message);
            }

            public void WriteLine(string format, params object[] args)
            {
                MessagesWritten.Add(string.Format(format, args));
            }
        }
    }
}
