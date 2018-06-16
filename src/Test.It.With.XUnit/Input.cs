using System;
using System.IO;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    internal static class Input
    {
        public static TextWriter Writer => It.Input.Writer;

        public static IDisposable WriteTo(ITestOutputHelper testOutputHelper)
        {
            var testOutputHelperTextWriter = new TestOutputHelperTextWriter(testOutputHelper);
            var unregistrer = It.Input.WriteTo(testOutputHelperTextWriter);

            return DisposeList.FromRange(unregistrer, testOutputHelperTextWriter);
        }
    }
}