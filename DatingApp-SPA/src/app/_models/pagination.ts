export interface Pagination {
  // replichiamo le informazioni che stiamo ricevendo e che inviamo
  // al pagination header

  // le recupereremo poi nella member-list.component-ts

  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// qui verrranno contenuti i risultati paginati
export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
