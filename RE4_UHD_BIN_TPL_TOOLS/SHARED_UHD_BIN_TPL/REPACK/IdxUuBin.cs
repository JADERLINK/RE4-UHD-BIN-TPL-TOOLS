using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using SHARED_TOOLS.ALL;

namespace SHARED_UHD_BIN_TPL.REPACK
{
    public class IdxUuBin
    {
        public (int ID, int parent, float x, float y, float z)[] Bones;
        public (ushort b1, ushort b2, ushort b3, ushort b4)[] BonePairs;

        public uint ObjFileUseBone;
        public bool UseAlternativeNormals;
        public bool UseWeightMap;
        public bool EnableBonepairTag;
        public bool EnableAdjacentBoneTag;
        public bool UseVertexColor;
    }

    public static class IdxUuBinLoad
    {
        public static IdxUuBin Load(Stream stream)
        {
            IdxUuBin idx = new IdxUuBin();

            StreamReader reader = new StreamReader(stream, Encoding.ASCII);

            List<(int ID, int parent, float x, float y, float z)> bones = new List<(int ID, int parent, float x, float y, float z)>();
            List<(ushort b1, ushort b2, ushort b3, ushort b4)> bonespairs = new List<(ushort b1, ushort b2, ushort b3, ushort b4)>();

            while (!reader.EndOfStream)
            {
                string line = reader?.ReadLine()?.Trim()?.ToUpperInvariant();

                if (line == null
                    || line.Length == 0
                    || line.StartsWith("\\")
                    || line.StartsWith("/")
                    || line.StartsWith("#")
                    || line.StartsWith(":")
                    || line.StartsWith("!")
                    || line.StartsWith("@")
                    || line.StartsWith("=")
                    )
                {
                    continue;
                }
                else if (line.StartsWith("BONELINE"))
                {
                    var split = NormalizeLine(line).Split(':');
                    if (split.Length >= 2)
                    {
                        var parts = split[1].Trim().Split(' ');
                        if (parts.Length >= 2)
                        {
                            int id;
                            int parent;
                            float p1 = 0;
                            float p2 = 0;
                            float p3 = 0;

                            int.TryParse(Utils.ReturnValidDecValue(parts[0]), NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
                            int.TryParse(Utils.ReturnValidDecWithNegativeValue(parts[1]), NumberStyles.Integer, CultureInfo.InvariantCulture, out parent);
                            if (parts.Length >= 3)
                            {
                                float.TryParse(Utils.ReturnValidFloatValue(parts[2]), NumberStyles.Float, CultureInfo.InvariantCulture, out p1);
                            }
                            if (parts.Length >= 4)
                            {
                                float.TryParse(Utils.ReturnValidFloatValue(parts[3]), NumberStyles.Float, CultureInfo.InvariantCulture, out p2);
                            }
                            if (parts.Length >= 5)
                            {
                                float.TryParse(Utils.ReturnValidFloatValue(parts[4]), NumberStyles.Float, CultureInfo.InvariantCulture, out p3);
                            }

                            bones.Add((id, parent, p1, p2, p3));
                        }
                    }
                }
                else if (line.StartsWith("BONEPAIR"))
                {
                    var split = NormalizeLine(line).Split(':');
                    if (split.Length >= 2)
                    {
                        var parts = split[1].Trim().Split(' ');
                        if (parts.Length >= 4)
                        {
                            ushort b1;
                            ushort b2;
                            ushort b3;
                            ushort b4;

                            ushort.TryParse(Utils.ReturnValidDecValue(parts[0]), NumberStyles.Integer, CultureInfo.InvariantCulture, out b1);
                            ushort.TryParse(Utils.ReturnValidDecValue(parts[1]), NumberStyles.Integer, CultureInfo.InvariantCulture, out b2);
                            ushort.TryParse(Utils.ReturnValidDecValue(parts[2]), NumberStyles.Integer, CultureInfo.InvariantCulture, out b3);
                            ushort.TryParse(Utils.ReturnValidDecValue(parts[3]), NumberStyles.Integer, CultureInfo.InvariantCulture, out b4);

                            bonespairs.Add((b1, b2, b3, b4));
                        }
                    }
                }
                else 
                {
                    _ = Utils.SetUintDec(ref line, "OBJFILEUSEBONE", ref idx.ObjFileUseBone)
                     || Utils.SetBoolean(ref line, "USEALTERNATIVENORMALS", ref idx.UseAlternativeNormals)
                     || Utils.SetBoolean(ref line, "USEWEIGHTMAP", ref idx.UseWeightMap)
                     || Utils.SetBoolean(ref line, "ENABLEADJACENTBONETAG", ref idx.EnableAdjacentBoneTag)
                     || Utils.SetBoolean(ref line, "ENABLEBONEPAIRTAG", ref idx.EnableBonepairTag)
                     || Utils.SetBoolean(ref line, "USEVERTEXCOLOR", ref idx.UseVertexColor)
                        ;
                }
            
            }

            idx.BonePairs = bonespairs.ToArray();
            idx.Bones = bones.ToArray();
            return idx;
        }

        private static string NormalizeLine(string line)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\s+");
            return regex.Replace(line, " ").Trim();
        }
    }

}