import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RoleService } from './role.service';

describe('RoleService', () => {
  let service: RoleService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RoleService]
    });

    service = TestBed.inject(RoleService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get roles', () => {
    const roles = [{ id: '1', name: 'Role1' }, { id: '2', name: 'Role2' }];

    service.getRoles().subscribe(data => {
      expect(data).toEqual(roles);
    });

    const req = httpTestingController.expectOne('/api/roles');
    expect(req.request.method).toEqual('GET');
    req.flush(roles);
  });

  it('should add roles', () => {
    const newRoles = [1, 2, 3];
    const successMessage = 'Roles added successfully';

    service.addRoles(newRoles).subscribe(message => {
      expect(message).toEqual(successMessage);
    });

    const req = httpTestingController.expectOne('/api/roles');
    expect(req.request.method).toEqual('POST');
    req.flush(successMessage);
  });

  it('should handle errors when getting roles', () => {
    const errorMessage = 'Internal Server Error';

    service.getRoles().subscribe({
      next: () => fail('Expected to fail with an error'),
      error: (error) => {
        expect(error.status).toEqual(500);
        expect(error.error).toEqual(errorMessage);
      }
    });

    const req = httpTestingController.expectOne('/api/roles');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });
});
