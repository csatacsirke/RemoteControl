import { NetworkInterface } from "./NetworkInterface";

export default function KeyboardInputComponent(props: {networkInterface: NetworkInterface, autofocus?: boolean}) {

	function onKey(e: React.KeyboardEvent<HTMLInputElement>) {
		props.networkInterface.addKeyboardEvent(e);
	}

	function onChange(e: React.ChangeEvent<HTMLInputElement>) {
		e.currentTarget.value = "";
	}

	const autoFocus = props.autofocus ? true : undefined;

	return (
		<>
			<div>
				<input className="keyboard-input" placeholder="Type here..." type="text" autoCorrect="off" autoCapitalize="off" onKeyUp={onKey} onChange={onChange} autoFocus={autoFocus}/>
			</div>
		</>
	);
}