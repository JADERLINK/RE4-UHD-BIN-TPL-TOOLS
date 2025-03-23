using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SHARED_UHD_BIN.ALL;
using SimpleEndianBinaryIO;

namespace SHARED_UHD_BIN.REPACK
{
    public class IdxUhdBin
    {
        public bool CompressVertices { get; set; }

        public int ObjFileUseBone { get; set; }

        public FinalBoneLine[] Bones { get; set; }

        public byte[][] BonePairLines { get; set; }

        public bool UseExtendedNormals { get; set; }

        public bool UseWeightMap { get; set; }

        public bool EnableBonepairTag { get; set; }

        public bool EnableAdjacentBoneTag { get; set; }

        public bool UseVertexColor { get; set; }
    }


    public static class IdxUhdBinLoad
    {
        public static IdxUhdBin Load(Stream stream, Endianness endianness)
        {
            IdxUhdBin idx = new IdxUhdBin();

            StreamReader reader = new StreamReader(stream, Encoding.ASCII);

            Dictionary<string, string> pair = new Dictionary<string, string>();

            //--------

            string line = "";
            while (line != null)
            {
                line = reader.ReadLine();
                if (line != null && line.Length != 0)
                {
                    var split = line.Trim().Split(new char[] { ':' });

                    if (line.TrimStart().StartsWith(":") || line.TrimStart().StartsWith("#") || line.StartsWith("\\") || line.TrimStart().StartsWith("/"))
                    {
                        continue;
                    }
                    else if (split.Length >= 2)
                    {

                        string key = split[0].ToUpper().Trim();

                        if (!pair.ContainsKey(key))
                        {
                            pair.Add(key, split[1]);
                        }

                    }

                }
            }

            //-------

            if (pair.ContainsKey("USEVERTEXCOLOR"))
            {
                try
                {
                    idx.UseVertexColor = bool.Parse(pair["USEVERTEXCOLOR"].Trim());
                }
                catch (Exception)
                {
                }
            }


            if (pair.ContainsKey("COMPRESSVERTICES"))
            {
                try
                {
                    idx.CompressVertices = bool.Parse(pair["COMPRESSVERTICES"].Trim());
                }
                catch (Exception)
                {
                }
            }

            if (pair.ContainsKey("USEEXTENDEDNORMALS"))
            {
                try
                {
                    idx.UseExtendedNormals = bool.Parse(pair["USEEXTENDEDNORMALS"].Trim());
                }
                catch (Exception)
                {
                }
            }


            if (pair.ContainsKey("USEWEIGHTMAP"))
            {

                try
                {
                    idx.UseWeightMap = bool.Parse(pair["USEWEIGHTMAP"].Trim());
                }
                catch (Exception)
                {
                }
            }


            if (pair.ContainsKey("ENABLEBONEPAIRTAG"))
            {

                try
                {
                    idx.EnableBonepairTag = bool.Parse(pair["ENABLEBONEPAIRTAG"].Trim());
                }
                catch (Exception)
                {
                }
            }


            if (pair.ContainsKey("ENABLEADJACENTBONETAG"))
            {

                try
                {
                    idx.EnableAdjacentBoneTag = bool.Parse(pair["ENABLEADJACENTBONETAG"].Trim());
                }
                catch (Exception)
                {
                }
            }


            if (pair.ContainsKey("OBJFILEUSEBONE"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["OBJFILEUSEBONE"]);
                    idx.ObjFileUseBone = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }


            //----------

            int BonesCount = 0;

            if (pair.ContainsKey("BONESCOUNT"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["BONESCOUNT"]);
                    BonesCount = int.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }

            List<FinalBoneLine> BoneLines = new List<FinalBoneLine>();

            for (int i = 0; i < BonesCount; i++)
            {
                byte[] boneLine = new byte[0x10];

                if (pair.ContainsKey("BONELINE_" + i))
                {

                    string value = Utils.ReturnValidHexValue(pair["BONELINE_" + i].ToUpper());
                    value = value.PadRight(0x10 * 2, '0');

                    int cont = 0;
                    for (int ipros = 0; ipros < boneLine.Length; ipros++)
                    {
                        string v = value[cont].ToString() + value[cont + 1].ToString();
                        boneLine[ipros] = byte.Parse(v, System.Globalization.NumberStyles.HexNumber);
                        cont += 2;
                    }

                }

                BoneLines.Add(new FinalBoneLine(boneLine, endianness));
            }


            //----------

            byte BonePairCount = 0;
            if (pair.ContainsKey("BONEPAIRCOUNT"))
            {

                try
                {
                    string value = Utils.ReturnValidDecValue(pair["BONEPAIRCOUNT"]);
                    BonePairCount = byte.Parse(value, System.Globalization.NumberStyles.Integer);
                }
                catch (Exception)
                {
                }
            }

            byte[][] BonePairLines = new byte[BonePairCount][];

            for (int i = 0; i < BonePairCount; i++)
            {
                byte[] bonepairLine = new byte[0x8];

                if (pair.ContainsKey("BONEPAIRLINE_" + i))
                {

                    string value = Utils.ReturnValidHexValue(pair["BONEPAIRLINE_" + i].ToUpper());
                    value = value.PadRight(0x8 * 2, '0');

                    int cont = 0;
                    for (int ipros = 0; ipros < bonepairLine.Length; ipros++)
                    {
                        string v = value[cont].ToString() + value[cont + 1].ToString();
                        bonepairLine[ipros] = byte.Parse(v, System.Globalization.NumberStyles.HexNumber);
                        cont += 2;
                    }

                }

                BonePairLines[i] = bonepairLine;
            }


            //----------

            idx.BonePairLines = BonePairLines;
            idx.Bones = BoneLines.ToArray();
            return idx;
        }




    }




}