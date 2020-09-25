import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl; // vedi environment.ts

  constructor(private http: HttpClient) {}

  getUsers(
    page?,
    itemsPerPage?,
    userParams?
  ): Observable<PaginatedResult<User[]>> {
    // conterrà i risultati paginati di tipo User array
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();

    let params = new HttpParams();

    // se esiste la pagina e la pagina contiene almeno un elemento
    if (page != null && itemsPerPage != null) {
      // esempio: http://localhost:5000/api/users?pageNumber=2&pageSize=3
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    // se non é nullo: popoliamo i params con gli user params
    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    // vogliamo ottenere gli user dal body della resposta
    // e le informazioni di paginazione dal HTTPReponse
    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  // path dove verrà modificato lo user
  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user); // http://localhost:5000/api/users/2
  }

  // path per settare la foto come principale
  // http://localhost:5000/api/users/2/photos/15/setMain
  setMainPhoto(userId: number, id: number) {
    return this.http.post(
      this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain',
      {}
    );
  }

  // http://localhost:5000/api/users/2/photos/15
  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
  }
}
