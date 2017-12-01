using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checksumator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileInfoEx _file;
        ObservableCollection<FileInfoEx> _files = new ObservableCollection<FileInfoEx>();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI Logic Single
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog() { CheckFileExists = false };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                if (ValidateInput(ofd.FileName))
                {
                    _file = new FileInfoEx(ofd.FileName);

                    tbFile.Text = _file.FullName;
                    gbAlgorithmSingle.IsEnabled = true;
                    gbResult.IsEnabled = true;

                    HashFile(_file,gAlgorithmSingle);
                    tbResult.Text = _file.Hash.Hash;
                }
                else
                {
                    MessageBox.Show("File " + ofd.FileName + " does not exist. Please select a valid file.", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    gbAlgorithmSingle.IsEnabled = false;
                    gbResult.IsEnabled = false;
                }
            }
        }

        private void tbFile_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidateInput(tbFile.Text))
            {
                _file = new FileInfoEx(tbFile.Text);

                gbAlgorithmSingle.IsEnabled = true;
                gbResult.IsEnabled = true;

                HashFile(_file, gAlgorithmSingle);
                tbResult.Text = _file.Hash.Hash;
            }
            else
            {
                gbAlgorithmSingle.IsEnabled = false;
                gbResult.IsEnabled = false;

                if (string.IsNullOrWhiteSpace(tbFile.Text))
                    return;

                MessageBox.Show("File " + tbFile.Text + " does not exist. Please select a valid file.", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
 
            }
        }

        private void rbAlgorithm_Changed(object sender, RoutedEventArgs e)
        {
            if (_file != null)
            {
                RadioButton rbAlgorithm = (RadioButton)sender;

                if (rbAlgorithm.IsChecked.Value)
                {
                    HashFile(_file, gAlgorithmSingle);
                    tbResult.Text = _file.Hash.Hash;
                }
            }
        }
        #endregion

        #region UI Logic Multiple
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lvFiles.ItemsSource = _files;
        }

        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog() { CheckFileExists = false , Multiselect = true };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string filename in ofd.FileNames)
                {
                    _files.Add(new FileInfoEx(filename));
                }
            }
        }

        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string filename in Directory.GetFiles(fbd.SelectedPath))
                {
                    _files.Add(new FileInfoEx(filename));
                }
            }
        }

        private void btnClrAll_Click(object sender, RoutedEventArgs e)
        {
            _files.Clear();
        }

        private void btnClrItem_Click(object sender, RoutedEventArgs e)
        {
            _files.Remove((FileInfoEx)lvFiles.SelectedItem);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileInfoEx file in _files)
            {
                HashFile(file, gAlgorithmMuliple);
            }

            lvFiles.Items.Refresh();
        }

        #endregion

        #region UI Logic All
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Execute Methods 
        private void HashFile(FileInfoEx file, Grid gAlgorithm)
        {
            foreach (UIElement uie in gAlgorithm.Children)
            {
                if (uie is RadioButton)
                {
                    RadioButton rbTemp = (RadioButton)uie;

                    if (rbTemp.IsChecked.Value)
                    {
                        if (rbTemp.Content.ToString().ToLower().Equals("md5"))
                            file.Hash = new Checksum(algo.MD5, Cryptographic.CalculateMD5(File.ReadAllBytes(file.FullName)));

                        if (rbTemp.Content.ToString().ToLower().Equals("sha1"))
                            file.Hash = new Checksum(algo.SHA1, Cryptographic.CalculateSHA1(File.ReadAllBytes(file.FullName)));
                        
                        if (rbTemp.Content.ToString().ToLower().Equals("sha2"))
                            file.Hash = new Checksum(algo.SHA2, Cryptographic.CalculateSHA2(File.ReadAllBytes(file.FullName)));

                        if (rbTemp.Content.ToString().ToLower().Equals("sha3"))
                            file.Hash = new Checksum(algo.SHA3, Cryptographic.CalculateSHA3(File.ReadAllBytes(file.FullName)));

                        if (rbTemp.Content.ToString().ToLower().Equals("crc32"))
                            file.Hash = new Checksum(algo.CRC32, Cryptographic.CalculateCRC32(File.ReadAllBytes(file.FullName)));

                        if (rbTemp.Content.ToString().ToLower().Equals("crc64"))
                            file.Hash = new Checksum(algo.CRC64, Cryptographic.CalculateCRC64(File.ReadAllBytes(file.FullName)));
                    }
                }
            }
        }

        private void HashString(string Text, Grid gAlgorithm)
        {
            foreach (UIElement uie in gAlgorithm.Children)
            {
                if (uie is RadioButton)
                {
                    RadioButton rbTemp = (RadioButton)uie;

                    if (rbTemp.IsChecked.Value)
                    {
                        if (rbTemp.Content.ToString().ToLower().Equals("md5"))
                            tbResultString.Text = Cryptographic.CalculateMD5(System.Text.Encoding.ASCII.GetBytes(Text));

                        if (rbTemp.Content.ToString().ToLower().Equals("sha1"))
                            tbResultString.Text = Cryptographic.CalculateSHA1(System.Text.Encoding.ASCII.GetBytes(Text));

                        if (rbTemp.Content.ToString().ToLower().Equals("sha2"))
                            tbResultString.Text = Cryptographic.CalculateSHA2(System.Text.Encoding.ASCII.GetBytes(Text));

                        if (rbTemp.Content.ToString().ToLower().Equals("sha3"))
                            tbResultString.Text = Cryptographic.CalculateSHA3(System.Text.Encoding.ASCII.GetBytes(Text));

                        if (rbTemp.Content.ToString().ToLower().Equals("crc32"))
                            tbResultString.Text = Cryptographic.CalculateCRC32(System.Text.Encoding.ASCII.GetBytes(Text));

                        if (rbTemp.Content.ToString().ToLower().Equals("crc64"))
                            tbResultString.Text = Cryptographic.CalculateCRC64(System.Text.Encoding.ASCII.GetBytes(Text));
                    }
                }
            }
        }

        private bool ValidateInput(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                if (new FileInfoEx(filename).Exists)
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        private void tbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbText.Text))
            {
                gbAlgorithmString.IsEnabled = true;
                HashString(tbText.Text, gAlgorithmString);
            }
            else {
                gbAlgorithmString.IsEnabled = false;
            }
        }
    }
}
