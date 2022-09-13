using Core.Events;
using Core.UI.DataTablesExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Core.UI
{
    public class DataTableDataSourceFiltered<T> where T : class
    {
        public IEnumerable<T> Datasource { get; set; }
    }

    public interface IDataTableService : IDependency
    {
        [Obsolete("Try to use GetResponse(IQueryable<T>, IDataTablesRequest, Action<IEnumerable<T>>) instead")]
        DataTablesResponse GetResponse<T>(IEnumerable<T> data, IDataTablesRequest request, Action<IEnumerable<T>> mappingExpression = null) where T : class;

        DataTablesResponse GetResponse<T>(IQueryable<T> data, IDataTablesRequest request, Action<IEnumerable<T>> mappingExpression = null) where T : class;
    }

    public class DataTableService : IDataTableService
    {
        private readonly IEventPublisher _eventPublisher;

        public DataTableService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public DataTablesResponse GetResponse<T>(IEnumerable<T> data, IDataTablesRequest request, Action<IEnumerable<T>> mappingExpression = null) where T : class
        {
            return GetResponse(data.AsQueryable(), request, mappingExpression);
        }

        public DataTablesResponse GetResponse<T>(IQueryable<T> data, IDataTablesRequest request, Action<IEnumerable<T>> mappingExpression = null) where T : class
        {
            var totalDataCount = data.Count();
            var myFilteredData = data.AsQueryable();

            if (request.Search != null && !string.IsNullOrEmpty(request.Search.Value))
            {
                var searchableColumns = request.Columns.Where(x => x.Searchable);
                myFilteredData = SearchData(myFilteredData, searchableColumns, request.Search);
            }

            var filteredColumns = request.Columns.GetFilteredColumns();
            foreach (var column in filteredColumns)
            {
                myFilteredData = FilterData(myFilteredData, column.Data, column.Search);
            }

            var sortedColumns = request.Columns.GetSortedColumns();

            myFilteredData = SortData(myFilteredData, sortedColumns);

            var paged = myFilteredData.Skip(request.Start).Take(request.Length).ToArray();

            _eventPublisher.Publish(new DataTableDataSourceFiltered<T> { Datasource = paged });

            mappingExpression?.Invoke(paged);

            return new DataTablesResponse(request.Draw,
                                          paged,
                                          myFilteredData.Count(),
                                          totalDataCount);
        }

        private IQueryable<T> SearchData<T>(IQueryable<T> myFilteredData, IEnumerable<Column> searchableColumns, Search search)
        {
            var strBuilder = new List<String>();

            foreach (var col in searchableColumns)
            {
                strBuilder.Add(col.Data + " != NULL && " + col.Data + ".ToLower().Contains(@0)");
            }

            var orderQuery = string.Join(" || ", strBuilder);

            return myFilteredData.Where(orderQuery, search.Value.ToLower());
        }

        private static IQueryable<T> FilterData<T>(IQueryable<T> myFilteredData, string columnName, Search search)
        {
            return myFilteredData.Where(columnName + " != NULL && " + columnName + ".ToLower().Contains(@0)", search.Value.ToLower());
        }

        private static IQueryable<T> SortData<T>(IQueryable<T> myFilteredData, IEnumerable<Column> sortColumn)
        {
            var strBuilder = new List<string>();

            foreach (var col in sortColumn)
            {
                var sortDirection = col.SortDirection == Column.OrderDirection.Ascendant ? "asc" : "desc";
                strBuilder.Add(col.Data + " " + sortDirection);
            }

            var orderQuery = string.Join(",", strBuilder);

            if (string.IsNullOrEmpty(orderQuery))
                return myFilteredData;

            return myFilteredData.OrderBy(orderQuery);
        }
    }
}