import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from './table/table.model';
import { TableService } from './table/table.service';

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.css']
})
export class DatabaseComponent implements OnInit {

  isConnected: boolean = true;
  tables: Table[] = [];

  constructor(private router: Router,
              private route: ActivatedRoute,
              private tableService: TableService) { }

  ngOnInit() {
    this.tableService.getTables().subscribe(
      tables => {
        this.tables = tables;
      }
    );
  }

  onAddTable() {
    this.router.navigate(['new'], {relativeTo: this.route});
  }

}
