
import mediaIconBackground from './assets/media-icon-background.png'

import './MediaButton.css'




export default function MediaButton(props: {src: string, onClick: React.MouseEventHandler}) {
	return (
		<>
			<img src={props.src} className="media-button" onClick={props.onClick}></img>
			{/* <span className="media-button">asdf</span> */}
		</>
	);
}
