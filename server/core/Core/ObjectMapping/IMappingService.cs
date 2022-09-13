using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ObjectMapping
{
    public interface IMappingService : ISingletonDependency
    {
        IMapper Mapper { get; }
        
        IMappingExpression<TSource, TDestination> ConfigureMapping<TSource, TDestination>();

        TOut Map<TIn, TOut>(TIn model);

        TOut Map<TOut>(object model);

        TOut Map<TIn, TOut>(TIn source, TOut destination);

        IEnumerable<TOut> Map<TIn, TOut>(IEnumerable<TIn> source);

        IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source);

        void LockConfiguration();
    }

    public class MappingService : IMappingService
    {
        private readonly MapperConfigurationExpression _configuration = new MapperConfigurationExpression();
        private bool _configurationLocked = false;

        public static IConfigurationProvider MappingConfiguration { get; private set; }

        public IMapper Mapper { get; private set; }

        public IMappingExpression<TSource, TDestination> ConfigureMapping<TSource, TDestination>()
        {
            if (_configurationLocked)
                throw new InvalidOperationException("Mapping configuration is locked, please configure mapping before it's locked.");

            return _configuration.CreateMap<TSource, TDestination>();
        }

        public TOut Map<TIn, TOut>(TIn model)
        {
            return Mapper.Map<TIn, TOut>(model);
        }

        public TOut Map<TOut>(object model)
        {
            return Mapper.Map<TOut>(model);
        }

        public TOut Map<TIn, TOut>(TIn source, TOut destination)
        {
            return Mapper.Map(source, destination);
        }

        public IEnumerable<TOut> Map<TIn, TOut>(IEnumerable<TIn> source)
        {
            return Mapper.Map<IEnumerable<TOut>>(source);
        }

        public IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source)
        {
            return source.ProjectTo<TOut>(MappingConfiguration);
        }

        public void LockConfiguration()
        {
            MappingConfiguration = new MapperConfiguration(_configuration);
            Mapper = new Mapper(MappingConfiguration);

            _configurationLocked = true;
        }
    }
}