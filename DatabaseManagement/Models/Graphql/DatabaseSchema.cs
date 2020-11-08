using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models.Graphql
{
    public class DatabaseSchema : Schema
    {
        public DatabaseSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<DatabaseQuery>();
        }
    }
}
