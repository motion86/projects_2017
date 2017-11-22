using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_resize
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var size =  new[] { 600, 80 };
                if (args.Length > 0)
                {
                    var dir = args[0];
                    Console.WriteLine(dir);
                    if (args.Length > 2)
                    {
                        if (args[0].ToLower() == "-s")
                            int.TryParse(args[1], out size[0]);
                    }

                    var destDir = new[] { $@"{dir}\..\samples", $@"{dir}\..\thumbs" };

                    foreach (var d in destDir)
                    {
                        if (!Directory.Exists(d))
                            Directory.CreateDirectory(d);
                    }
                    foreach (var img in Directory.GetFiles(dir).Select(x => new FileInfo(x)))
                    {
                        int i = 0;
                        foreach (var _dir in destDir)
                        {
                            var ext = img.Extension.ToLower();
                            if (ext == ".png" || ext == ".jpg")
                            {
                                Console.WriteLine($"resizing => {img.Name}");
                                var resizer = new ImageResizer(img.FullName);
                                if (ext == ".png")
                                    resizer.Resize(size[i], ImageEncoding.Png);
                                else
                                    resizer.Resize(size[i], ImageEncoding.Jpg90);

                                resizer.SaveToFile($@"{_dir}\{img.Name}");
                            }
                            i++;
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine("use: [rawDir]");
                    Console.WriteLine("or: [rawDir] -s [size]");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed ==> {ex}");
            }
        }
    }
}
