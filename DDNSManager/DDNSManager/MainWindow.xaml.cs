using DDNSManager.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DDNSManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Timers.Timer Timer { get; set; }
        bool timerRunning;

        public Util.DDNSTools Updater { get; set; }
        public string InfoPath { get; set; }

        private string[] info { get; set; }

        public string Msg { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            // 3min Tick
            Timer = new System.Timers.Timer(180000);

            timerRunning = false;
            Msg = "Not Running!";

            InfoPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "info.dat");

            Timer.Elapsed += (o, ee) =>
            {
                //var _ip = Util.DDNSTools.GetPublicIP();

                var dnsObj = FileIO.ImportJsonAsObject<Util.DDNSTools>(InfoPath);
                if(dnsObj == null || !CompareDnsObjToForm(dnsObj, info))
                {
                    dnsObj = CreateDnsObj(info);
                }
                else
                {
                    tbCom.Text = dnsObj.ComEmail;
                    tbPass.Password = FileIO.UnscrambleString(dnsObj.Pwd);
                    tbCell.Text = dnsObj.CellNumber;
                    tbMachine.Text = dnsObj.MachineName;
                }


                if (dnsObj.UpdateUser())
                    Msg = $"Notification Sent @ {DateTime.Now.ToLongDateString()}, IP: {dnsObj.IP}";

            };

            // Updates UI with existing settings
            if (UpdateUI())
            {
                btnGetIP.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                lbShowIP.Content = "Auto Started";
                Msg = lbShowIP.Content.ToString();
            }

            var dTimer = new System.Windows.Threading.DispatcherTimer();
            dTimer.Tick += (o, e) => { lbShowIP.Content = Msg; };
            dTimer.Interval = new TimeSpan(0, 0, 30);
            dTimer.Start();
        }

        private DDNSTools CreateDnsObj(string[] pars, string ip = null)
        {
            var dnsObj = new DDNSTools
            {
                ComEmail = pars[0],
                Pwd = pars[1],
                CellNumber = pars[2],
                MachineName = pars[3],
                IP = ip
            };

            dnsObj.WriteObjectToFile(InfoPath);

            return dnsObj;
        }
        private bool CompareDnsObjToForm(DDNSTools obj, string[] pars)
        {
            return (obj?.ComEmail == pars[0] && obj?.Pwd == pars[1] && obj?.CellNumber == pars[2] && obj?.MachineName == pars[3]);
        }

        private void btnGetIP_Click(object sender, RoutedEventArgs e)
        {
            var ip = Util.DDNSTools.GetPublicIP();

            //var updater = new Util.DDNSTools { Username = tbUser.Text, Password = tbPass.Text };

            //var status = updater.UpdatePublicIP(ip);
            if (timerRunning)
            {
                Timer.Stop();
                btnGetIP.Content = "Start";
                timerRunning = false;
            }
            else
            {
                info = new string[]{ tbCom.Text, FileIO.ScrambleString(tbPass.Password), tbCell.Text, tbMachine.Text};

                Timer.Start();
                btnGetIP.Content = "Stop";
                timerRunning = true;
            }
                
            //lbShowIP.Content = $"{ip} -- {(status ? "Good" : "Problem")}";
        }

        private bool UpdateUI()
        {
            var dnsObj = FileIO.ImportJsonAsObject<Util.DDNSTools>(InfoPath);
            if (dnsObj != null)
            {
                tbCom.Text = dnsObj.ComEmail;
                tbPass.Password = FileIO.UnscrambleString(dnsObj.Pwd);
                tbCell.Text = dnsObj.CellNumber;
                tbMachine.Text = dnsObj.MachineName;
                return (tbCom.Text != null && tbPass.Password != null && tbCell.Text != null && tbMachine.Text != null);
            }
            return false;
        }
        
    }
}
