using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SHARED_UHD_BIN;

namespace RE4_PS4NS_BIN_TOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine(Shared.HeaderText());

            if (args.Length == 0)
            {
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE4-UHD-BIN-TOOL");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else if (args.Length >= 1 && File.Exists(args[0]))
            {
                try
                {
                    //FileInfo
                    FileInfo fileInfo1 = new FileInfo(args[0]);
                    FileInfo fileInfo2 = null;

                    //extension
                    string file1Extension = fileInfo1.Extension.ToUpperInvariant();
                    string file2Extension = null;

                    Console.WriteLine("File1: " + fileInfo1.Name);

                    //verrifica o file2
                    if (args.Length >= 2 && File.Exists(args[1]))
                    {
                        fileInfo2 = new FileInfo(args[1]);
                        file2Extension = fileInfo2.Extension.ToUpperInvariant();
                        Console.WriteLine("File2: " + fileInfo2.Name);
                    }

                    MainAction.Actions(fileInfo1, file1Extension, fileInfo2, file2Extension, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error1: " + ex);
                }

            }
            else
            {
                Console.WriteLine("File specified does not exist.");
            }

            Console.WriteLine("Finished!!!");
        }
    }
}
