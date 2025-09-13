import { useState } from 'react';
import { flushSync } from 'react-dom';
import { EventType, NetworkInterface } from './NetworkInterface';
import { Point } from './CommonTypes';



export default function InputAreaComponent(props: {networkInterface: NetworkInterface}) {

	let [isPointerDown, setPointerIsDown] = useState<boolean>(false);
	let [lastPointerPosition, setLastPointerPosition] = useState<Point | null>(null);
	let [logText, setLogText] = useState<string>("");
	let [_debugClickCount, _setDebugClickCount] = useState<number>(0);

	let theElement = (
		<div 
			className={`input-area ${isPointerDown ? "pointer-is-captured" : ""}`} 
			onPointerDown={onPointerDown} 
			onPointerUp={onPointerUp} 
			onPointerCancel={onPointerUp} 
			onPointerMove={onCapturedPointerMove} 
			onClick={onClick}
			//onClickCapture={onClick}
		>
			{logText}
		</div>
	);

	function onClick() {
		if (isPointerDown) {
			return;
		}
		setLogText("click" + _debugClickCount.toString());
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

		setLogText(`${ JSON.stringify([mouseDelta, lastPointerPosition, newPos])}`);
		setLastPointerPosition(newPos);
		//e.stopPropagation();
	}

	function onPointerDown(e: React.PointerEvent<HTMLDivElement>) {
		setPointerIsDown(true);
		e.currentTarget.setPointerCapture(0);
		//e.currentTarget.onpointermove = (e) => { onCapturedPointerMove(e) };
		setLastPointerPosition({
			x: e.clientX,
			y: e.clientY
		});
		//e.stopPropagation();
	};

	function onPointerUp(e: React.PointerEvent<HTMLDivElement>) {
		//e.currentTarget.onpointermove = null;
		e.currentTarget.releasePointerCapture(0);
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
