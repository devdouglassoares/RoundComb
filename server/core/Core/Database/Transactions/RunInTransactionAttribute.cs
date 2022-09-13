using Microsoft.Practices.ServiceLocation;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Core.Database.Transactions
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RunInTransactionAttribute : ActionFilterAttribute
    {
        private ITransactionManager _transaction;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _transaction = ServiceLocator.Current.GetInstance<ITransactionManager>();

            _transaction?.BeginTransaction();

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Exception != null)
            {
                _transaction?.Rollback();
            }
        }
    }
}