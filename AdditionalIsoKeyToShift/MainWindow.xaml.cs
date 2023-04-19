using Microsoft.Win32;
using System;
using System.Drawing;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

namespace AdditionalIsoKeyToShift
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!IsAdministrator())
            {
                MessageBox.Show("Run in administrator mode");
                this.Close();
            }
        }

        private void lockButtons(bool state)
        {
            if (state)
            {
                btnChangeIsoToShift.IsEnabled = true;
                btnCreateBackup.IsEnabled = true;
                btnExit.IsEnabled = true;
                btnResetPc.IsEnabled = true;
                btnRestoreBackup.IsEnabled = true;
                btnUndoChangeIsoToShift.IsEnabled = true;
            }
            else
            {
                btnChangeIsoToShift.IsEnabled = false;
                btnCreateBackup.IsEnabled = false;
                btnExit.IsEnabled = false;
                btnResetPc.IsEnabled = false;
                btnRestoreBackup.IsEnabled = false;
                btnUndoChangeIsoToShift.IsEnabled = false;
            }
        }

        private void ChangeStateResetButton()
        {
            btnResetPc.Foreground = Brushes.Red;
        }

        private void btnCreateBackup_Click(object sender, RoutedEventArgs e)
        {
            lockButtons(false);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Registry file (*.reg)|*.reg|Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = GlobalFlags.DefaultBackupRestoreDirectory;

            if (saveFileDialog.ShowDialog() == true)
            {
                RegistryOperations.CreateBackup(saveFileDialog.FileName);
                MessageBox.Show("Backup completed");
            }
            lockButtons(true);
            ChangeStateResetButton();
        }

        private void btnRestoreBackup_Click(object sender, RoutedEventArgs e)
        {
            lockButtons(false);
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Registry file (*.reg)|*.reg";

            if(openFileDialog.ShowDialog() == true)
            {
                RegistryOperations.RestoreBackup(openFileDialog.FileName);
                MessageBox.Show("Restored");
            }
            lockButtons(true);
            ChangeStateResetButton();
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void btnChangeIsoToShift_Click(object sender, RoutedEventArgs e)
        {
            lockButtons(false);
            byte[] bytes = new byte[] { 00, 00, 00, 00, 00, 00, 00, 00,
                                        02, 00, 00, 00, 42, 00, 86, 00,
                                        00, 00, 00, 00 };

            RegistryOperations.AddRegistryKey(GlobalFlags.RegistryKeyPatch, GlobalFlags.RegistryKeyName, bytes);
            lockButtons(true);
            ChangeStateResetButton();
        }

        private void btnUndoChangeIsoToShift_Click(object sender, RoutedEventArgs e)
        {
            lockButtons(false);
            RegistryOperations.RemoveRegistryKey(GlobalFlags.RegistryKeyPatch, GlobalFlags.RegistryKeyName);
            lockButtons(true);
            ChangeStateResetButton();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnResetPc_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }
    }
}
