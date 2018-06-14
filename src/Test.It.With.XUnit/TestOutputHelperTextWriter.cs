using System.IO;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Test.It.With.XUnit
{
    internal class TestOutputHelperTextWriter : TextWriter
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string _value = string.Empty;
        private readonly object _lock = new object();
        
        public TestOutputHelperTextWriter(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public override void Write(char value)
        {
            lock (_lock)
            {
                _value += value;
                if (_value.EndsWith(NewLine))
                {
                    _testOutputHelper.WriteLine(_value.Substring(0, _value.Length - NewLine.Length));
                    _value = string.Empty;
                }
            }
        }

        public override void Flush()
        {
            lock (_lock)
            {
                if (_value.Any())
                {
                    _testOutputHelper.WriteLine(_value);
                    _value = string.Empty;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Flush();
            }
        }

        public override Encoding Encoding { get; } = Encoding.UTF8;
    }
}