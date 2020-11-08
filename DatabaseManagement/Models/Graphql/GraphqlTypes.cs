using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManagement.Models.Graphql
{

    public class TableType : ObjectGraphType<Table>
    {
        public TableType()
        {
            Field(x => x.Name);
            Field<ListGraphType<RowType>>("rows", resolve: context => context.Source.Rows);
        }
    }

    public class RowType : ObjectGraphType<Row>
    {
        public RowType()
        {
            Field<ListGraphType<ColumnValueType>>("values", resolve: context => context.Source.Values);
        }
    }

    public class ColumnValueType : ObjectGraphType<IColumnValue>
    {
        public ColumnValueType()
        {
            Field<StringGraphType>("value", resolve: context => context.Source.GetValue());
            Field<ColumnGraphType>("column", resolve: context => context.Source.Column);
        }
    }

    public class ColumnGraphType : ObjectGraphType<Column>
    {
        public ColumnGraphType()
        {
            Field(x => x.Name);
            Field<ColumnTypeEnum>("type", resolve: context => context.Source.Type);
            Field(x => x.IsKey);
        }
    }

    public class ColumnTypeEnum : EnumerationGraphType<ColumnType>
    {
    }
}
