export interface Photo {
  // replichiamo le proprietà che stiamo ritornando al nostro user

  id: number;
  url: string;
  description: string;
  dateAdded: Date;
  isMain: boolean;
}
