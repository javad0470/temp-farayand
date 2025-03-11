using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public static class ExtentionMethods
    {
        public static ObjectContext GetContext(this IEntityWithRelationships entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var relationshipManager = entity.RelationshipManager;

            var relatedEnd = relationshipManager.GetAllRelatedEnds()
                                                .FirstOrDefault();

            if (relatedEnd == null)
                throw new Exception("No relationships found");

            var query = relatedEnd.CreateSourceQuery() as ObjectQuery;

            if (query == null)
                throw new Exception("The Entity is Detached");

            return query.Context;
        }


        public static T GetContext<T>(this IEntityWithRelationships entity) where T : ObjectContext
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var relationshipManager = entity.RelationshipManager;

            var relatedEnd = relationshipManager.GetAllRelatedEnds()
                                                .FirstOrDefault();

            if (relatedEnd == null)
                throw new Exception("No relationships found");

            var query = relatedEnd.CreateSourceQuery() as ObjectQuery;

            if (query == null)
                throw new Exception("The Entity is Detached");

            return query.Context as T;
        }
    }
}
