import { useState } from 'react';
import logo from './logo.svg';
import './App.css';
import InputAreaComponent from './InputAreaComponent';
import { ConnectionState, ILogger, NetworkInterface } from './NetworkInterface';
import TabView from './TabView';
import KeyboardInputComponent from './KeyboardInputComponent';
import React from 'react';
import MediaInterfaceComponent from './MediaInterfaceComponent';


// TODO config file
//const wsUri = "http://nagygep.local:16666";
const wsUri = "http://192.168.0.171:16666";

function App() {

  //const [networkInterface, ] = useState<NetworkInterface>(new NetworkInterface());

  const [networkLog, setNetworkLog] = useState<string>("");
  const [connectionState, setConnectionState] = useState<ConnectionState>(ConnectionState.NotConnected);

  
  //networkInterface.logger = networkLogger;

  //const networkInterface = new NetworkInterface(networkLogger);
  const [networkInterface, ] =  useState<NetworkInterface>(() => {
    const networkLogger: ILogger = {
      logMessage: (message: string) => {
        setNetworkLog(networkLog + "\r\n" + message);
      },
      onStateChange: function (newState: ConnectionState): void {
        setConnectionState(newState);
      }
    };
    return new NetworkInterface(wsUri, networkLogger);
  });
  


  return (
    <React.StrictMode>
      <div className="App">
        <TabView tabs={[
          
          {
            key: "1",
            displayName: "General",
            contents: (<InputAreaComponent networkInterface={networkInterface} connectionState={connectionState} />)
          },
          {
            key: "3",
            displayName: "Media",
            contents: (<MediaInterfaceComponent networkInterface={networkInterface} />)
          },
          // {
          //   key: "2",
          //   displayName: "Keyboard",
          //   contents: (<KeyboardInputComponent networkInterface={networkInterface} />)
          // },
        ]} />


        <header className="App-header">
          <div style={{display: "none"}}>
            {networkLog}
          </div>
        </header>
      </div>
    </React.StrictMode>
    
  );
}

export default App;
