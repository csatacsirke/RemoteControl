using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RemoteControlPC.Winapi;




namespace RemoteControlPC {



    class VirtualKeyboard {
        enum InputType : uint {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        }

        //// flags
        //const int MOUSEEVENTF_MOVE = 0x1;
        //const int MOUSEEVENTF_WHEEL = 0x0800;

        //const int MOUSEEVENTF_XDOWN = 0x0080;
        //const int MOUSEEVENTF_XUP = 0x0100;

        //// mousedata
        //const int XBUTTON1 = 0x1;
        //const int XBUTTON2 = 0x2;

        public static void SimulateMouseClick() {

            INPUT Input_mouseDown = new INPUT();
            Input_mouseDown.type = (uint)InputType.INPUT_MOUSE; 
            Input_mouseDown.U.mi.dwFlags = MOUSEEVENTF.LEFTDOWN;

            SimulateUserInput(Input_mouseDown);

            INPUT Input_mouseUp = new INPUT();
            Input_mouseUp.type = (uint)InputType.INPUT_MOUSE; 
            Input_mouseUp.U.mi.dwFlags = MOUSEEVENTF.LEFTUP;

            SimulateUserInput(Input_mouseUp);

            //INPUT[] inputs = { Input_mouseDown, Input_mouseUp };

            //SimulateUserInput(inputs);
        }

        public static void SimulateKeypress(string str) {
            SimulateUserInput(TranslateKeyStringToInput(str));
        }

        public static void SimulateScroll(string str) {
            var mouseParams = str.Split(',');
            if (mouseParams.Length < 2) return;

            try {
                float dx = float.Parse(mouseParams[0]);
                float dy = float.Parse(mouseParams[1]);


                INPUT Input = new INPUT();
                Input.type = (uint)InputType.INPUT_MOUSE;


                
                Input.U.mi.mouseData = (UInt32)( dy * 10);
                //Input.U.mi.dx = (int)(dx*120);
                //Input.U.mi.dy = (int)(dy*120);
                Input.U.mi.dwFlags = MOUSEEVENTF.WHEEL;

                SimulateUserInput(Input);
            } catch (Exception) {
                return;
            }
        }

        public static void SimulateMouseMove(double dx, double dy) {
            
            try {
                
                INPUT Input = new INPUT();
                Input.type = (uint)InputType.INPUT_MOUSE; 


                Input.U.mi.dx = (int)(dx);
                Input.U.mi.dy = (int)(dy);
                Input.U.mi.dwFlags = MOUSEEVENTF.MOVE;

                SimulateUserInput(Input);
            } catch (Exception) {
                return;
            }

    
        }


        //// str: mov,-32,32,
        //public static void SimulateMouse(string str) {
        //    var mouseParams = str.Split(',');

        //    if (mouseParams.Length < 3) return;
        //    try {

        //        int dx = int.Parse(mouseParams[1]);
        //        int dy = int.Parse(mouseParams[2]);

        //        INPUT Input = new INPUT();
        //        Input.type = (uint)InputType.INPUT_MOUSE; // 1 = Keyboard Input



        //        Input.U.mi.dx = dx;
        //        Input.U.mi.dy = dy;
        //        Input.U.mi.dwFlags = MOUSEEVENTF.MOVE;
                
        //        SimulateUserInput(Input);
        //    } catch (Exception) {
        //        return;
        //    } 
        
        //}

        //private static INPUT TranslateMouseEventStringToInput(string str) {

        //}

        private static INPUT TranslateKeyStringToInput(string str) {
            INPUT Input = new INPUT();
            Input.type = (uint)InputType.INPUT_KEYBOARD; 

            switch(str.ToUpper()) {
                case "":
                    Input.U.ki.wScan = ScanCodeShort.SPACE;
                    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
                    break;
                case "SPACE":
                    Input.U.ki.wScan = ScanCodeShort.SPACE;
                    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
                    break;
                case "ENTER":
                    Input.U.ki.wScan = ScanCodeShort.RETURN;
                    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
                    break;
                case "BACKSPACE":
                    //Input.U.ki.wScan = ScanCodeShort.BACK;
                    Input.U.ki.wScan = 0;
                    //Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
                    Input.U.ki.dwFlags = KEYEVENTF.UNICODE;
                    Input.U.ki.wVk = VirtualKeyShort.BACK;
                    break;
                default:
                    char firstChar = str.First();
                    Input.U.ki.wScan = (ScanCodeShort)firstChar;
                    Input.U.ki.dwFlags = KEYEVENTF.UNICODE;
                    break;
            }

            //if (str == "SPACE") {
            //    Input.U.ki.wScan = ScanCodeShort.SPACE;
            //    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;

            //} else if (str == "") {
            //    Input.U.ki.wScan = ScanCodeShort.SPACE;
            //    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
            //} else {
            //    char firstChar = str.First();
            //    Input.U.ki.wScan = (ScanCodeShort)firstChar;
            //    Input.U.ki.dwFlags = KEYEVENTF.UNICODE;
            //}

            
            return Input;
            //SimulateKeypress(Input);
        }


        public static void SimulateKeypress(short virtualKey) {
            
            INPUT Input = new INPUT();

            Input.type = 1; // 1 = Keyboard Input
            //Input.U.ki.wScan = ScanCodeShort.KEY_D;
            Input.U.ki.wScan = (ScanCodeShort)virtualKey;
            Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;

            SimulateUserInput(Input);


        }
        
        public static void SimulateUserInput(INPUT[] Inputs) {
            SendInput(1, Inputs, INPUT.Size);
        }

        // TODO lehet batch optimalizálni, lehet hogy így összeakad? -- nem biztos
        public static void SimulateUserInput(INPUT Input) {
            INPUT[] Inputs = new INPUT[1];
            Inputs[0] = Input;


            SendInput(1, Inputs, INPUT.Size);
        }

        public static void Test() {
            SimulateKeypress("Q");
            SimulateKeypress("A");
            SimulateKeypress("q");
            SimulateKeypress("a");
            SimulateKeypress("é");
            SimulateKeypress("ő");
        }
        
        public static void SendInputWithAPI() {
            INPUT[] Inputs = new INPUT[4];
            INPUT Input = new INPUT();

            Input.type = 1; // 1 = Keyboard Input
            Input.U.ki.wScan = ScanCodeShort.KEY_W;
            Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
            Inputs[0] = Input;

            Input.type = 1; // 1 = Keyboard Input
            Input.U.ki.wScan = ScanCodeShort.KEY_S;
            Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
            Inputs[1] = Input;

            Input.type = 1; // 1 = Keyboard Input
            Input.U.ki.wScan = ScanCodeShort.KEY_A;
            Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
            Inputs[2] = Input;

            Input.type = 1; // 1 = Keyboard Input
            Input.U.ki.wScan = ScanCodeShort.KEY_D;
            Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
            Inputs[3] = Input;


            SendInput(4, Inputs, INPUT.Size);
        }

    }
}
