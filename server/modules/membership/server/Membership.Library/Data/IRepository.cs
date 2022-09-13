using System;
using Core;
using Core.Database.Repositories;

namespace Membership.Library.Data
{
    /// <summary>
    /// Represents entities repository.
    /// </summary>
    public interface IRepository : IBaseRepository, IDependency, IDisposable
    {
    }
}