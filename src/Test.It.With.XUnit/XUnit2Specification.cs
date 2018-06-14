using System;
using Test.It.Specifications;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2Specification : Specification, IDisposable
    {
        private readonly DisposeList _disposeList = new DisposeList();

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
            if (TestOutputHelper.GetType() != typeof(NoTestOutputHelper))
            {
                _disposeList.Add(Output.WriteTo(TestOutputHelper));
            }
        }

        protected readonly ITestOutputHelper TestOutputHelper;

        protected virtual void Dispose(bool disposing)
        {
            _disposeList.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}