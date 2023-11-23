import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: any[] = [];
  managers: any[] = [];
  selectedManager: string = '';

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadManagers();
    this.loadEmployees();
  }

  loadManagers() {
    this.employeeService.getManagers().subscribe((data) => {
      this.managers = data;
    });
  }

  loadEmployees() {
    if (this.selectedManager) {
      this.employeeService.getEmployeesByManager(this.selectedManager).subscribe((data) => {
        this.employees = data;
      });
    } else {
      this.employeeService.getEmployees().subscribe((data) => {
        this.employees = data;
      });
    }
  }

  onManagerChange() {
    this.loadEmployees();
  }

  deleteEmployee(id: string) {
    this.employeeService.deleteEmployee(id).subscribe(() => {
      this.loadManagers();
      this.loadEmployees();
    });
  }
}

