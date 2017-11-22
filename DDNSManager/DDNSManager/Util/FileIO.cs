using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNSManager.Util
{
    public static class FileIO
    {
        public static TransferObject ImportJsonAsObject<TransferObject>(string filePath) where TransferObject : class
        {
            if (System.IO.File.Exists(filePath))
            {
                var data = System.IO.File.ReadAllText(filePath);

                var myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TransferObject>(data);

                return myObject;
            }
            return null;
        }

        public static void WriteObjectToFile<T>(this T obj, string filePath = null) where T : class
        {
            // Serialize object..

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            if (filePath == null)
                filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "info.dat");

            // Create a file to write to or Overwrite existing.
            System.IO.File.WriteAllText(filePath, jsonStr);
        }
        public static string ScrambleString(string text)
        {
            var sb = new StringBuilder(4 * text.Length);
            var len = text.Length;
            var index = 1;

            var pick = new char[] { 'A', 'F', 'h', 'k', 'O', 'l', 'M', 'Q', 'y', 'Z', '^', '$', '#' };

            var rdn = new Random();

            sb.Append(rdn.Next(0, 9));
            foreach (var c in text)
            {
                sb.Append(text[rdn.Next(0, text.Length - 1)]).Append(c).Append(pick[rdn.Next(0, pick.Length - 1)]);
            }
            sb.Append(rdn.Next(0, 9));

            return sb.ToString();
        }

        public static string UnscrambleString(string text)
        {
            var pick = 2;
            var sb = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                if (i == pick)
                {
                    sb.Append(text[i]);
                    pick += 3;
                }
            }
            return sb.ToString();
        }
    }


}
