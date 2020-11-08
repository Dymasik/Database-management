using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseManagement.Models.Graphql
{
    public class DatabaseQuery : ObjectGraphType
    {
        public DatabaseQuery()
        {
            var db = Database.GetInstance();
            Field<ListGraphType<TableType>>("tables",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<StringGraphType>
                    {
                        Name = "tableName"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "columnName"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "value"
                    }
                }),
                resolve: context =>
                {
                    var tableName = context.GetArgument<string>("tableName");
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        return db.Where(t => t.Name == tableName);
                    }

                    var columnName = context.GetArgument<string>("columnName");
                    var value = context.GetArgument<string>("value");
                    if (!string.IsNullOrEmpty(columnName) && !string.IsNullOrEmpty(value))
                    {
                        return db
                            .Where(t => 
                                t.Rows
                                    .Where(r => r.Values.Where(v => {
                                        return v.Column.Name.Equals(columnName) &&
                                            v.GetValue().Equals(value);
                                        })
                                    .Count() > 0)
                                .Count() > 0);
                    }

                    return db;
                }
            );
        }
    }
}