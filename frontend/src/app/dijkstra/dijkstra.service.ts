import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { env } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class DijkstraService {
  map: BehaviorSubject<{danger: number, tile: number}[][]> = new BehaviorSubject<{danger: number, tile: number}[][]>([]);
  danger: number[][] | null = null;
  tile: number[][] | null = null;
  path: BehaviorSubject<{danger: number, path: {x: number, y: number}[]} | null> = new BehaviorSubject<{danger: number, path: {x: number, y: number}[]} | null>(null);
  error: string = "";

  constructor(private http: HttpClient) {
    this.getDangerMap();
    this.getTileMap();
  }

  getDangerMap() {
    this.http.get<number[][]>(`${env.API_URL}/danger-map`).subscribe((map: number[][]) => {
      this.danger = map;
      this.setMap();
    })
  }

  getTileMap() {
    this.http.get<number[][]>(`${env.API_URL}/tile-map`).subscribe((map: number[][]) => {
      this.tile = map;
      this.setMap();
    })
  }

  getPathLandOnly(x1: number, y1: number, x2: number, y2: number) {
    this.http.get<{danger: number, path: {x: number, y: number}[]}>(`${env.API_URL}/path-only-land?x1=${x1}&y1=${y1}&x2=${x2}&y2=${y2}`).subscribe(path => {
      this.path.next(path);
    })
  }

  getPathLandAndWater(x1: number, y1: number, x2: number, y2: number) {
    this.http.get<{danger: number, path: {x: number, y: number}[]}>(`${env.API_URL}/path-land-and-water?x1=${x1}&y1=${y1}&x2=${x2}&y2=${y2}`).subscribe(path => {
      this.path.next(path);
    })
  }

  setMap() {
    if (!this.danger || !this.tile) return;
    let map = []
    for(let i = 0; i < this.danger.length; i++) {
      let tmp = [];
      for (let e = 0; e < this.danger[i].length; e++) {
        tmp.push({danger: this.danger[i][e], tile: this.tile[i][e]})
      }
      map.push(tmp);
    }
    console.log(map);
    this.map.next(map);
  }
}
