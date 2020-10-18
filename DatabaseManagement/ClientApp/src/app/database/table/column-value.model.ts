import { ColumnType } from "src/app/shared/column-type.enum";
import { Column } from "./column.model";

export interface IColumnValue {
    column: Column;
    value: any;

    getValue();
}

export class ColumnValue<T> implements IColumnValue {
    column: Column;
    value: T;

    constructor(column: Column, value: T) {
        this.column = column;
        this.value = value;
    }

    getValue() {
        return this.value;
    }

    static getInstance(column: Column, value: any): IColumnValue {
        if (column.type === ColumnType.INT || column.type === ColumnType.REAL) {
            return new ColumnValue<number>(column, +value);
        } else if (column.type === ColumnType.CHAR || column.type === ColumnType.STRING) {
            return new ColumnValue<string>(column, value);
        } else {
            return new ColumnValue<any>(column, value);
        }
    }
}