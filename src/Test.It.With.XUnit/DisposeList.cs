using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.It.With.XUnit
{
    internal class DisposeList : List<IDisposable>, IDisposable
    {
        public static DisposeList FromRange(params IDisposable[] disposables)
        {
            var list = new DisposeList();

            if (disposables.Any())
            {
                list.AddRange(disposables);
            }

            return list;
        }

        public void Dispose()
        {
            ForEach(disposable => disposable.Dispose());
        }
    }
}