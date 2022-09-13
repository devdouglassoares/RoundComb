using Common.Core.Entities;
using Core;
using Core.Database;

namespace Common.Services.Interfaces
{
    public interface IExceptionLoggerService : IBaseService<ExceptionLogger, ExceptionLoggerDto>, IDependency
    {
        void LogExceptions(ExceptionLoggerDto[] exceptions);
    }
}