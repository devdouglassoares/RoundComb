using System.Collections.Generic;

namespace Core.Events
{
    public class EntityCreating<TEntity, TDto>
    {
        public TEntity Entity { get; set; }

        public TDto Dto { get; set; }
    }

    public class EntityBeingInserted
    {
        public dynamic Entity { get; set; }
    }

    public class EntityBeingUpdated
    {
        public dynamic Entity { get; set; }
    }

    public class EntityCreated<TEntity, TDto>
    {
        public TEntity Entity { get; set; }

        public TDto Dto { get; set; }
    }

    public class EntityUpdating<TEntity, TDto>
    {
        public TEntity Entity { get; set; }

        public TDto Dto { get; set; }
    }

    public class EntityUpdated<TEntity, TDto>
    {
        public TEntity Entity { get; set; }

        public TDto Dto { get; set; }
    }

    public class EntityDeleted<TEntity>
    {
        public TEntity Entity { get; set; }
    }

    public class EntityDtoWireUp<TEntity, TDto>
    {
        public TEntity Entity { get; set; }

        public TDto Dto { get; set; }
    }

    public class EntitiesDtoWireUp<TEntity, TDto>
    {
        public IEnumerable<TEntity> Entity { get; set; }

        public IEnumerable<TDto> Dto { get; set; }
    }
}