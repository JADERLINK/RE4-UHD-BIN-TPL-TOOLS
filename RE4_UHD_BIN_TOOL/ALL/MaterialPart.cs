using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RE4_UHD_BIN_TOOL
{
    public class MaterialPart
    {
        public byte unk_min_11;
        public byte unk_min_10;
        public byte unk_min_09;
        public byte unk_min_08;
        public byte unk_min_07;
        public byte unk_min_06;
        public byte unk_min_05;
        public byte unk_min_04;
        public byte unk_min_03;
        public byte unk_min_02;
        public byte unk_min_01;
        public byte material_flag;
        public byte diffuse_map;
        public byte bump_map;
        public byte opacity_map; //alpha
        public byte generic_specular_map;
        public byte intensity_specular_r;
        public byte intensity_specular_g;
        public byte intensity_specular_b;
        public byte unk_08;
        public byte unk_09;
        public byte specular_scale;
        public byte unk_11;
        public byte custom_specular_map;

        public byte[] GetArray()
        {
            byte[] b = new byte[24];
            b[00] = unk_min_11;
            b[01] = unk_min_10;
            b[02] = unk_min_09;
            b[03] = unk_min_08;
            b[04] = unk_min_07;
            b[05] = unk_min_06;
            b[06] = unk_min_05;
            b[07] = unk_min_04;
            b[08] = unk_min_03;
            b[09] = unk_min_02;
            b[10] = unk_min_01;
            b[11] = material_flag;
            b[12] = diffuse_map;
            b[13] = bump_map;
            b[14] = opacity_map;
            b[15] = generic_specular_map;
            b[16] = intensity_specular_r;
            b[17] = intensity_specular_g;
            b[18] = intensity_specular_b;
            b[19] = unk_08;
            b[20] = unk_09;
            b[21] = specular_scale;
            b[22] = unk_11;
            b[23] = custom_specular_map;
            return b;
        }

    }

}
