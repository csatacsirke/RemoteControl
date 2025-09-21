import { useState } from 'react';
import './App.css';
import './TabView.css';


export interface ITabViewElement {
	key: string,
	displayName: string,
	contents: React.ReactElement,
}

export default function TabView(props: { tabs: ITabViewElement[] }) {

	const tabs = props.tabs;

	let [selectedTabKey, setSelectedTabKey] = useState<string>(tabs[0].key);


	const root = (
		<div className="tab-view">
			<span className="tab-view-header">
				{
					tabs.map((tab: ITabViewElement) => {
						let classes = ['tab-header-element'];
						if (selectedTabKey === tab.key) {
							classes.push("selected-tab")
						}
						return (<span className={classes.join(" ")} key={tab.key} onClick={() =>setSelectedTabKey(tab.key)} >{tab.displayName}</span>);
					})
				}
			</span>
			<span className='tab-view-content-wrapper'>
				{
					tabs.map((tab: ITabViewElement) => {
						if (selectedTabKey === tab.key) {
							return (<span className='tab-content-wrapper' key={tab.key}>{tab.contents}</span>);
						}
						return null;
					})
				}
			</span>
		</div>
	)




	return root;
}