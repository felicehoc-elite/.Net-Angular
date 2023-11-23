import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../employee.service';
import { RoleService } from '../role.service';
import { Location } from '@angular/common';
import { KeyValue } from '@angular/common';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  employee: any = {
    roleIds: []
  };
  managers: any[] = [];
  roles: any[] = [];
  hasManagers: boolean = false;
  employeeId: any = null;
  apiErrors: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private roleService: RoleService,
    private location: Location
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.employeeId = id;

    if (id) {
      this.employeeService.getEmployee(id).subscribe((data) => {
        this.employee = data;
      });
    }

    this.roleService.getRoles().subscribe((data) => {
      this.roles = data;
    });

    this.employeeService.getManagers().subscribe((managers) => {
      this.managers = managers;
      this.hasManagers = managers.length > 0;
    });
  }

  onSubmit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.apiErrors = [];
    if (id) {
      this.employeeService.updateEmployee(id, this.employee).subscribe(
        () => {
          this.router.navigate(['/employees']);
        },
        (error) => {
          if (error.error && error.error.errors) {
            this.apiErrors = error.error.errors;
          }
        }
      );
    } else {
      this.employeeService.addEmployee(this.employee).subscribe(
        () => {
          this.router.navigate(['/employees']);
        },
        (error) => {
          if (error.error && error.error.errors) {
            this.apiErrors = error.error.errors;
          }
        }
      );
    }
  }

  getApiErrors(): KeyValue<string, string>[] {
    if (this.apiErrors) {
      return Object.entries(this.apiErrors).map(([key, value]) => ({ key, value: value[0] }));
    }
    return [];
  }

  onRoleChange(roleId: number): void {
    const index = this.employee.roleIds.indexOf(roleId);

    if (index === -1) {
      this.employee.roleIds.push(roleId);
    } else {
      this.employee.roleIds.splice(index, 1);
    }
  }

  isManagerSelected(managerId: string | null): boolean {
    return this.employee.managerId === managerId;
  }

  goBack(): void {
    this.location.back();
  }
}
