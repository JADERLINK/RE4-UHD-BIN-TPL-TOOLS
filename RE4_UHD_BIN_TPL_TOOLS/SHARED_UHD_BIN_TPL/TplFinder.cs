using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SHARED_UHD_BIN_TPL
{
    /// <summary>
    /// Finds the TPL that belongs to a BIN.
    /// 1) TPL with the same name as the BIN.
    /// 2) Otherwise the TPL is the BIN number + 1 (keeping the digit count):
    ///    0003.BIN -> 0004.TPL | 3.BIN -> 4.TPL | R100_027.BIN -> R100_028.TPL | Em20_27.BIN -> Em20_28.TPL
    /// Case-insensitive.
    /// </summary>
    internal static class TplFinder
    {
        private static readonly Regex OnlyNumber = new Regex(@"^(\d+)$", RegexOptions.Compiled);
        private static readonly Regex PrefixNumber = new Regex(@"^(.+_)(\d+)$", RegexOptions.Compiled);

        public static string Find(string directory, string binBaseName, out string foundFileName)
        {
            foundFileName = null;

            // 1) same name
            string path = FindIgnoreCase(directory, binBaseName + ".TPL");
            if (path != null)
            {
                foundFileName = Path.GetFileName(path);
                return path;
            }

            // 2) name + 1
            string nextName = GetNextName(binBaseName);
            if (nextName != null)
            {
                path = FindIgnoreCase(directory, nextName + ".TPL");
                if (path != null)
                {
                    foundFileName = Path.GetFileName(path);
                    return path;
                }
            }

            return null;
        }

        private static string GetNextName(string name)
        {
            string prefix;
            string digits;

            Match m = OnlyNumber.Match(name);
            if (m.Success)
            {
                prefix = "";
                digits = m.Groups[1].Value;
            }
            else
            {
                m = PrefixNumber.Match(name);
                if (!m.Success)
                {
                    return null;
                }
                prefix = m.Groups[1].Value;
                digits = m.Groups[2].Value;
            }

            if (!ulong.TryParse(digits, out ulong number) || number == ulong.MaxValue)
            {
                return null;
            }

            return prefix + (number + 1).ToString().PadLeft(digits.Length, '0');
        }

        private static string FindIgnoreCase(string directory, string fileName)
        {
            string direct = Path.Combine(directory, fileName);
            if (File.Exists(direct))
            {
                return direct;
            }

            try
            {
                foreach (string file in Directory.EnumerateFiles(directory))
                {
                    if (string.Equals(Path.GetFileName(file), fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return file;
                    }
                }
            }
            catch (Exception) { }

            return null;
        }
    }
}
