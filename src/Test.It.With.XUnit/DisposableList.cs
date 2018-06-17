using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.It.With.XUnit
{
    internal class DisposableList : List<IDisposable>, IDisposable
    {
        public static DisposableList FromRange(params IDisposable[] disposables)
        {
            var list = new DisposableList();

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