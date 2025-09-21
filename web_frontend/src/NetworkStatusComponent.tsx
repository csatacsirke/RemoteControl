import { ConnectionState } from "./NetworkInterface";

import './NetworkStatus.css';


export default function MediaInterfaceComponent(props: {connectionState: ConnectionState}) {

	if (props.connectionState == ConnectionState.Disconnected) {
		return (<div className="network-status-bar error">Connection error!</div>);
	}

	if (props.connectionState == ConnectionState.NotConnected) {
		return (<div className="network-status-bar connecting">Connecting...</div>);
	}

	return (
		<>
		</>
	)
}

