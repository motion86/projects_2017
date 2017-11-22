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

namespace TemplateUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbSource.Drop += DropEventHandler;
            lbDestination.Drop += DropEventHandler;
            lbDestination.MouseDoubleClick += DoubleClickEventHandler;
            btnUpdate.Click += OnUpdateClicked;
            lbSource.KeyDown += DeleteItem;
            lbDestination.KeyDown += DeleteItem;
            btnRepackage.Click += PackageTemplates;
            
            cbResizeImages.Checked += (o, e) =>
            {
                if ((bool)cbResizeImages.IsChecked)
                {
                    lbParams.Content = "Width, OutDir: ";
                }
            };

            tbPublishPath.Text = @"C:\Dev\TFS\VT3000\VRS2 Templates\Published Templates\";
            //lbSource.MouseRightButtonUp += DeleteItem;

            // This line must be called in UI thread to get correct scheduler
            //Tools.Util.Scheduler = System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext();
        }

        private async void PackageTemplates(object sender, RoutedEventArgs e)
        {
            var myTasks = new List<Task<string>>();
            var destinations = lbDestination.Items.Cast<ListBoxItem>().Where(x => x.IsSelected).ToList();

            btnRepackage.IsEnabled = false;

            Tools.Util.DirIdentifier = tbDirId.Text;

            tbOutput.Text = "";

            UpdateOutput("Packging Templates!\n");

            foreach (var dest in destinations)
            {
                var pPath = tbPublishPath.Text;
                var _dest = (string)dest.Tag;

                var result = await Task.Run(() => Tools.Util.PackageTemplate(_dest, pPath));

                UpdateOutput(result);
            }

            UpdateOutput("\nFinished Packaging Templates!\n");

            btnRepackage.IsEnabled = true;
        }

        

        private void DeleteItem(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Delete)
                {
                    var lb = (ListBox)sender;
                    var items = lb.SelectedItems;
                    var toBeRemoved = new List<ListBoxItem>(30);

                    foreach (ListBoxItem item in items)
                        toBeRemoved.Add(item);

                    toBeRemoved.ForEach(x => lb.Items.Remove(x));
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void OnUpdateClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.Util.DirIdentifier = tbDirId.Text;

                var source = lbSource.SelectedItems.Cast<ListBoxItem>().Select(x => (string)x.Tag).AsEnumerable();
                var destination = lbDestination.SelectedItems.Cast<ListBoxItem>().Where(x => (string)x.Content != "<...>").Select(x => (string)x.Tag).AsEnumerable();

                var replace = tbReplace.Text;
                var _with = tbWith.Text;

                tbOutput.Text = "";


                bool copyDlls = (bool)cbDll.IsChecked;
                bool copyAspx = (bool)cbAspx.IsChecked;
                Tools.Util.Update(source, destination, replace, copyDlls, copyAspx, _with, UpdateOutput);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem Occured!");
            }
        }

        void DropEventHandler(object sender, DragEventArgs e)
        {
            try
            {
                bool copyDlls = (bool)cbDll.IsChecked;
                bool copyAspx = (bool)cbAspx.IsChecked;
                Func<string, bool> _CheckExt = (x) => 
                copyDlls || copyAspx ?
                    (copyDlls && copyAspx ? 
                        (x.EndsWith(".cs") || x.EndsWith(".dll")) || x.EndsWith(".aspx") :
                        copyDlls ? 
                            (x.EndsWith(".cs") || x.EndsWith(".dll")) : (x.EndsWith(".cs") || x.EndsWith(".aspx"))) :
                    x.EndsWith(".cs");

                var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop, false)).Where(x => _CheckExt(x) || System.IO.Directory.Exists(x)).ToList();

                filePath.ForEach(x => ((ListBox)sender).Items.Add(new ListBoxItem
                {
                    Content = x.Substring(x.LastIndexOf('\\') + 1),
                    Tag = x,
                    IsSelected = true
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem with drop!");
            }
        }

        void DoubleClickEventHandler(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            try
            {
                var filePath = $"{((ListBoxItem)lb.SelectedItem).Tag}";
                var name = $"{((ListBoxItem)lb.SelectedItem).Content}";

                lb.Items.Clear();

                if (System.IO.Directory.Exists(filePath))
                {
                    if (name == "<...>")
                    {
                        System.IO.Directory.GetDirectories(filePath).ToList().ForEach(x => lb.Items.Add(new ListBoxItem
                        {
                            Content = x.Substring(x.LastIndexOf('\\') + 1),
                            Tag = x
                        }));
                    }
                    else
                    {
                        lb.Items.Add(new ListBoxItem
                        {
                            Content = "...",
                            Tag = filePath.Substring(0, filePath.LastIndexOf('\\')).Substring(0, filePath.LastIndexOf('\\'))
                        });

                        System.IO.Directory.GetDirectories(filePath).ToList().ForEach(x => lb.Items.Add(new ListBoxItem
                        {
                            Content = x.Substring(x.LastIndexOf('\\') + 1),
                            Tag = x
                        }));

                        //System.IO.Directory.GetFiles(filePath).ToList().ForEach(x => lb.Items.Add(new ListBoxItem
                        //{
                        //    Content = x.Substring(x.LastIndexOf('\\') + 1),
                        //    Tag = x
                        //}));
                    }
                }
                else
                {
                    lb.Items.Add(new ListBoxItem
                    {
                        Content = "...",
                        Tag = filePath.Substring(0, filePath.LastIndexOf('\\')).Substring(0, filePath.LastIndexOf('\\'))
                    });
                }
            }
            catch (Exception ex)
            {
                lb.Items.Add(new ListBoxItem
                {
                    Content = "C:\\",
                    Tag = "C:\\"
                });
            }
        }

        public void UpdateOutput(string update)
        {
            tbOutput.Text = $"{tbOutput.Text}\n{update}";
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop, false))
                    .Where(x => System.IO.Directory.Exists(x))
                    .SelectMany(x => System.IO.Directory.GetDirectories(x).Where(y => y.EndsWith(tbDirId.Text))).ToList();

                filePath.ForEach(x => lbDestination.Items.Add(new ListBoxItem
                {
                    Content = x.Substring(x.LastIndexOf('\\') + 1),
                    Tag = x,
                    IsSelected = true
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem with drop!");
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cbResizeImages.IsChecked)
            {
                tbOutput.Text = "";
                var source = lbSource.SelectedItems.Cast<ListBoxItem>().Select(x => (string)x.Tag).AsEnumerable();
                var pars = tbParams.Text.Split(',');
                string width = "", outputDirName = "";

                if (pars.Length > 0)
                {
                    width = pars[0];
                }
                else
                    width = "80";
                if (pars.Length > 1)
                {
                    outputDirName = pars[1];
                }
                else
                    outputDirName = "thumbs";
                
                foreach (var dir in source)
                {
                    Tools.Util.ResizeImages(dir, outputDirName, width, UpdateOutput);
                }
            }
        }
    }
}
