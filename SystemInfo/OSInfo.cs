using System;
using System.Text;
using Microsoft.Win32;     //This namespace is used to work with Registry editor.
using System.Management;   //This namespace is used to work with WMI classes. For using this namespace add reference of System.Management.dll .
// Make sure to include the references to Microsoft.VisualBasic and System.Management

namespace OSInfo
{
    // For testing SystemInfo class
    class TestProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine(SystemInfo.GetOperatingSystemInfo());             // Call get operating system info method which will display operating system information.
            Console.WriteLine(SystemInfo.GetProcessorInfo());                   // Call get  processor info method which will display processor info.
            Console.WriteLine(SystemInfo.GetRam());                             // Call Visual basic computer info to get physical memory
            Console.WriteLine(SystemInfo.GetVideoCard());                       // Call to get video card information via management object searcher
            Console.ReadLine();                                                 // Wait for user to accept input key.
        }
    }
    
    public class SystemInfo
    {
        public static string GetOperatingSystemInfo()
        {
            //Create an object of ManagementObjectSearcher class and pass query as parameter.
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            StringBuilder sb = new StringBuilder();

            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    sb.AppendLine("Operating System Name: " + managementObject["Caption"].ToString());   //Display operating system caption
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    sb.AppendLine("Operating System Architecture: " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                }
                if (managementObject["CSDVersion"] != null)
                {
                    sb.AppendLine("Operating System Service Pack: " + managementObject["CSDVersion"].ToString());     //Display operating system version.
                }
            }
            return sb.ToString();
        }

        public static string GetProcessorInfo()
        {
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    return "Processor Name: " + processor_name.GetValue("ProcessorNameString").ToString();   //Display processor 
                }
            }
            return "N/A";
        }

        public static string GetRam()
        {
            double ram = Math.Round((new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / Math.Pow(1024, 3)), 2);
            return "RAM: " + ram + " GB";
        }

        public static string GetVideoCard()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        graphicsCard = property.Value.ToString();
                    }
                }
            }
            return "Video Card: " + graphicsCard;
        }
    }
}
