using System;
using System.Diagnostics;
using System.Linq;
using Test.It.Specifications;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2Specification : Specification, IDisposable
    {
        protected readonly ITestOutputHelper TestOutputHelper;

        protected XUnit2Specification() 
            : this(new NoTestOutputHelper())
        {
        }

        protected XUnit2Specification(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;

            var outputWriter = new TestOutputHelperTextWriter(testOutputHelper);
            Console.SetOut(outputWriter);
            if (Trace.Listeners.OfType<ConsoleTraceListener>().Any() == false)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }

            Setup();
        }

        protected event EventHandler OnDisposing;

        public void Dispose()
        {
            OnDisposing?.Invoke(this, null);
        }
    }
}