import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';
import InputAreaComponent from './InputAreaComponent';
import { ILogger, NetworkInterface } from './NetworkInterface';



function App() {

  //const [networkInterface, ] = useState<NetworkInterface>(new NetworkInterface());

  const [networkLog, setNetworkLog] = useState<string>("");
  
  //networkInterface.logger = networkLogger;

  //const networkInterface = new NetworkInterface(networkLogger);
  const [networkInterface, ] =  useState<NetworkInterface>(() => {
    const networkLogger: ILogger = { 
      logMessage: (message: string) => {
        setNetworkLog(networkLog + "\r\n" + message);
      }
    };
    return new NetworkInterface(networkLogger);
  });
  


  return (
    <div className="App">
      <header className="App-header">
        <InputAreaComponent networkInterface={networkInterface} />       
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
