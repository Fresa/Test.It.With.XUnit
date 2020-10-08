using System;
using System.IO;
using Test.It.Specifications;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2Specification : Specification, IDisposable
    {
        private readonly DisposableList _disposableList = new DisposableList();
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
            }

            if (startTestSetup)
            {
                Setup();
            }
        }

        private void SetupOutput()
        {
            _disposableList.Add(Output.WriteTo(_testOutputHelper));
        }

        protected readonly TextWriter TestOutputHelper = Output.Writer;
        
        protected virtual void Dispose(bool disposing)
        {
            _disposableList.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~XUnit2Specification()
        {
            Dispose(false);
        }
    }
}