import { HttpClient, HttpParams } from "@angular/common/http";
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

    getFilteredRows(tableName: string, columnName: string, value: string) {
        let params = new HttpParams();
        params = params.append("tableName", tableName);
        params = params.append("columnName", columnName);
        params = params.append("value", value);
        return this.http.get<Row[]>(this.baseUrl + 'row/filter', {
            params: params
        })
    }

    saveRow(row: Row, table: Table) {
        return this.http.post(this.baseUrl + 'row/save', {
            TableName: table.name,
            Row: row
        });
    }

    deleteRow(row: Row, table: Table) {
        let params = new HttpParams();
        params = params.append("tableName", table.name);
        params = params.append("key", row.keyValue);
        return this.http.delete(this.baseUrl + 'row/delete', {
            params: params
        });
    }
}