using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FarManager
{
    public partial class MainWindow : Window
    {
        public DirectoryInfo dInfo { get; set; }
        public DirectoryInfo[] directories { get; set; }
        public FileInfo[] fInfo { get; set; }
        public string[] dir { get; set; }

        public long sizeFolders { get; set; }
        public int countFiles { get; set; }
        public int countFolders { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            cbxfirst.Items.Add(@"C:\");
            cbxfirst.Items.Add(@"D:\");

            cbxsecond.Items.Add(@"C:\");
            cbxsecond.Items.Add(@"D:\");
        }


        private void FillLitsbox(ListBox listBox)
        {
            directories = dInfo.GetDirectories();
            listBox.Items.Add("<<<<<<<<<<<<<<<<< Folders >>>>>>>>>>>>>>>>>");
            foreach (var dr in directories)
            {
                listBox.Items.Add(dr);
            }

            fInfo = dInfo.GetFiles();
            listBox.Items.Add("<<<<<<<<<<<<<<<<<< Files >>>>>>>>>>>>>>>>>>");
            foreach (var fl in fInfo)
            {
                listBox.Items.Add(fl);
            }
        }

        private void Back(TextBox textBox, ListBox listBox)
        {
            var temp = textBox.Text;
            int startindex = temp.LastIndexOf(@"\");
            int count = temp.Length - startindex;
            int countsymbol = temp.Count(c => c == '\\');



            if (countsymbol == 1)
            {
                dir = Directory.GetFiles(temp.Remove(startindex + 1, count - 1));
                dInfo = new DirectoryInfo(temp.Remove(startindex + 1, count - 1));
                textBox.Text = temp.Remove(startindex + 1, count - 1);
            }
            else
            {
                dir = Directory.GetFiles(temp.Remove(startindex, count));
                dInfo = new DirectoryInfo(temp.Remove(startindex, count));
                textBox.Text = temp.Remove(startindex, count);
            }
            listBox.Items.Clear();
            FillLitsbox(listBox);
        }

        private string SizeFormat(long _size)
        {
            if (_size >= 1073741824) // GB
            {
                _size /= 1073741824;
                return _size.ToString("0.00") + " GB";
            }
            else if (_size < 1073741824 && _size >= 1048576) // MB
            {
                _size /= 1048576;
                return _size.ToString("0.00") + " MB";
            }
            else if (_size < 1048576 && _size >= 1024) // KB
            {
                _size /= 1024;
                return _size.ToString("0.00") + " KB";
            }
            else if (_size < 1024) // bytes
            {
                return _size.ToString("0.00") + " bytes";
            }

            return null;
        }

        public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }


        private void cbxfirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txbfirst_path.Text = cbxfirst.SelectedItem.ToString();
            listbox_first.Items.Clear();

            dir = Directory.GetFiles($"{cbxfirst.SelectedItem.ToString()}");
            dInfo = new DirectoryInfo($"{cbxfirst.SelectedItem.ToString()}");

            FillLitsbox(listbox_first);
        }

        private void listbox_first_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_first.SelectedItem is DirectoryInfo)
            {
                txbfirst_path.Text = (listbox_first.SelectedItem as DirectoryInfo).FullName;
                dir = Directory.GetFiles((listbox_first.SelectedItem as DirectoryInfo).FullName);
                dInfo = new DirectoryInfo((listbox_first.SelectedItem as DirectoryInfo).FullName);


                listbox_first.Items.Clear();
                FillLitsbox(listbox_first);
            }
        }

        private void btn_backfirst_Click(object sender, RoutedEventArgs e)
        {
            Back(txbfirst_path, listbox_first);
        }


        private void cbxsecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txbsecond_path.Text = cbxsecond.SelectedItem.ToString();
            listbox_second.Items.Clear();

            dir = Directory.GetFiles($"{cbxsecond.SelectedItem.ToString()}");
            dInfo = new DirectoryInfo($"{cbxsecond.SelectedItem.ToString()}");

            FillLitsbox(listbox_second);
        }

        private void listbox_second_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listbox_second.SelectedItem is DirectoryInfo)
            {
                txbsecond_path.Text = (listbox_second.SelectedItem as DirectoryInfo).FullName;
                dir = Directory.GetFiles((listbox_second.SelectedItem as DirectoryInfo).FullName);
                dInfo = new DirectoryInfo((listbox_second.SelectedItem as DirectoryInfo).FullName);


                listbox_second.Items.Clear();
                FillLitsbox(listbox_second);
            }
        }

        private void btn_backsecond_Click(object sender, RoutedEventArgs e)
        {
            Back(txbsecond_path, listbox_second);
        }


        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (listbox_first.SelectedIndex != -1)
            {
                try
                {
                    if (listbox_first.SelectedItem is DirectoryInfo && Directory.Exists((listbox_first.SelectedItem as DirectoryInfo).FullName))
                    {
                        (listbox_first.SelectedItem as DirectoryInfo).Delete(true);
                    }
                    else if (listbox_first.SelectedItem is FileInfo && File.Exists((listbox_first.SelectedItem as FileInfo).FullName))
                    {
                        (listbox_first.SelectedItem as FileInfo).Delete();
                    }
                    await Task.Delay(3000);
                    MessageBox.Show("Delete Done!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selected First Listbox!");
            }
            

        }

        private async void btn_move_Click(object sender, RoutedEventArgs e)
        {
            if (listbox_first.SelectedIndex != -1)
            {
                try
                {
                    if (listbox_first.SelectedItem is FileInfo)
                    {
                        File.Move((listbox_first.SelectedItem as FileInfo).FullName, txbsecond_path.Text + "\\" + (listbox_first.SelectedItem as FileInfo).Name);
                    }
                    else if (listbox_first.SelectedItem is DirectoryInfo)
                    {
                        Directory.Move((listbox_first.SelectedItem as DirectoryInfo).FullName, txbsecond_path.Text + "\\" + (listbox_first.SelectedItem as DirectoryInfo).Name);
                    }
                    await Task.Delay(3000);
                    MessageBox.Show("Move Success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selected First Listbox!");
            }

            
        }

        private async void btn_properties_Click(object sender, RoutedEventArgs e)
        {

            if (listbox_first.SelectedIndex != -1)
            {
                try
                {
                    if (listbox_first.SelectedItem is DirectoryInfo)
                    {
                        var dir = listbox_first.SelectedItem as DirectoryInfo;
                        sizeFolders = await Task.Run(() => dir.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length));
                        countFiles = await Task.Run(() => dir.EnumerateFiles("*", SearchOption.AllDirectories).Count());
                        countFolders = await Task.Run(() => dir.EnumerateDirectories("*", SearchOption.AllDirectories).Count());


                        string proporties = $@"
Folder name:   {dir.Name}
Type:               File folder
Location:         {dir.Root.FullName}
Size:                {SizeFormat(sizeFolders)} ({(double)sizeFolders} bytes) 
Contains:        {countFiles} Files, {countFolders} Folders
Created:          {dir.CreationTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")}";
                        MessageBox.Show(proporties, dir.Name + " Proporties", MessageBoxButton.OK);
                    }
                    else if (listbox_first.SelectedItem is FileInfo)
                    {
                        var file = listbox_first.SelectedItem as FileInfo;
                        string proporties = $@"
File name:       {file.Name}
Type of file:      {file.Extension}
Location:         {System.IO.Path.GetDirectoryName(file.FullName)}
Size:                {SizeFormat(file.Length)} ({(double)file.Length} bytes) 
Created:          {file.CreationTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")}
Modified:         {file.LastWriteTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")}
Accessed:         {file.LastAccessTime.ToString("dddd, dd MMMM yyyy HH:mm:ss")}";
                        MessageBox.Show(proporties, file.Name + " Proporties", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selected First Listbox!");
            }
            
        }

        private async  void btn_copy_Click(object sender, RoutedEventArgs e)
        {

            if (listbox_first.SelectedIndex != -1)
            {
                try
                {
                    if (listbox_first.SelectedItem is FileInfo)
                    {
                        string sourcePath = (listbox_first.SelectedItem as FileInfo).FullName;
                        string targetPath = txbsecond_path.Text + "\\" + (listbox_first.SelectedItem as FileInfo).Name;

                        File.Copy(sourcePath, targetPath, true);
                    }
                    else if (listbox_first.SelectedItem is DirectoryInfo)
                    {
                        string sourcePath = (listbox_first.SelectedItem as DirectoryInfo).FullName;
                        string targetPath = txbsecond_path.Text + "\\" + (listbox_first.SelectedItem as DirectoryInfo).Name;
                        CopyFolder(sourcePath, targetPath);
                    }
                    await Task.Delay(3000);
                    MessageBox.Show("Copy Success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selected First Listbox!");
            }
            
        }

        private void btn_refreshfirst_Click(object sender, RoutedEventArgs e)
        {
            listbox_first.Items.Clear();
            dir = Directory.GetFiles(txbfirst_path.Text);
            dInfo = new DirectoryInfo(txbfirst_path.Text);

            FillLitsbox(listbox_first);
        }

        private void btn_refreshsecond_Click(object sender, RoutedEventArgs e)
        {
            listbox_second.Items.Clear();
            dir = Directory.GetFiles(txbsecond_path.Text);
            dInfo = new DirectoryInfo(txbsecond_path.Text);

            FillLitsbox(listbox_second);
        }

       
    }
}
