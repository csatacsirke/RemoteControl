import React, { useContext, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import InputAreaComponent from './InputAreaComponent';
import { ConnectionState as ConnectionState, ILogger, NetworkInterface } from './NetworkInterface';
import { createContext } from 'vm';


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
    <div className="App">
      <header className="App-header">
        <InputAreaComponent networkInterface={networkInterface} connectionState={connectionState} />       
        <div>
          {networkLog}
        </div>
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
