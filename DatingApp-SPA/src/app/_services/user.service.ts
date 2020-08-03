import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl; // vedi environment.ts

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    // viene ritornato a questo link, un arra di users
    return this.http.get<User[]>(this.baseUrl + 'users'); // http://localhost:5000/api/users
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }
}
