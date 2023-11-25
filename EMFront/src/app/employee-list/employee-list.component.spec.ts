import { ComponentFixture, TestBed, tick, fakeAsync } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { EmployeeListComponent } from './employee-list.component';
import { EmployeeService } from '../employee.service';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';

describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;
  let employeeService: jasmine.SpyObj<EmployeeService>;
  let router: Router;

  beforeEach(() => {
    const employeeServiceSpy = jasmine.createSpyObj('EmployeeService',
      ['getEmployees', 'getManagers', 'getEmployeesByManager', 'deleteEmployee']);

    TestBed.configureTestingModule({
      declarations: [EmployeeListComponent],
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule],
      providers: [
        { provide: EmployeeService, useValue: employeeServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '123-45-6789',
              },
            },
          },
        },
        {
          provide: Router,
          useClass: class {
            navigate = jasmine.createSpy('navigate');
          },
        },
      ],
    });

    fixture = TestBed.createComponent(EmployeeListComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load managers on init', () => {
    const managers = [
      { id: '123-12-1234', firstName: 'Manager', lastName: 'One' },
      { id: '123-12-1235', firstName: 'Manager', lastName: 'Two' }
    ];
    employeeService.getManagers.and.returnValue(of(managers));

    fixture.detectChanges();

    expect(component.managers).toEqual(managers);
  });

  it('should load employees by manager when a manager is selected', fakeAsync(() => {
    const employees = [
      { id: '123-12-1234', firstName: 'John', lastName: 'Doe' },
      { id: '123-12-1235', firstName: 'Jane', lastName: 'Doe' }
    ];
    employeeService.getEmployeesByManager.and.returnValue(of(employees));

    component.selectedManager = '123-12-1234';
    component.onManagerChange();

    tick();

    expect(component.employees).toEqual(employees);
  }));
});
