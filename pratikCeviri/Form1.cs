using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace pratikCeviri 
{
    /*
     Pişmanlık asla kaçamayacağın bir canavar,
     Elleri bazen öldürür, bazen sertçe yakalar.
     Bil ki Sagopa cesaretinin bir kısmını zulada saklar,
     Yanan ışıklarımı kaplayacak kadar karanlığım var.
     */
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        public Form1()
        {
            InitializeComponent();
            string data = File.ReadAllText("romantizma.kits");
            comboBox1.SelectedIndex = Convert.ToInt32(data.Split(',')[0]);
            comboBox2.SelectedIndex = Convert.ToInt32(data.Split(',')[1]);
            //CheckForIllegalCrossThreadCalls = false;
            Thread the = new Thread(new ThreadStart(listenForKeys));
            the.Start();
        }
        bool keysPressed = default(bool);
        Ceviri cvr = default(Ceviri);
        Dictionary<string, string> countryCodes = new Dictionary<string, string>() {
            { "Almanca","de" },
             { "Fransızca","fr" },
              { "Türkçe","tr" },
               { "İngilizce","en" }
        };
        private void listenForKeys()
        {
            for (;;)
            {
                keysPressed = ((GetAsyncKeyState(Keys.F8) != 0) && (ModifierKeys == Keys.Shift));
                if (keysPressed)
                {
                    Invoke((MethodInvoker)delegate {
                        if (Application.OpenForms["Ceviri"] == null)
                        {
                            cvr = new Ceviri();
                            try
                            {
                                switch (comboBox1.SelectedIndex)
                                {
                                    case 0:
                                        cvr.ilkDil = "";
                                        cvr.ikinciDil = countryCodes[comboBox2.Items[comboBox2.SelectedIndex].ToString()];
                                        break;
                                    default:
                                        cvr.ilkDil = countryCodes[comboBox1.Items[comboBox1.SelectedIndex].ToString()];
                                        cvr.ikinciDil = countryCodes[comboBox2.Items[comboBox2.SelectedIndex].ToString()];
                                        if (cvr.ilkDil == cvr.ikinciDil)
                                        {
                                            label1.Text = "Aynı dillerde çeviri yapmak sence saçma değil mi?";
                                            return;
                                        }
                                        break;
                                }
                            }
                            catch (Exception) { }
                            cvr.metin = Clipboard.GetText();
                            cvr.Show();
                        }
                        else
                        {
                            try
                            {
                                switch (comboBox1.SelectedIndex)
                                {
                                    case 0:
                                        cvr.ilkDil = "";
                                        cvr.ikinciDil = countryCodes[comboBox2.Items[comboBox2.SelectedIndex].ToString()];
                                        break;
                                    default:
                                        cvr.ilkDil = countryCodes[comboBox1.Items[comboBox1.SelectedIndex].ToString()];
                                        cvr.ikinciDil = countryCodes[comboBox2.Items[comboBox2.SelectedIndex].ToString()];
                                        if (cvr.ilkDil == cvr.ikinciDil)
                                        {
                                            label1.Text = "Aynı dillerde çeviri yapmak sence saçma değil mi?";
                                            return;
                                        }
                                        break;
                                }
                            }
                            catch (Exception) { }
                            cvr.metin = Clipboard.GetText();
                            cvr.cevir();
                        }
                    });
                }
                Thread.Sleep(1);
            }
        }
     
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        private void gösterGizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = !Visible;
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = "...";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = "...";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText("romantizma.kits", comboBox1.SelectedIndex.ToString() + "," + comboBox2.SelectedIndex.ToString());
            MessageBox.Show("Kaydedildi.","Kayıt");
        }
    }
}
