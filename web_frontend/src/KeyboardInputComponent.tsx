import { NetworkInterface } from "./NetworkInterface";

export default function KeyboardInputComponent(props: {networkInterface: NetworkInterface}) {

	function onKey(e: React.KeyboardEvent<HTMLInputElement>) {
		props.networkInterface.addKeyboardEvent(e);
	}

	function onChange(e: React.ChangeEvent<HTMLInputElement>) {
		e.currentTarget.value = "";
	}

	return (
		<>
			<input inputMode="text" placeholder="Type here..." onKeyUp={onKey} onChange={onChange} autoFocus/>
		</>
	);
}