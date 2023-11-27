import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeService } from './employee.service';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeService]
    });

    service = TestBed.inject(EmployeeService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get employees', () => {
    const employees = [{ id: '1', firstName: 'John', lastName: 'Doe' }, { id: '2', firstName: 'Jane', lastName: 'Doe' }];

    service.getEmployees().subscribe(data => {
      expect(data).toEqual(employees);
    });

    const req = httpTestingController.expectOne('/api/employees/list');
    expect(req.request.method).toEqual('GET');
    req.flush(employees);
  });

  it('should get employee by id', () => {
    const employee = { id: '1', firstName: 'John', lastName: 'Doe' };

    service.getEmployee('1').subscribe(data => {
      expect(data).toEqual(employee);
    });

    const req = httpTestingController.expectOne('/api/employees/edit/1');
    expect(req.request.method).toEqual('GET');
    req.flush(employee);
  });

  it('should get managers', () => {
    const managers = [{ id: '1', firstName: 'Manager', lastName: 'One' }, { id: '2', firstName: 'Manager', lastName: 'Two' }];

    service.getManagers().subscribe(data => {
      expect(data).toEqual(managers);
    });

    const req = httpTestingController.expectOne('/api/employees/managers');
    expect(req.request.method).toEqual('GET');
    req.flush(managers);
  });

  it('should handle errors when getting employees', () => {
    const errorMessage = 'Internal Server Error';

    service.getEmployees().subscribe({
      next: () => fail('Expected to fail with an error'),
      error: (error) => {
        expect(error.status).toEqual(500);
        expect(error.error).toEqual(errorMessage);
      }
    });

    const req = httpTestingController.expectOne('/api/employees/list');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });
});
