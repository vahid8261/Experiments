import {Component, OnInit} from '@angular/core';
import {Observable} from 'rxjs/Observable';

import {ChannelService, ConnectionState} from './services/channel.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

    // An internal "copy" of the connection state stream used because
    //  we want to map the values of the original stream. If we didn't
    //  need to do that then we could use the service's observable
    //  right in the template.
    //
    connectionState$: Observable<string>;

    constructor(
        private channelService: ChannelService
    ) {

        // Let's wire up to the signalr observables
        //
        this.connectionState$ = this.channelService.connectionState$
            .map((state: ConnectionState) => ConnectionState[state]);

        this.channelService.error$.subscribe(
            (error: any) => { console.warn(error); },
            (error: any) => { console.error('errors$ error', error); }
        );

        // Wire up a handler for the starting$ observable to log the
        //  success/fail result
        //
        this.channelService.starting$.subscribe(
            () => { console.log('signalr service has been started'); },
            () => { console.warn('signalr service failed to start!'); }
        );
    }

    ngOnInit() {
        // Start the connection up!
        //
        console.log('Starting the channel service');

        this.channelService.start();
    }
}
