using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Task_1.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            String subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            return (string)Registry.LocalMachine.OpenSubKey(subKey).GetValue("ProductName");
        }
    }
}
