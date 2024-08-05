import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface HoteDetail {
  id: number;
  roomName: string;
  description: string;
  date : string,
  price: number;
  imageUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private apiUrl = 'http://localhost:5026/hotelApi/api';
  private postUrl = 'http://localhost:5079/userdata/userdata';

  constructor(private http: HttpClient) {}

  getRooms(): Observable<HoteDetail[]> {
    return this.http.get<HoteDetail[]>(this.apiUrl);
  }
  addUserData(userDetail: HoteDetail & { name: string, email: string, check: boolean }): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(this.postUrl, userDetail, { headers });
  }
}
