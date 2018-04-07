using System;
using System.IO;

namespace Athena.Importer.Tests
{
    public class NoOutputTest : IDisposable
    {
        private readonly TextWriter _out, _err;
        
        public NoOutputTest()
        {
            _out = Console.Out;
            _err = Console.Error;
            Console.SetOut(new StringWriter());
            Console.SetError(new StringWriter());
        }

        public virtual void Dispose()
        {
            Console.SetOut(_out);
            Console.SetError(_err);
        }
    }
}