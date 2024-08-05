import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeService, HoteDetail } from './home.service';

declare var bootstrap: any;


@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  hotelRooms: HoteDetail = {
    id: 0,
    roomName: "",
    description: "",
    date: "",
    price: 0,
    imageUrl: ""
  };


  response: HoteDetail | undefined;
  rooms: HoteDetail[] = [];
  filteredRooms: HoteDetail[] = [];
  selectedDate: string = '';
  selectedRoomType: string = '';
  userData= {
    name : "", 
    email : "", 
    check : false, 
  }
selectedRoom : HoteDetail | undefined;
  constructor(private hotelService: HomeService, private chr: ChangeDetectorRef , private el: ElementRef) {}


  ngAfterViewInit(): void {
    const toastTrigger = this.el.nativeElement.querySelector('#liveToastBtn');
    const toastLiveExample = this.el.nativeElement.querySelector('#liveToast');

    if (toastTrigger) {
      const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample);
      toastTrigger.addEventListener('click', () => {
        toastBootstrap.show();
      });
    }
  }


  ngOnInit(): void {
    this.hotelService.getRooms().subscribe((room) => {
      this.rooms = room;
      this.filteredRooms = room;
      this.chr.detectChanges(); // Ensure changes are detected
    });
  }

  getImageSrc(imageUrl: string): string {
    console.log('imageUrl:', imageUrl);
    if (imageUrl.startsWith('data:image')) {
      return imageUrl;
    } else if (imageUrl.startsWith('http')) {
      return imageUrl;
    } else {
      return `data:image/jpg;base64,${imageUrl}`;
    }
  }
  

  filterRooms(): void {
    this.filteredRooms = this.rooms.filter(room => {
      const matchesDate = this.selectedDate ? room.date === this.selectedDate : true;
      const matchesRoomType = this.selectedRoomType ? room.roomName === this.selectedRoomType : true;
      return matchesDate && matchesRoomType;
    });
  }


  openModal(room: HoteDetail): void {
    this.selectedRoom = room;
  }

  submitForm(): void {
    console.log('User Data:', this.userData); // Debugging output

    if (this.selectedRoom) {
      const payload = {
        ...this.selectedRoom,
        ...this.userData
      };
      console.log(this.userData);
      // console.log('Payload:', payload); 

      this.hotelService.addUserData(payload).subscribe(
        response => {
          console.log('Data submitted successfully:', response);
          // Optionally handle success, reset form, etc.
        },
        error => {
          console.error('Error submitting data:', error);
          // Optionally handle error
        }
      );
    }
  }
}
