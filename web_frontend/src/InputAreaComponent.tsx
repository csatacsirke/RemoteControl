import { useState } from 'react';
import { ConnectionState, NetworkInterface } from './NetworkInterface';
import { Point } from './CommonTypes';
import './App.css';

const MAX_POINTER_MOVE_THAT_COUNTS_AS_CLICK = 10;

export default function InputAreaComponent(props: {networkInterface: NetworkInterface, connectionState: ConnectionState}) {

	let [isPointerDown, setPointerIsDown] = useState<boolean>(false);
	let [lastPointerPosition, setLastPointerPosition] = useState<Point | null>(null);
	let [cumulativeMoveSincePointerDown, setCumulativeMoveSincePointerDown] = useState<number>(0);
	let [needTouchHintLogo, setNeedTouchHintLogo] = useState<boolean>(true);

	
	let [isDebugMode, _setDebugMode] = useState<boolean>(true);
	let [_debugLogText, _debugSetLogText] = useState<string>("");
	let [_debugClickCount, _setDebugClickCount] = useState<number>(0);


	let classes = new Array<string>();
	if (needTouchHintLogo) {
		classes.push("touch-screen-hint-logo");
	}

	if (isPointerDown && props.connectionState == ConnectionState.Connected) {
		classes.push("pointer-is-captured");
	}

	if (props.connectionState == ConnectionState.Connected) {
		classes.push("client-is-connected");
	} 

	if (props.connectionState == ConnectionState.Disconnected) {
		classes.push("network-error");
	}


	let theElement = (
		<div 
			className={`input-area ${classes.join(" ")}`} 
			onPointerDown={onPointerDown} 
			onPointerUp={onPointerUp} 
			onPointerCancel={onPointerUp} 
			onPointerMove={onCapturedPointerMove} 
			onClick={onClick}
			//onClickCapture={onClick}
		>
			{isDebugMode ? _debugLogText : ""}
		</div>
	);

	function onClick() {
		if (isPointerDown || cumulativeMoveSincePointerDown > MAX_POINTER_MOVE_THAT_COUNTS_AS_CLICK) {
			return;
		}
		props.networkInterface.addClickEvent();

		_debugSetLogText("click" + _debugClickCount.toString());
		_setDebugClickCount(_debugClickCount + 1);
	}

	//function onCapturedPointerMove(e: PointerEvent) {
	function onCapturedPointerMove(e: React.PointerEvent<HTMLDivElement>) {
		if (!isPointerDown) {
			return;
		}
		const newPos = {
			x: e.clientX,
			y: e.clientY
		};

		const mouseDelta = lastPointerPosition ?  
			{x: newPos.x - lastPointerPosition.x, y: newPos.y - lastPointerPosition.y} :
			{x: 0, y: 0};

			
		props.networkInterface.addMouseMoveEvent(mouseDelta);

		_debugSetLogText(`${ JSON.stringify([mouseDelta, lastPointerPosition, newPos, cumulativeMoveSincePointerDown])}`);
		setLastPointerPosition(newPos);
		setCumulativeMoveSincePointerDown(cumulativeMoveSincePointerDown + Math.hypot(mouseDelta.x, mouseDelta.y));
		setNeedTouchHintLogo(false);// csak az elejen legyen
		//e.stopPropagation();
	}

	function onPointerDown(e: React.PointerEvent<HTMLDivElement>) {
		setPointerIsDown(true);
		e.currentTarget.setPointerCapture(e.pointerId);
		//e.currentTarget.onpointermove = (e) => { onCapturedPointerMove(e) };
		setLastPointerPosition({
			x: e.clientX,
			y: e.clientY
		});
		
		setCumulativeMoveSincePointerDown(0);
		//e.stopPropagation();
	};

	function onPointerUp(e: React.PointerEvent<HTMLDivElement>) {
		//e.currentTarget.onpointermove = null;
		e.currentTarget.releasePointerCapture(e.pointerId);
		setPointerIsDown(false);
		setLastPointerPosition(null);

		e.stopPropagation();
		e.preventDefault();
	};

	

	return (
		<>
			{ theElement }
		</>
	)
}
