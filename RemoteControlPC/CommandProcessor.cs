using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlPC {
    class CommandProcessor {
        public void Process(string command) {

            var words = command.Split();

            if (words.Length < 1) {
                return;
            }

            

            switch (words.First()) {
                case "keys":
                    foreach(var word in words.Skip(1)) {
                        VirtaulKeyboard.SimulateKeypress(word);
                    }
                    break;
                case "move":
                    foreach (var word in words.Skip(1)) {
                        VirtaulKeyboard.SimulateMouseMove(word);
                    }
                    break;
                case "click":
                    VirtaulKeyboard.SimulateMouseClick();
                    break;
                case "scroll":
                    foreach (var word in words.Skip(1)) {
                        VirtaulKeyboard.SimulateScroll(word);
                    }
                    break;
                case "spec":
                    foreach (var word in words.Skip(1)) {
                        ProcessSpecial(word);
                    }
                    break;
               
            }

            //var command = "m 1 2 3";
            //var result = command.Split();
            //foreach(string str in result) {
            //    Console.Write(str + "|");
            //}

        }

        void ProcessSpecial(String str) {

            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

            var volume = defaultPlaybackDevice.Volume;

            switch (str) {
                case "VOLUME_DOWN":
                    defaultPlaybackDevice.Volume = Math.Max(0, volume - 10);
                    break;
                case "VOLUME_UP":
                    defaultPlaybackDevice.Volume = Math.Min(100, volume + 10);
                    break;
            }
        }
    }
}
