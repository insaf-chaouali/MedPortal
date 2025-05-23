import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AuthentificationService } from '../services/authentification.service';

describe('AuthentificationService', () => {
  let service: AuthentificationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthentificationService]
    });
    service = TestBed.inject(AuthentificationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear(); // cleanup localStorage after each test
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and store user in localStorage', () => {
    const mockCredentials = { login: 'test@example.com', password: 'password123' };
    const mockResponse = { id: 1, email: 'test@example.com', role: 'Patient' };

    service.login(mockCredentials).subscribe(response => {
      expect(response).toEqual(mockResponse);
      expect(localStorage.getItem('user')).toEqual(JSON.stringify(mockResponse));
    });

    const req = httpMock.expectOne('http://localhost:5127/api/Auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should remove user from localStorage on logout', () => {
    localStorage.setItem('user', JSON.stringify({ id: 1, login: 'user' }));
    service.logout();
    expect(localStorage.getItem('user')).toBeNull();
  });

  it('should return false if not logged in', () => {
    expect(service.isLoggedIn()).toBeFalse();
  });

  it('should return user from localStorage if logged in', () => {
    const user = { id: 1, login: 'user' };
    localStorage.setItem('user', JSON.stringify(user));
    expect(service.getCurrentUser()).toEqual(user);
  });
});
