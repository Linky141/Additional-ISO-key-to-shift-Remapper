using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalIsoKeyToShift
{
    public static class GlobalFlags
    {
        public static string RegistryKeyBackupRestore = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layout";
        
        public static string DefaultBackupRestoreDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //public static string DefaultBackupRestoreDirectory = @"C:\Users\tomas\OneDrive\Dokumenty\PROGRAMOWANIE\VisualStudio\AdditionalIsoKeyToShift\AdditionalIsoKeyToShift\bin\Debug\net6.0-windows\TEST\";

        public static string RegistryKeyPatch = @"SYSTEM\CurrentControlSet\Control\Keyboard Layout";
        public static string RegistryKeyName = "Scancode Map";
    }
}
