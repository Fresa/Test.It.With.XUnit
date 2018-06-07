using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Test.It.Specifications;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2Specification : Specification, IDisposable
    {
        protected readonly ITestOutputHelper TestOutputHelper;
        private TextWriter _standardOutput;

        protected XUnit2Specification()
            : this(new NoTestOutputHelper())
        {
        }

        protected XUnit2Specification(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
            SetupOutput();

            Setup();
        }

        private void SetupOutput()
        {
            if (Trace.Listeners.OfType<ConsoleTraceListener>().Any() == false)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }

            if (TestOutputHelper.GetType() != typeof(NoTestOutputHelper))
            {
                var outputWriter = new TestOutputHelperTextWriter(TestOutputHelper);
                _standardOutput = Console.Out;
                Console.SetOut(outputWriter);
            }

            OnSetupOutputDone();
        }

        protected virtual void OnSetupOutputDone() { }

        protected event EventHandler OnDisposing;

        public virtual void Dispose()
        {
            OnDisposing?.Invoke(this, null);
            if (_standardOutput != null)
            {
                Console.SetOut(_standardOutput);
            }
        }
    }
}