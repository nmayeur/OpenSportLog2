using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Common.Infrastructure.Log
{
    public interface ILoggerService
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
}
