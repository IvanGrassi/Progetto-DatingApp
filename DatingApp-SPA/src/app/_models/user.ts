import { Photo } from './photo';

export interface User {
  // replichiamo le proprietà che stiamo ritornando al nostro user

  id: number;
  username: string;
  knownAs: string;
  age: number;
  gender: string;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string; // ? rende opzionale il campo
  introduction: string;
  lookingFor?: string;
  photos?: Photo[];
}
