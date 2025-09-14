import { Point } from "./CommonTypes";


export enum EventType {
	MouseMove,
	MouseClick
};

export interface ILogger {
	logMessage(message: string): void;
};


interface Packet {
	kind: string;
	event_data?: any
};


export class NetworkInterface {

	wsUri = "http://localhost:16666";
	websocket!: WebSocket;
	logger?: ILogger;

	constructor(logger?: ILogger) {
		this.logger = logger;
		this.open();
	}

	open() {
		this.logger?.logMessage("opening connection...");
		this.websocket = new WebSocket(this.wsUri);	
		this.websocket.onmessage = (e: MessageEvent) => {
			//alert(e.data());
			this.logger?.logMessage(e.data().toString());
		};
		this.websocket.onopen = () => {
			this.logger?.logMessage("Connection is now open!");
		};
		this.websocket.onclose = () => {
			this.logger?.logMessage("Connection has been closed!");
		}
		this.websocket.onerror = () => {
			this.logger?.logMessage("Connection has been closed with error!");
		}
		
	}


	sendMessage(message: Packet) {
		if (this.websocket.readyState === this.websocket.OPEN) {
			this.websocket.send(JSON.stringify(message));
		}
	}

	// async run() {
	// 	const wsUri = "ws://127.0.0.1/";
	// 	const websocket = new WebSocket(wsUri);	

		
	// }

	addMouseMoveEvent(delta: Point) {
		this.sendMessage({
			kind: "mousemove",
			event_data: {
				dx: delta.x,
				dy: delta.y
			},
		});
	}

	addClickEvent() {
		this.sendMessage({
			kind: "click",
		});
	}
}

