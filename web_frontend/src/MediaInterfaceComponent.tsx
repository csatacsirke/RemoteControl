import { NetworkInterface } from "./NetworkInterface";
import MediaButton from "./MediaButton";

import './MediaInterface.css';

import playIcon from './assets/play-button.svg'
import stopIcon from './assets/stop-button.svg'
import ffIcon from './assets/ff-button.svg'
import rwIcon from './assets/rw-button.svg'

import volumeDownIcon from './assets/volume down icon.svg'
import volumeUpIcon from './assets/volume up icon.svg'
import muteIcon from './assets/mute icon.svg'

import './assets/edited topdown coffee cup.png'



export default function MediaInterfaceComponent(props: {networkInterface: NetworkInterface}) {
	return (
		<>
			<div className="media-interface-container">
				<MediaButton src={rwIcon} onClick={() => props.networkInterface.addMediaCommand("rewind")}></MediaButton>
				<MediaButton src={stopIcon} onClick={() => props.networkInterface.addMediaCommand("stop")}></MediaButton>
				<MediaButton src={playIcon} onClick={() => props.networkInterface.addMediaCommand("play")}></MediaButton>
				<MediaButton src={ffIcon} onClick={() => props.networkInterface.addMediaCommand("fastforward")}></MediaButton>
				<br/>
				
				<img src={volumeDownIcon} className="media-interface-button svg-icon" onClick={() => props.networkInterface.addMediaCommand("volumedown")}></img>
				<img src={muteIcon} className="media-interface-button svg-icon" onClick={() => props.networkInterface.addMediaCommand("volumeup")}></img>
				<img src={volumeUpIcon} className="media-interface-button svg-icon" onClick={() => props.networkInterface.addMediaCommand("volumeup")}></img>
			</div>
		</>
	);
}


