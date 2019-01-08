using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Should.Core.Exceptions;
using Test.It.With.XUnit;
using Test.It.With.XUnit.Tests;
using Xunit;
using Xunit.Abstractions;

namespace Given_an_xunit_output_helper
{
    public class When_writing_to_trace_async : XUnit2SpecificationTesterAsync
    {
        public When_writing_to_trace_async() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override async Task WhenAsync()
        {
            await Task.Run(() => Trace.WriteLine("Testing"));
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_writing_to_input_async : XUnit2SpecificationTesterAsync
    {
        private InputWriter _inputWriter;

        public When_writing_to_input_async() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override async Task GivenAsync()
        {
            await Task.Run(() =>
            {
                _inputWriter = new InputWriter();
                _inputWriter.OnOutput += TestOutputHelper.Write;
            });
        }

        protected override async Task WhenAsync()
        {
            await Task.Run(() => _inputWriter.WriteLine("Testing"));
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_writing_to_output_async : XUnit2SpecificationTesterAsync
    {
        public When_writing_to_output_async() : base(A.Fake<ITestOutputHelper>())
        {
        }

        protected override async Task WhenAsync()
        {
            await Task.Run(() => TestOutputHelper.WriteLine("Testing"));
        }

        [Fact]
        public void It_should_have_written_to_output()
        {
            A.CallTo(() => OutputHelper.WriteLine("Testing")).MustHaveHappened(Repeated.Exactly.Once);
        }
    }

    public class When_writing_to_output_in_multiple_instances_at_the_same_time_async : XUnit2SpecificationAsync
    {
        private readonly ConcurrentBag<Test> _tests = new ConcurrentBag<Test>();
        private const int NumberOfTests = 1000;

        protected override async Task WhenAsync()
        {
            await Task.WhenAll(Enumerable.Range(0, NumberOfTests).Select(async i =>
            {
                await Task.Run(() =>
                {
                    using (var test = new Test(i, new ListeningTestOutputHelper()))
                    {
                        test.WriteLine("running");
                        _tests.Add(test);
                    }
                });
            }));
        }

        [Fact]
        public void It_should_have_written_messages()
        {
            var exceptions = new List<AssertException>();
            foreach (var test in _tests.Where(test => test.ListeningTestOutputHelper.MessagesWritten.Count != 1))
            {
                exceptions.Add(new AssertException($"{test.TestNumber} has {test.ListeningTestOutputHelper.MessagesWritten.Count} messages received. Expected 1."));
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        [Fact]
        public void It_should_have_written_a_message_to_each_output_helper()
        {
            var exceptions = new List<AssertException>();
            foreach (var test in _tests.Where(test => test.ListeningTestOutputHelper.MessagesWritten.Count != 1))
            {
                exceptions.Add(new AssertException($"{test.TestNumber} has received {(test.ListeningTestOutputHelper.MessagesWritten.Count == 0 ? "''" : string.Join(", ", test.ListeningTestOutputHelper.MessagesWritten.Select(s => $"'{s}'")))}. Expected 'running'."));
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
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
