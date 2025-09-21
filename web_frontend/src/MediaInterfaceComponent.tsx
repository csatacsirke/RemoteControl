import { NetworkInterface } from "./NetworkInterface";
import './MediaInterface.css';

export default function MediaInterfaceComponent(props: {networkInterface: NetworkInterface}) {
	return (
		<>
			<div className="media-interface-container">
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("rewind")}>⏪</span>
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("stop")}>⏹</span>
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("play")}>⏯</span>
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("fastforward")}>⏩</span>
			</div>

			<div className="media-interface-container">
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("volumedown")}>➖</span>
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("mute")}>🔇</span>
				<span className="media-interface-button" onClick={() => props.networkInterface.addMediaCommand("volumeup")}>➕</span>
			</div>
		</>
	);
}


