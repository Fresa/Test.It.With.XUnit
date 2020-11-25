using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;
using Xunit;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public abstract class XUnit2SpecificationAsync : SpecificationAsync,
        IAsyncLifetime
    {
        private readonly DisposableList _disposableList = new DisposableList();
        private readonly ITestOutputHelper _testOutputHelper;

        protected XUnit2SpecificationAsync()
            : this(new NoTestOutputHelper())
        {
        }

        protected XUnit2SpecificationAsync(
            ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            if (_testOutputHelper.GetType() != typeof(NoTestOutputHelper))
            {
                SetupOutput();
            }
        }

        protected virtual CancellationTokenSource CancellationTokenSource
        {
            get;
        } = new CancellationTokenSource();

        private void SetupOutput()
        {
            _disposableList.Add(Output.WriteTo(_testOutputHelper));
        }

        protected readonly TextWriter TestOutputHelper = Output.Writer;

        protected virtual Task DisposeAsync(
            bool disposing)
        {
            if (!disposing)
            {
                return Task.CompletedTask;
            }

            CancellationTokenSource.Dispose();
            _disposableList.Dispose();
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
            => SetupAsync(CancellationTokenSource.Token);

        public Task DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return DisposeAsync(true);
        }

        ~XUnit2SpecificationAsync()
        {
            DisposeAsync(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}