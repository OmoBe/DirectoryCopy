using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace CopyDirectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private FolderBrowserDialog fdb, fdb1;
        List<DirectoryInfo> ldi = new List<DirectoryInfo>();
        List<FileInfo> lfi = new List<FileInfo>();

        public MainWindow()
        {
            //btnCopyFiles.IsEnabled = false;
        }


        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            //openFileDialog1.Multiselect = true;
            //openFileDialog1.ShowDialog();
            lstSelectedFolder.Items.Clear();
            using (this.fdb = new System.Windows.Forms.FolderBrowserDialog())
            {
               
                if (fdb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblPath.Content = "Current path: " + fdb.SelectedPath;
                    DirectoryInfo di = new DirectoryInfo(fdb.SelectedPath);

                    foreach (DirectoryInfo _di in di.GetDirectories())
                        lstSelectedFolder.Items.Add(_di);

                    foreach (FileInfo _fi in di.GetFiles())
                        lstSelectedFolder.Items.Add(_fi);
                }
            }

        }

        private void btnSelectCopyTo_Click(object sender, RoutedEventArgs e)
        {
            using (this.fdb1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (fdb1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblPathCopyTo.Content = "Copy files to: " + fdb1.SelectedPath;
                    btnCopyFiles.IsEnabled = true;
                }

            }
        }

        private async void btnCopyFiles_Click(object sender, RoutedEventArgs e)
        {
            //Make sure user has selected files or directories from a directory that exists
            if (lstSelectedFolder.SelectedItems.Count < 1 || !Directory.Exists(fdb.SelectedPath))
            {
                System.Windows.MessageBox.Show("Please select some files to copy");
                return;
            }

            foreach (var _item in lstSelectedFolder.SelectedItems)
            {
                //check type of selected item
                if (_item is FileInfo)
                    lfi.Add((FileInfo)_item);
                else
                    ldi.Add((DirectoryInfo)_item);
            }

            var progressIndicator = new Progress<string>(ReportProgress);

            //Copy each individual file to the target directory
            if (lfi.Count > 0)
                foreach (FileInfo fi in lfi)
                    await copySystem.IO.CopyFile(fi, progressIndicator, fdb1.SelectedPath);


            //Copy the selected directory plus any subdirectories and files
            if (ldi.Count > 0)
                foreach (DirectoryInfo di in ldi)
                    await copySystem.IO.CopyDirectory(di, progressIndicator, fdb1.SelectedPath);

            
            lblPathCopyTo.Content = "";
        }

        void ReportProgress(string value)
        {
            //Update the UI to reflect the progress string that is passed back
            lblPathCopyTo.Content = value;
        }
    }
}
