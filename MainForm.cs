using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Linq;

namespace AntiRecoilApp
{
    public class MainForm : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        private ComboBox weaponSelector;
        private Button toggleButton;
        private Label statusLabel;
        private bool isRecoilActive = false;
        private Thread recoilThread;
        private string selectedWeapon = "";

        private Dictionary<string, PointF> recoilPatterns = new()
        {
            { "COM-18", new PointF(0f, 1.5f) },
            { "Crossvec", new PointF(0f, 2.2f) },
            { "FSP-9", new PointF(0f, 2.0f) },
            { "E-11-SR", new PointF(0f, 2.8f) },
            { "FRMG-0", new PointF(-3f, 8.5f) },
            { "AK", new PointF(0f, 3.7f) },
            { "Shotgun", new PointF(0f, 4.0f) },
            { "Logicer", new PointF(0f, 4.5f) }
        };

        private float accX = 0f, accY = 0f;

        public MainForm()
        {
            Text = "Anti-Recoil SCP:SL";
            Size = new Size(300, 180);
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);

            weaponSelector = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240, Location = new Point(20, 20) };
            weaponSelector.Items.AddRange(recoilPatterns.Keys.Cast<object>().ToArray());
            weaponSelector.SelectedIndexChanged += (s, e) => selectedWeapon = weaponSelector.SelectedItem.ToString();
            Controls.Add(weaponSelector);

            toggleButton = new Button() { Text = "Toggle (F6)", Location = new Point(20, 60), Width = 240, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White };
            toggleButton.Click += (s, e) => ToggleRecoil();
            Controls.Add(toggleButton);

            statusLabel = new Label() { Text = "Status: OFF", Location = new Point(20, 100), Width = 240 };
            Controls.Add(statusLabel);

            recoilThread = new Thread(RecoilLoop) { IsBackground = true };
            recoilThread.Start();
        }

        private void ToggleRecoil()
        {
            isRecoilActive = !isRecoilActive;
            statusLabel.Text = $"Status: {(isRecoilActive ? "ON" : "OFF")}";
        }

        private void RecoilLoop()
        {
            while (true)
            {
                if (isRecoilActive && GetAsyncKeyState(Keys.RButton) < 0 && GetAsyncKeyState(Keys.LButton) < 0 && recoilPatterns.ContainsKey(selectedWeapon))
                {
                    var recoil = recoilPatterns[selectedWeapon];

                    accX += recoil.X;
                    accY += recoil.Y;

                    int moveX = (int)accX;
                    int moveY = (int)accY;

                    accX -= moveX;
                    accY -= moveY;

                    mouse_event(0x0001, moveX, moveY, 0, 0);
                    Thread.Sleep(15);
                }

                if (GetAsyncKeyState(Keys.F6) < 0)
                {
                    Invoke((MethodInvoker)ToggleRecoil);
                    Thread.Sleep(300);
                }

                Thread.Sleep(1);
            }
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
    }
}
