import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import {
  NgxGalleryOptions,
  NgxGalleryImage,
  NgxGalleryAction,
  NgxGalleryAnimation,
} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  // pagina di visione dei dettagli del singolo utente selezionato

  user: User;
  // ngx gallery
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // recuperiamo i dati dalla route (prima bisogna sottoscriverla)
    this.route.data.subscribe((data) => {
      this.user = data.user;
    });

    // opzioni (array) per settare la galleria delle foto
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];
    // conterrà tutte le immagini caricate
    this.galleryImages = this.getImages();
  }

  // recupero foto
  getImages() {
    const imageUrls = [];
    // forof: permette di scorrere le immagini all'interno dell'oggetto utente
    for (const photo of this.user.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description,
      });
    }
    return imageUrls;
  }

  // il metodo loadUser non é più necessario perché i dati vengono ricevuti
  // dal MemberDetailComponent (fa già la route)

  /* riferimento alla route: members/4 (id esempio)
  loadUser() {
    // il + recupera l'id come stringa e poi lo parsa a numero
    this.userService.getUser(+this.route.snapshot.params.id).subscribe(
      (user: User) => {
        this.user = user;
        // assegnamo la proprietà user (di questo metodo) a quella dichiarata nella classe qua sopra
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }*/
}
