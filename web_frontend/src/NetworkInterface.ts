import { Point } from "./CommonTypes";


export enum EventType {
	MouseMove,
	MouseClick
};

export class NetworkInterface {

	async run() {
		const wsUri = "ws://127.0.0.1/";
		const websocket = new WebSocket(wsUri);	
	}

	public addMouseMoveEvent(delta: Point) {

	}
}

