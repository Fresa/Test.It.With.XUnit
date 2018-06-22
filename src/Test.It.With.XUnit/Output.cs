using System;
using System.IO;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    public static class Output
    {
        public static TextWriter Writer => It.Output.Writer;

        public static IDisposable WriteTo(ITestOutputHelper testOutputHelper)
        {
            var testOutputHelperTextWriter = new TestOutputHelperTextWriter(testOutputHelper);
            var unregistrer = It.Output.WriteTo(testOutputHelperTextWriter);

            return DisposableList.FromRange(unregistrer, testOutputHelperTextWriter);
        }
    }
}