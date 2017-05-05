using OpmInspection.Shared.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpmInspection.Console
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string option = args[0].ToLower();

                switch (option)
                {
                    case "bing":
                        // ดาวน์โหลด bing photo
                        System.Console.WriteLine(string.Format("Downloaded: {0} files.", Bing.Download(path: args[1].ToLower()).ToString("#,##0")));
                        break;
                    case "mysql":
                        // สร้าง TruncateTime function ใน mysql server
                        System.Console.WriteLine(Program.CreateFunction(name: args[1].ToLower()));
                        break;
                    default:
                        Program.Nothing();
                        break;
                }
            }
            else
            {
                Program.Nothing();
            }
        }

        static void Nothing()
        {
            System.Console.WriteLine("Invalid option arguments.");
        }
    }
}
