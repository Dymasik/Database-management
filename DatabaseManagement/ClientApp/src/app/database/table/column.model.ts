import { ColumnType } from "src/app/shared/column-type.enum";

export class Column {
    name: string;
    type: ColumnType;

    constructor(name: string, type: ColumnType) {
        this.name = name;
        this.type = type;
    }
}