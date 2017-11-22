using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateUpdater.Tools
{
    public static class Util
    {
        public static TaskScheduler Scheduler { get; set; }

        public static string DirIdentifier { get; set; }
        public static void Update(IEnumerable<string> source, IEnumerable<string> destination, string replace, bool copyDlls, bool copyAspx, string with = "",  Action<string> ProgressUpdater = null)
        {
            string _replace = "", _with = with;

            if(!string.IsNullOrEmpty(replace))
            {
                _replace = replace.Split('*')[0].Trim();
            }

            var autoDetermineWith = false;
            foreach (var dest in destination)
            {
                ProgressUpdater($"\nUpdating {ShorthenPath(dest)}");

                if (string.IsNullOrEmpty(_with) || autoDetermineWith)
                {
                    _with = dest.Substring(dest.LastIndexOf('\\') + 1).Split(' ')[0];
                    autoDetermineWith = true;
                }

                foreach (var src in source)
                {
                    if (File.Exists(src))
                    {
                        var a = src.IndexOf(DirIdentifier) + DirIdentifier.Length;
                        var b = src.LastIndexOf('\\');
                        var destSub = src.Substring(a, b - a);

                        var _dest = $"{dest}{destSub}";

                        UpdateFile(src, _dest, _replace, _with, ProgressUpdater);
                    }
                    else if (Directory.Exists(src))
                    {
                        var basePath = src.Substring(GetParentDir(src).Length);

                        UpdateDir(src, dest, _replace, _with, basePath, copyDlls, copyAspx, ProgressUpdater);
                    }
                }

                ProgressUpdater($"\n{ShorthenPath(dest)} Updated!");
            }
        }

        private static bool UpdateDir(string sourcePath, string destPath, string replace, string with, string subDir, bool copyDlls, bool copyAspx, Action<string> ProgressUpdater = null)
        {
            var d = $"{destPath}{subDir}";
            if (!Directory.Exists(d))
                Directory.CreateDirectory(d);

            Func<string, bool> _CheckExt = (x) => copyDlls || copyAspx ?
            (copyDlls && copyAspx ? (x.EndsWith(".cs") || x.EndsWith(".dll")) || x.EndsWith(".aspx") :
            copyDlls ? (x.EndsWith(".cs") || x.EndsWith(".dll")) : (x.EndsWith(".cs") || x.EndsWith(".aspx"))) : 
            x.EndsWith(".cs");

            foreach (var file in Directory.GetFiles(sourcePath).Where(x => _CheckExt(x)))
            {
                UpdateFile(file, d, replace, with, ProgressUpdater);
            }
            foreach (var dir in Directory.GetDirectories(sourcePath))
            {
                subDir = dir.Substring(dir.IndexOf(subDir));
                d = $"{destPath}{subDir}";
                UpdateDir(dir, destPath, replace, with, subDir, copyDlls, copyAspx, ProgressUpdater);
            }
            return true;
        }

        public static bool UpdateFile(string sourcePath, string destPath, string replace, string with, Action<string> ProgressUpdater = null)
        {
            try
            {
                var fileName = (new FileInfo(sourcePath)).Name;
                var updatedFilePath = Path.Combine(destPath, fileName);

                if (File.Exists(sourcePath) && Directory.Exists(destPath) && (sourcePath.EndsWith(".cs") || sourcePath.EndsWith(".aspx")))
                {
                    var updated = File.ReadAllText(sourcePath).Replace(replace, with);

                    File.WriteAllText(updatedFilePath, updated);

                    updatedFilePath = ShorthenPath(updatedFilePath);

                    ProgressUpdater(updatedFilePath);

                    return true;
                }
                else if (sourcePath.EndsWith(".dll"))
                {
                    File.Copy(sourcePath, updatedFilePath, true);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string PackageTemplate(string path, string publishPath)
        {
            var defPath = Path.Combine(path, "Definition.json");
            if (File.Exists(defPath))
            {
                string msg;
                if(TemplateDeploymentTools.Util.LoadDefinitionFile(defPath, out msg))
                {
                    var config = TemplateDeploymentTools.Util.GetConfig;
                    config.PublishPath = publishPath;

                    TemplateDeploymentTools.Util.WriteObjectToFile(config);
                    //ProgressUpdater($"Building {config.FolderName}...");

                    if (TemplateDeploymentTools.Util.PackageTemplate())
                        return $"{config.FolderName} Built Successfully!";
                }
            }
            return "Build Attempt Failed!";
        }

        public static void ResizeImages(string from, string outputDirName, string width, Action<string> ProgressUpdater)
        {
            try
            {
                var size = 80;
                
                if (!string.IsNullOrEmpty(width))
                {
                    int.TryParse(width, out size);
                    ProgressUpdater($"Resizing to {width}px width");
                }

                var destDir = $@"{from}\..\{outputDirName}";

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                foreach (var img in Directory.GetFiles(from).Select(x => new FileInfo(x)))
                {
                    var ext = img.Extension.ToLower();
                    if (ext == ".png" || ext == ".jpg")
                    {
                        Console.WriteLine($"resizing => {img.Name}");
                        var resizer = new ImageResizer(img.FullName);
                        if (ext == ".png")
                            resizer.Resize(size, ImageEncoding.Png);
                        else
                            resizer.Resize(size, ImageEncoding.Jpg90);

                        resizer.SaveToFile($@"{destDir}\{img.Name}");

                        ProgressUpdater($"Resizing => {img.Name}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed ==> {ex}");
            }
        }

        private static string ShorthenPath(string updatedFilePath)
        {
            var index = updatedFilePath.IndexOf($@"{DirIdentifier}\");
            if (index == -1) index = updatedFilePath.Length - 1;
            while (true)
            {
                if (updatedFilePath[index--] == '\\')
                    break;
                if (index < 0)
                    break;
            }
            updatedFilePath = $"..{updatedFilePath.Substring(++index)}";
            return updatedFilePath;
        }

        public static string GetParentDir(string path)
        {
            return path.Substring(0, path.LastIndexOf('\\'));
        }

        private static void UpdateUI(Action<string> ProgressUpdater, string msg)
        {
            if (ProgressUpdater != null)
            {
                // this can be called anywhere
                var task = new System.Threading.Tasks.Task(() => ProgressUpdater(msg));
                task.Start(Scheduler);
            }
        }
    }
}
