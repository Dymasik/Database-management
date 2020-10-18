import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ColumnType } from 'src/app/shared/column-type.enum';
import { ColumnValue } from '../table/column-value.model';
import { Row } from '../table/row.model';
import { Table } from '../table/table.model';
import { TableService } from '../table/table.service';

@Component({
  selector: 'app-table-content',
  templateUrl: './table-content.component.html',
  styleUrls: ['./table-content.component.css']
})
export class TableContentComponent implements OnInit {

  private tableName: string;
  table: Table;
  tableForm: FormGroup;
  initializedTable: boolean = false;

  constructor(private route: ActivatedRoute,
              private tableService: TableService) { }

  ngOnInit() {
    this.route.params.subscribe(
      params => {
        this.tableName = params['name'];
        this.initTable();
      }
    );
  }

  private initTable() {
    this.tableService.getTableByName(this.tableName)
      .subscribe(table => { 
        this.table = table;
        this.initTableForm();
      });
  }

  private initTableForm() {
    const rows = new FormArray([]);
    if (this.table && this.table.rows) {
      for (let row of this.table.rows) {
        var rowConfig = {};
        for (let value of row.values) {
          rowConfig[value.column.name] = new FormControl(value.value);
        }
        rows.push(new FormGroup(rowConfig));
      }
    }
    this.tableForm = new FormGroup({
      'rows': rows
    });
    this.initializedTable = true;
  }

  onSave() {}

  get rowControls() {
    return (this.tableForm.get('rows') as FormArray).controls;
  }

  private getRow(index: number): Row {
    let row = new Row();
    let rowConfig = (this.tableForm.get('rows') as FormArray).value[index];
    for (var prop in rowConfig) {
      let column = this.table.structure.find(column => {
        if (column.name === prop) {
          return true;
        }
      });
      if (!column) {
        continue;
      }
      let value = ColumnValue.getInstance(column, rowConfig[prop]);
      row.values.push(value);
    }
    return row;
  }

  onAddRow() {
    var rowConfig = {};
    for (let column of this.table.structure) {
      rowConfig[column.name] = new FormControl(null);
    }
    (this.tableForm.get('rows') as FormArray).push(new FormGroup(rowConfig));
  }

  onSaveRow(index: number) {
    var row = this.getRow(index);
    this.tableService.saveRow(row, this.table)
    .subscribe();
  }

  onDeleteRow(index: number) {
    var row = this.getRow(index);
    this.tableService.deleteRow(row, this.table)
      .subscribe();
  }

  getColumnTypeAttribute(type: number) {
    switch (type) {
      case 0:
      case 1:
        return 'number';
      case 2:
      case 3:
        return 'text';
      case 4:
        return 'date';
      default:
        break;
    }
  }
}
