using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace AdditionalIsoKeyToShift
{
    internal class RegistryOperations
    {
        public static void CreateBackup(string patch)
        {
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "reg.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.Arguments = "export \"" + GlobalFlags.RegistryKeyBackupRestore + "\" \"" + patch + "\" /y";
                    proc.Start();
                    string stdout = proc.StandardOutput.ReadToEnd();
                    string stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void RestoreBackup(string patch)
        {
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "reg.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.Arguments = "import \"" + patch + "\" ";
                    proc.Start();
                    string stdout = proc.StandardOutput.ReadToEnd();
                    string stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
  
        public static void AddRegistryKey(string keyPatch, string keyName, byte[] value)
        {
            if (!CheckRegistryValueExists(keyPatch, keyName))
            {
                try
                {
                    RegistryKey key;
                    key = Registry.LocalMachine.OpenSubKey(keyPatch, true);
                    key.SetValue(keyName, value, RegistryValueKind.Binary);
                    key.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Added key " + keyName);
            }
            else
            {
                MessageBox.Show("Key " + keyName + " already exists");
            }
        }

        public static void RemoveRegistryKey(string keyPatch, string keyName)
        {
            if (CheckRegistryValueExists(keyPatch, keyName))
            {
                try
                {
                    RegistryKey key;
                    key = Registry.LocalMachine.OpenSubKey(keyPatch, true);
                    key.DeleteValue(keyName);
                    key.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Removed key " + keyName);
            }
            else
            {
                MessageBox.Show("Key " + keyName + " is missing");
            }
        }

        public static bool CheckRegistryValueExists(string keyPatch, string keyName)
        {
            RegistryKey winLogonKey = Registry.LocalMachine.OpenSubKey(keyPatch);
            return (winLogonKey.GetValueNames().Contains(keyName));
        }
    }
}
