import { ComponentFixture, TestBed, tick, fakeAsync } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { SpyLocation } from '@angular/common/testing';
import { of } from 'rxjs';
import { EmployeeFormComponent } from './employee-form.component';
import { EmployeeService } from '../employee.service';
import { RoleService } from '../role.service';
import { RouterTestingModule } from '@angular/router/testing';

describe('EmployeeFormComponent', () => {
  let component: EmployeeFormComponent;
  let fixture: ComponentFixture<EmployeeFormComponent>;
  let employeeService: jasmine.SpyObj<EmployeeService>;
  let roleService: jasmine.SpyObj<RoleService>;
  let router: Router;
  let location: Location;

  beforeEach(() => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getEmployee', 'updateEmployee', 'addEmployee', 'getManagers']);
    const roleServiceSpy = jasmine.createSpyObj('RoleService', ['getRoles']);

    TestBed.configureTestingModule({
      declarations: [EmployeeFormComponent],
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule],
      providers: [
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: { get: () => '123-12-1234' } } } },
        { provide: Router, useValue: { navigate: jasmine.createSpy('navigate') } },
        { provide: Location, useClass: SpyLocation },
        { provide: EmployeeService, useValue: employeeServiceSpy },
        { provide: RoleService, useValue: roleServiceSpy },
      ],
    });

    fixture = TestBed.createComponent(EmployeeFormComponent);
    component = fixture.componentInstance;

    employeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
    roleService = TestBed.inject(RoleService) as jasmine.SpyObj<RoleService>;
    router = TestBed.inject(Router);
    location = TestBed.inject(Location) as SpyLocation;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize when editing an existing employee', fakeAsync(() => {
    const employee = { id: '123-12-1234', firstName: 'John', lastName: 'Doe' };
    employeeService.getEmployee.and.returnValue(of(employee));
    employeeService.getManagers.and.returnValue(of([]));
    roleService.getRoles.and.returnValue(of([]));

    fixture.detectChanges();
    tick();

    expect(component.employee).toEqual(employee);
    expect(component.managers).toEqual([]);
    expect(component.roles).toEqual([]);
    expect(component.hasManagers).toBeFalse();
  }));

  it('should navigate back when goBack is called', () => {
    spyOn(location, 'back');

    component.goBack();

    expect(location.back).toHaveBeenCalled();
  });
});
