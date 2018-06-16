using System;
using System.IO;
using System.Text;

namespace Test.It.With.XUnit.Tests
{
    internal class InputWriter : TextWriter
    {
        public event Action<char> OnOutput;

        public override void Write(char value)
        {
            OnOutput?.Invoke(value);
        }

        public override Encoding Encoding => Encoding.Default;
    }
}