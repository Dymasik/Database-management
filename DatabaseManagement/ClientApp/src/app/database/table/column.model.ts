import { ColumnType } from "src/app/shared/column-type.enum";

export class Column {
    name: string;
    type: ColumnType;
    isKey: boolean;

    constructor(name: string, type: ColumnType, isKey?: boolean) {
        this.name = name;
        this.type = type;
        this.isKey = isKey;
    }
}