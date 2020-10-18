import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ColumnType } from 'src/app/shared/column-type.enum';
import { TableService } from '../table.service';

@Component({
  selector: 'app-table-edit',
  templateUrl: './table-edit.component.html',
  styleUrls: ['./table-edit.component.css']
})
export class TableEditComponent implements OnInit {

  editMode: boolean = false;
  tableName: string;
  tableForm: FormGroup;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private tableService: TableService) { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.tableName = params['name'];
        this.editMode = params['id'] != null;
        this.initForm();
      }
    );
  }

  private initForm() {
    let columns = new FormArray([]);

    this.tableForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'columns': columns
    });
  }

  onSave() {
    this.tableService.addTable(this.tableForm.value);
  }

  onCancel() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  onDeleteColumn(index: number) {
    (this.tableForm.get('columns') as FormArray).removeAt(index);
  } 

  onAddColumn() {
    (this.tableForm.get('columns') as FormArray).push(new FormGroup({
      'name': new FormControl(null, Validators.required),
      'type': new FormControl(null, Validators.required)
    }));
  }

  get columnTypes() {
    var types = [];
    for (let type in ColumnType) {
      types.push(ColumnType[type]);
    }
    return types;
  }

  get columnsControls() {
    return (this.tableForm.get('columns') as FormArray).controls;
  }
}
