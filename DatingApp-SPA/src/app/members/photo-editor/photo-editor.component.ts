import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})

// qui la gestione delle immagini (cambio foto principale, eliminazione) viene gestita
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[]; // riceve in input l'array contenente tutte le foto legate ad uno user
  @Output() getMemberPhotoChange = new EventEmitter<string>();

  // codice recuperato dalla documentazione di ng2-file upload
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;
  currentMain: Photo; // attuale foto principale

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService
  ) {
    this.initializeUploader();
  }

  ngOnInit() {}

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  // permette di avviare la procedura di upload foto basata su ng2-file upload
  initializeUploader() {
    // esempio http://localhost:5000/api/users/2/photos/15
    this.uploader = new FileUploader({
      url:
        this.baseUrl +
        'users/' +
        this.authService.decodedToken.nameid +
        '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'), // usa il token con cui é stato fatto il login
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // dimensione massima: 10 Mb
    });

    // permette di evitare l'errore CORS che si presentava nella console
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    // dopo aver caricato correttamente una foto
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response); // json.parse converte la stringa in oggetto
        // costruiamo un oggetto foto popolato sulla base della risposta del server
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain,
        };
        // la foto viene inserita nell'array di foto
        this.photos.push(photo);

        // se é la foto che viene caricata, é la PRIMA foto presente sul profilo:
        // allora questa diventerà anche la foto principale e dovrà essere visualizzata

        if (photo.isMain) {
          // permette di cambiare la foto principale dello user
          this.authService.changeMemberPhoto(photo.url);
          // permette di evutare il cambio di foto se aggiorno la pagina
          this.authService.currentUser.photoUrl = photo.url;
          // sovrascrive l'oggetto user all'interno del localstorage con la nuova immagine
          localStorage.setItem(
            'user',
            JSON.stringify(this.authService.currentUser)
          );
        }
      }
    };
  }

  setMainPhoto(photo: Photo) {
    // trovare qual'é la foto principale, settarla a folse e settare a true la nuova foto scelta come main
    this.userService
      .setMainPhoto(this.authService.decodedToken.nameid, photo.id)
      .subscribe(
        () => {
          // la funzione filtro dell array ritorna una copia dell'array delle foto, filtrando tutto il resto
          this.currentMain = this.photos.filter((p) => p.isMain === true)[0]; // array di 1 elemento (solo la foto principale)
          this.currentMain.isMain = false;
          photo.isMain = true;
          // permette di cambiare la foto principale dello user
          this.authService.changeMemberPhoto(photo.url);
          // permette di evutare il cambio di foto se aggiorno la pagina
          this.authService.currentUser.photoUrl = photo.url;
          // sovrascrive l'oggetto user all'interno del localstorage con la nuova immagine
          localStorage.setItem(
            'user',
            JSON.stringify(this.authService.currentUser)
          );
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }

  deletePhoto(id: number) {
    this.alertify.confirm(
      'Are you sure yout want to delete this photo?',
      () => {
        this.userService
          .deletePhoto(this.authService.decodedToken.nameid, id)
          .subscribe(
            () => {
              // rimozione foto dall'array di foto (usando splice), verifica che l'id passato sia uguale a quello del token
              this.photos.splice(
                this.photos.findIndex((p) => p.id === id),
                1
              ); // operazione di delete specificata qui
              this.alertify.success('Photo has been deleted');
            },
            (error) => {
              this.alertify.error('Failed to delete the photo');
            }
          );
      }
    );
  }
}
