using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KZPC1.Setting
{
    public class Setup
    {
        public void SetupAll()
        {
            Console.WriteLine("Dir setuping ...");
            foreach(var t in Const.ALL_PATH)
            {
                var path = Path.GetFullPath(Const.SAVE_PATH_FILE);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Dir created in " + path);
                }
            }
            Console.WriteLine("Dir setuping DONE");
        }

        public void DelAllPic()
        {
            Console.WriteLine("Pic deleting ...");
            foreach (var t in Const.ALL_PATH)
            {
                var path = Path.GetFullPath(Const.SAVE_PATH_FILE);
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path);
                    foreach(var f in files)
                    {
                        try
                        {
                            File.Delete(f);
                            Console.WriteLine("File deleted in '" + Path.GetDirectoryName(f) + "' named '" + Path.GetFileName(f) + "'");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Can not delet file in '" + Path.GetDirectoryName(f) + "' named '" + Path.GetFileName(f) + "' with exception '" + e + "'");
                        }
                    }
                }
            }
            Console.WriteLine("Pic deleting DONE");
        }

        public static string GetContentFile(string fileName)
        {
            return Path.Combine(Path.GetFullPath(Const.CONTENT_PATH_FILE), fileName);
        }
    }
}
