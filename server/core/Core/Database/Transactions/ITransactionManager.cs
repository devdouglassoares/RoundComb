using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http.Controllers;

namespace Core.Database.Transactions
{
	public interface ITransactionManager : IDisposable, IDependency
	{
		string TimeStamp { get; }

		TransactionScope BeginTransaction();

		void Rollback();
	}

	public class TransactionManager : ITransactionManager
	{
		private TransactionScope _transactionScope;

		private bool _roolback = false;

		public TransactionManager()
		{
			TimeStamp = DateTime.Now.ToString("hh.mm.ss.ffffff");
		}

		public void Commit()
		{
			_transactionScope.Complete();
		}

		public string TimeStamp { get; set; }

		public TransactionScope BeginTransaction()
		{
			_transactionScope = new TransactionScope();

			return _transactionScope;
		}

		public void Rollback()
		{
			_roolback = true;
		}

		public void Dispose()
		{
			if (_transactionScope != null)
			{
				lock (this)
				{
					if (!_roolback)
						Commit();

					try
					{
						_transactionScope.Dispose();
					}
					catch (InvalidOperationException exception)
					{
						// Transaction is being disposed in another thread, reinitialize and dispose
						BeginTransaction();
						_transactionScope.Dispose();
					}
					finally
					{
					}
				}
			}
		}
	}
}