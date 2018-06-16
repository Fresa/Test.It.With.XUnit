using System;
using System.IO;
using Test.It.Specifications;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2Specification : Specification, IDisposable
    {
        private readonly DisposeList _disposeList = new DisposeList();
        private readonly ITestOutputHelper _testOutputHelper;

        protected XUnit2Specification()
            : this(new NoTestOutputHelper())
        {
        }

        protected XUnit2Specification(ITestOutputHelper testOutputHelper, bool startTestSetup = true)
        {
            _testOutputHelper = testOutputHelper;

            if (_testOutputHelper.GetType() != typeof(NoTestOutputHelper))
            {
                SetupOutput();
                SetupInput();
            }

            if (startTestSetup)
            {
                Setup();
            }
        }

        private void SetupOutput()
        {
            _disposeList.Add(XUnit.Output.WriteTo(_testOutputHelper));
        }

        private void SetupInput()
        {
            _disposeList.Add(XUnit.Input.WriteTo(_testOutputHelper));
        }

        protected readonly TextWriter TestInputHelper = XUnit.Input.Writer;
        protected readonly TextWriter TestOutputHelper = XUnit.Output.Writer;
        
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