using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace copySystem
{ 
    public class IO
    {

        public static async Task CopyDirectory(DirectoryInfo di, IProgress<string> progress, string targetPath)
        {
            string newPath = System.IO.Path.Combine(targetPath, di.Name);
            if (!System.IO.Directory.Exists(newPath))
                System.IO.Directory.CreateDirectory(newPath);

            foreach(FileInfo fi in di.GetFiles())
                await copySystem.IO.CopyFile(fi, progress, newPath);

            foreach (DirectoryInfo _di in di.GetDirectories())
                await copySystem.IO.CopyDirectory(_di, progress, newPath);

        }


        public static async Task CopyFile(FileInfo lfi, IProgress<string> progress, string targetPath)
        {
            //string newPath;

            using (FileStream SourceStream = File.Open(lfi.FullName, FileMode.OpenOrCreate))
            {
                using (FileStream DestinationStream = File.Create(System.IO.Path.Combine(targetPath, lfi.Name)))
                {
                    progress.Report("Copying: " + lfi.FullName);
                    await SourceStream.CopyToAsync(DestinationStream);
                }
            }
        }
        
    }
}
