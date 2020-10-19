import { IColumnValue } from "./column-value.model";

export class Row {
    values: IColumnValue[] = [];

    get keyValue() {
        for (let columnValue of this.values) {
            if (columnValue.column.isKey) {
                return columnValue.value;
            }
        }
        return null;
    }
}