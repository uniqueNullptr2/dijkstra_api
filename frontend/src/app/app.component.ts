import { Component } from '@angular/core';
import { DijkstraService } from './dijkstra/dijkstra.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'frontend';

  constructor(public dijkstraService: DijkstraService) {

  }

  flagLandOnly=true;

  modeToggle() {
    this.flagLandOnly = !this.flagLandOnly;
  }
}
