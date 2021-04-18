using System;
using System.Collections.Generic;
using System.Text;

namespace Api.infrastructure.Extensions
{
    public interface IStartupTask
    {
        void Run();
    }
}
