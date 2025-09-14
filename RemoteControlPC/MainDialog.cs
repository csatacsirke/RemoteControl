using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteControlPC {
    public partial class MainDialog : Form {
        public MainDialog() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Thread.Sleep(3000);
            VirtualKeyboard.Test();
        }


        public void ThreadSafeSetNetworkStatusText(string text) {
            Invoke(new Action(() => {
                m_networkStatusLabel.Text = text;
            }));
        }
    }
}
