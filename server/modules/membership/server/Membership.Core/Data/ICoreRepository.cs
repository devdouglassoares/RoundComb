using System;
using Core;
using Core.Database.Repositories;

namespace Membership.Core.Data
{
    /// <summary>
    /// Represents entities repository.
    /// </summary>
    public interface ICoreRepository : IBaseRepository, IDependency, IDisposable
    {
    }
}