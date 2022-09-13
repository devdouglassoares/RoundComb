import { Component,OnInit  } from '@angular/core';
import {io} from 'socket.io-client';

import { WebSocketSubject } from 'rxjs/observable/dom/WebSocketSubject';

export class Message {
  constructor(
      public sender: string,
      public content: string,
      public isBroadcast = false,
  ) { }
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private socket: any;
  public jsonresult: any;
  title = 'socket-client';

  private socket$: WebSocketSubject<Message>;
  public serverMessages = new Array<Message>();
  constructor() {
  // Connect Socket with server URL
  //this.socket = io('http://localhost:3000');
  }
  public ngOnInit(): void {

  
   

    this.socket = new WebSocketSubject('ws://localhost:3000');


    this.socket$
   .subscribe(
       (message) => { 
             console.log(message);
        },
       (err) => console.error(err)
);

 
  }
}