using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SHARED_UHD_BIN_TPL.ALL
{
    public static class Utils
    {
        public static string ReturnValidHexValue(string cont)
        {
            string res = "";
            foreach (var c in cont.ToUpperInvariant())
            {
                if (char.IsDigit(c)
                    || c == 'A'
                    || c == 'B'
                    || c == 'C'
                    || c == 'D'
                    || c == 'E'
                    || c == 'F'
                    )
                {
                    res += c;
                }
            }
            return res;
        }

        public static string ReturnValidDecValue(string cont)
        {
            string res = "";
            foreach (var c in cont)
            {
                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }

        public static string ReturnValidDecWithNegativeValue(string cont)
        {
            bool negative = false;

            string res = "";
            foreach (var c in cont)
            {
                if (negative == false && c == '-')
                {
                    res = c + res;
                    negative = true;
                }

                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }

        public static string ReturnValidFloatValue(string cont)
        {
            bool Dot = false;
            bool negative = false;

            string res = "";
            foreach (var c in cont)
            {
                if (negative == false && c == '-')
                {
                    res = c + res;
                    negative = true;
                }

                if (Dot == false && c == '.')
                {
                    res += c;
                    Dot = true;
                }
                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }

        public static short ParseFloatToShort(float value)
        {
            string sv = value.ToString("F", System.Globalization.CultureInfo.InvariantCulture).Split('.')[0];
            int iv = 0;
            try
            {
                iv = int.Parse(sv, System.Globalization.NumberStyles.Integer);
            }
            catch (Exception)
            {
            }
            if (iv > short.MaxValue)
            {
                iv = short.MaxValue;
            }
            else if (iv < short.MinValue)
            {
                iv = short.MinValue;
            }
            return (short)iv;
        }

        public static bool SetByteHex(ref string line, string key, ref byte varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = byte.Parse(Utils.ReturnValidHexValue(split[1]), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetByteDec(ref string line, string key, ref byte varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = byte.Parse(Utils.ReturnValidDecValue(split[1]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetUintHex(ref string line, string key, ref uint varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = uint.Parse(Utils.ReturnValidHexValue(split[1]), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetUintDec(ref string line, string key, ref uint varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = uint.Parse(Utils.ReturnValidDecValue(split[1]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetUshortDec(ref string line, string key, ref ushort varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = ushort.Parse(Utils.ReturnValidDecValue(split[1]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetFloatDec(ref string line, string key, ref float varToSet)
        {
            if (line.StartsWith(key))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = float.Parse(Utils.ReturnValidFloatValue(split[1]), NumberStyles.Float, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }

        public static bool SetBoolean(ref string line, string key, ref bool varToSet) 
        {
            if (line.StartsWith(key))
            {
                var split = line.ToLowerInvariant().Split(':');
                if (split.Length >= 2)
                {
                    try
                    {
                        varToSet = bool.Parse(split[1].Trim());
                    }
                    catch (Exception)
                    {
                    }
                }
                return true;
            }
            return false;
        }
    }
}
