import { Column } from "./column.model";
import { Row } from "./row.model";

export class Table {
    name: string;
    structure: Column[];
    rows: Row[];
    
    addColumn(column: Column) {
        this.structure.push(column);
    }
}