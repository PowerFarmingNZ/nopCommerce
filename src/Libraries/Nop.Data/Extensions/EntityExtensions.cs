using System;
using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;

namespace Nop.Data.Extensions
{
    /// <summary>
    /// Represents extensions
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Get unproxied entity type
        /// </summary>
        /// <remarks> If your Entity Framework context is proxy-enabled, 
        /// the runtime will create a proxy instance of your entities, 
        /// i.e. a dynamically generated class which inherits from your entity class 
        /// and overrides its virtual properties by inserting specific code useful for example 
        /// for tracking changes and lazy loading.
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Type GetUnproxiedEntityType(this BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Type type;
            //cachable entity (get the base entity type)
            if (entity is IEntityForCaching)
            {
                type = ((IEntityForCaching) entity).GetType().BaseType;
            }
            else
            {
                var dbContext = EngineContext.Current.Resolve<IDbContext>() as DbContext;
                type = dbContext?.Model.FindRuntimeEntityType(entity.GetType()).ClrType;
            }

            if (type == null)
                throw new Exception("Original entity type cannot be loaded");

            return type;
        }
    }
}