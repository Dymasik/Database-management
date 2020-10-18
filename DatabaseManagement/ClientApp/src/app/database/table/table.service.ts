import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { ColumnType } from "src/app/shared/column-type.enum";
import { Row } from "./row.model";
import { Table } from "./table.model";

@Injectable({
    providedIn: 'root'
})
export class TableService {
    
    constructor(private http: HttpClient,
                @Inject('BASE_URL') private baseUrl: string) {}

    getTables() {
        return this.http.get<Table[]>(this.baseUrl + 'table');
    }

    addTable(table: Table) {
        this.http.post(this.baseUrl + 'table/Add', table).subscribe(() => {});
    }

    getTableByName(name: string) {
        return this.http.get<Table>(this.baseUrl + 'table/' + name);
    }

    saveRow(row: Row, table: Table) {
        return this.http.post(this.baseUrl + 'row/save', {
            TableName: table.name,
            Row: row
        });
    }

    deleteRow(row: Row, table: Table) {
        return this.http.post(this.baseUrl + 'row/delete', {
            TableName: table.name,
            Row: row
        });
    }
}