using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteControlPC {
    class CommandProcessor {



        public void Process(string commandJson) {

            try {

                var packet = JsonDocument.Parse(commandJson);
                var root = packet.RootElement;

                var kind = root.GetProperty("kind");


                //root.TryGetProperty("event_data", out JsonElement eventData);




                switch (kind.GetString()) {
                    case "keyboard": 
                    {
                        var eventData = root.GetProperty("event_data");
                        string key = eventData.GetProperty("key").GetString();
                        VirtualKeyboard.SimulateKeypress(key);
                        break;
                    }
                    case "mousemove": 
                    {
                        var eventData = root.GetProperty("event_data");
                        double dx = eventData.GetProperty("dx").GetDouble();
                        double dy = eventData.GetProperty("dy").GetDouble();
                        VirtualKeyboard.SimulateMouseMove(dx, dy);
                        break;
                    }

                    case "click":
                        VirtualKeyboard.SimulateMouseClick();
                        break;
                    case "scroll": 
                    {
                        string[] words = { "TODO" };
                        foreach (var word in words.Skip(1)) {
                            VirtualKeyboard.SimulateScroll(word);
                        }
                    }
                    break;
                    case "spec": 
                    {
                        string[] words = { "TODO" };
                        foreach (var word in words.Skip(1)) {
                            ProcessSpecial(word);
                        }
                        break;
                    }
                }

            } catch (Exception) {
                Debug.Assert(false);
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
