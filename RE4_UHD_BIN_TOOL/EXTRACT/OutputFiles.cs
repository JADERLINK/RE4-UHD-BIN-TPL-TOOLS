using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_UHD_BIN_TOOL
{
    public static class OutputFiles
    {

        //Studiomdl Data
        public static void CreateSMD(UhdBIN uhdbin, string baseDiretory, string baseFileName)
        {
            float NORMAL_FIX = ReturnsNormalsFixValue(uhdbin.Header.texture2_flags);

            var inv = System.Globalization.CultureInfo.InvariantCulture;

            TextWriter text = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".smd")).CreateText();
            text.WriteLine("version 1");
            text.WriteLine("nodes");

            for (int i = 0; i < uhdbin.Bones.Length; i++)
            {
                text.WriteLine(uhdbin.Bones[i].BoneID + " \"BONE_" + uhdbin.Bones[i].BoneID.ToString("D3") + "\" " + uhdbin.Bones[i].BoneParent);
            }

            text.WriteLine("end");

            text.WriteLine("skeleton");
            text.WriteLine("time 0");

            for (int i = 0; i < uhdbin.Bones.Length; i++)
            {
                text.WriteLine(uhdbin.Bones[i].BoneID + "  " +
                    (uhdbin.Bones[i].PositionX / CONSTs.GLOBAL_POSITION_SCALE).ToString("f9", inv) + " " +
                    (uhdbin.Bones[i].PositionZ * -1 / CONSTs.GLOBAL_POSITION_SCALE).ToString("f9", inv) + " " +
                    (uhdbin.Bones[i].PositionY / CONSTs.GLOBAL_POSITION_SCALE).ToString("f9", inv) + "  0.000000 0.000000 0.000000");
            }

            text.WriteLine("end");

            text.WriteLine("triangles");

            for (int g = 0; g < uhdbin.Materials.Length; g++)
            {
                for (int l = 0; l < uhdbin.Materials[g].face_index_array.Length; l++)
                {
                    text.WriteLine(CONSTs.UHD_MATERIAL + g.ToString("D3"));

                    int[] indexs = new int[3];
                    indexs[0] = uhdbin.Materials[g].face_index_array[l].i1;
                    indexs[1] = uhdbin.Materials[g].face_index_array[l].i2;
                    indexs[2] = uhdbin.Materials[g].face_index_array[l].i3;
                    for (int i = 0; i < indexs.Length; i++)
                    {
                        float vx = uhdbin.Vertex_Position_Array[indexs[i]].vx / CONSTs.GLOBAL_POSITION_SCALE;
                        float vy = uhdbin.Vertex_Position_Array[indexs[i]].vy / CONSTs.GLOBAL_POSITION_SCALE;
                        float vz = uhdbin.Vertex_Position_Array[indexs[i]].vz / CONSTs.GLOBAL_POSITION_SCALE * -1;

                        float nx = uhdbin.Vertex_Normal_Array[indexs[i]].nx / NORMAL_FIX;
                        float ny = uhdbin.Vertex_Normal_Array[indexs[i]].ny / NORMAL_FIX;
                        float nz = uhdbin.Vertex_Normal_Array[indexs[i]].nz / NORMAL_FIX * -1;

                        float tu = uhdbin.Vertex_UV_Array[indexs[i]].tu;
                        float tv = (uhdbin.Vertex_UV_Array[indexs[i]].tv -1) * -1;

                        string res = "0"
                        + " " + vx.ToString("F9", inv)
                        + " " + vz.ToString("F9", inv)
                        + " " + vy.ToString("F9", inv)
                        + " " + nx.ToString("F9", inv)
                        + " " + nz.ToString("F9", inv)
                        + " " + ny.ToString("F9", inv)
                        + " " + tu.ToString("F9", inv)
                        + " " + tv.ToString("F9", inv);

                        if (uhdbin.WeightMaps != null && uhdbin.WeightIndex.Length != 0)
                        {
                            ushort indexw = uhdbin.WeightIndex[indexs[i]];

                            int links = uhdbin.WeightMaps[indexw].count;

                            res += " " + links;

                            if (links >= 1)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId1 + " " + (uhdbin.WeightMaps[indexw].weight1 / 100f).ToString("F9", inv);
                            }

                            if (links >= 2)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId2 + " " + (uhdbin.WeightMaps[indexw].weight2 / 100f).ToString("F9", inv);
                            }

                            if (links >= 3)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId3 + " " + (uhdbin.WeightMaps[indexw].weight3 / 100f).ToString("F9", inv);
                            }

                        }
                        else
                        {

                            res += " 0";
                        }


                        text.WriteLine(res);
                    }


                }

            }

            text.WriteLine("end");
            text.WriteLine("// RE4_UHD_BIN_TOOL" + Environment.NewLine +
                   "// by: JADERLINK" + Environment.NewLine +
                  $"// Version {Program.VERSION}");
            text.Close();
        }

        public static void CreateOBJ(UhdBIN uhdbin, string baseDiretory, string baseFileName)
        {
            float NORMAL_FIX = ReturnsNormalsFixValue(uhdbin.Header.texture2_flags);

            var inv = System.Globalization.CultureInfo.InvariantCulture;

            var obj = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".obj")).CreateText();

            obj.WriteLine(Program.headerText());

            obj.WriteLine("mtllib " + baseFileName + ".mtl");

            for (int i = 0; i < uhdbin.Vertex_Position_Array.Length; i++)
            {
                float vx = uhdbin.Vertex_Position_Array[i].vx / CONSTs.GLOBAL_POSITION_SCALE;
                float vy = uhdbin.Vertex_Position_Array[i].vy / CONSTs.GLOBAL_POSITION_SCALE;
                float vz = uhdbin.Vertex_Position_Array[i].vz / CONSTs.GLOBAL_POSITION_SCALE;
                obj.WriteLine("v " + vx.ToString("F9", inv) + " " + vy.ToString("F9", inv) + " " + vz.ToString("F9", inv));
            }

            for (int i = 0; i < uhdbin.Vertex_Normal_Array.Length; i++)
            {
                float nx = uhdbin.Vertex_Normal_Array[i].nx / NORMAL_FIX;
                float ny = uhdbin.Vertex_Normal_Array[i].ny / NORMAL_FIX;
                float nz = uhdbin.Vertex_Normal_Array[i].nz / NORMAL_FIX;
                obj.WriteLine("vn " + nx.ToString("F9", inv) + " " + ny.ToString("F9", inv) + " " + nz.ToString("F9", inv));
            }

            for (int i = 0; i < uhdbin.Vertex_UV_Array.Length; i++)
            {
                float tu = uhdbin.Vertex_UV_Array[i].tu;
                float tv = (uhdbin.Vertex_UV_Array[i].tv -1) *-1;
                obj.WriteLine("vt " + tu.ToString("F9", inv) + " " + tv.ToString("F9", inv));
            }


            for (int g = 0; g < uhdbin.Materials.Length; g++)
            {
                obj.WriteLine("g " + CONSTs.UHD_MATERIAL + g.ToString("D3"));
                obj.WriteLine("usemtl " + CONSTs.UHD_MATERIAL + g.ToString("D3"));

                for (int i = 0; i < uhdbin.Materials[g].face_index_array.Length; i++)
                {

                    string a = (uhdbin.Materials[g].face_index_array[i].i1 + 1).ToString();
                    string b = (uhdbin.Materials[g].face_index_array[i].i2 + 1).ToString();
                    string c = (uhdbin.Materials[g].face_index_array[i].i3 + 1).ToString();

                    obj.WriteLine("f " + a + "/" + a + "/" + a
                                 + " " + b + "/" + b + "/" + b
                                 + " " + c + "/" + c + "/" + c);
                }

            }

            obj.Close();
        }

        public static void CreateIdxUhdTpl(UhdTPL uhdtpl, string baseDiretory, string baseFileName) 
        {
            var inv = System.Globalization.CultureInfo.InvariantCulture;

            TextWriter text = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".idxuhdtpl")).CreateText();
            text.WriteLine(Program.headerText());
            text.WriteLine();
            text.WriteLine();

            for (int i = 0; i < uhdtpl.TplArray.Length; i++)
            {
                text.WriteLine("TPL_" + i.ToString("D3"));

                text.WriteLine("PackID:" + uhdtpl.TplArray[i].PackID.ToString("X8"));
                text.WriteLine("TextureID:" + uhdtpl.TplArray[i].TextureID.ToString("D4"));

                text.WriteLine("PixelFormatType:" + uhdtpl.TplArray[i].PixelFormatType.ToString("X2"));
                text.WriteLine("width:" + uhdtpl.TplArray[i].width);
                text.WriteLine("height:" + uhdtpl.TplArray[i].height);

                if (uhdtpl.TplArray[i].wrap_s != 1)
                {
                    text.WriteLine("wrap_s:" + uhdtpl.TplArray[i].wrap_s);
                }

                if (uhdtpl.TplArray[i].wrap_t != 1)
                {
                    text.WriteLine("wrap_t:" + uhdtpl.TplArray[i].wrap_t);
                }

                if (uhdtpl.TplArray[i].min_filter != 1)
                {
                    text.WriteLine("min_filter:" + uhdtpl.TplArray[i].min_filter);
                }

                if (uhdtpl.TplArray[i].mag_filter != 1)
                {
                    text.WriteLine("mag_filter:" + uhdtpl.TplArray[i].mag_filter);
                }

                if (uhdtpl.TplArray[i].lod_bias != 0)
                {
                    text.WriteLine("lod_bias:" + uhdtpl.TplArray[i].lod_bias.ToString("f6", inv));
                }

                if (uhdtpl.TplArray[i].enable_lod != 0)
                {
                    text.WriteLine("enable_lod:" + uhdtpl.TplArray[i].enable_lod);
                }

                if (uhdtpl.TplArray[i].min_lod != 0)
                {
                    text.WriteLine("min_lod:" + uhdtpl.TplArray[i].min_lod);
                }

                if (uhdtpl.TplArray[i].max_lod != 0)
                {
                    text.WriteLine("max_lod:" + uhdtpl.TplArray[i].max_lod);
                }

                if (uhdtpl.TplArray[i].is_compressed != 0)
                {
                    text.WriteLine("is_compressed:" + uhdtpl.TplArray[i].is_compressed);
                }

                text.WriteLine();
                text.WriteLine();
            }


            text.Close();
        }

        public static void CreateIdxMaterial(IdxMaterial idxmaterial, string baseDiretory, string baseFileName) 
        {

            TextWriter text = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".idxmaterial")).CreateText();
            text.WriteLine(Program.headerText());
            text.WriteLine();
            text.WriteLine();

            foreach (var mat in idxmaterial.MaterialDic)
            {
                text.WriteLine("UseMaterial:" + mat.Key);

                if (mat.Value.unk_min_11 != 0)
                {
                    text.WriteLine("unk_min_11:" + mat.Value.unk_min_11);
                }
                if (mat.Value.unk_min_10 != 0)
                {
                    text.WriteLine("unk_min_10:" + mat.Value.unk_min_10);
                }
                if (mat.Value.unk_min_09 != 0)
                {
                    text.WriteLine("unk_min_09:" + mat.Value.unk_min_09);
                }
                if (mat.Value.unk_min_08 != 0)
                {
                    text.WriteLine("unk_min_08:" + mat.Value.unk_min_08);
                }
                if (mat.Value.unk_min_07 != 0)
                {
                    text.WriteLine("unk_min_07:" + mat.Value.unk_min_07);
                }
                if (mat.Value.unk_min_06 != 0)
                {
                    text.WriteLine("unk_min_06:" + mat.Value.unk_min_06);
                }
                if (mat.Value.unk_min_05 != 0)
                {
                    text.WriteLine("unk_min_05:" + mat.Value.unk_min_05);
                }
                if (mat.Value.unk_min_04 != 0)
                {
                    text.WriteLine("unk_min_04:" + mat.Value.unk_min_04);
                }
                if (mat.Value.unk_min_03 != 0)
                {
                    text.WriteLine("unk_min_03:" + mat.Value.unk_min_03);
                }
                if (mat.Value.unk_min_02 != 0)
                {
                    text.WriteLine("unk_min_02:" + mat.Value.unk_min_02);
                }
                if (mat.Value.unk_min_01 != 0)
                {
                    text.WriteLine("unk_min_01:" + mat.Value.unk_min_01);
                }

                text.WriteLine("material_flag:" + mat.Value.material_flag.ToString("X2"));
                text.WriteLine("diffuse_map:" + mat.Value.diffuse_map);
                text.WriteLine("bump_map:" + mat.Value.bump_map);
                text.WriteLine("opacity_map:" + mat.Value.opacity_map);
                text.WriteLine("generic_specular_map:" + mat.Value.generic_specular_map);
                text.WriteLine("intensity_specular_r:" + mat.Value.intensity_specular_r);
                text.WriteLine("intensity_specular_g:" + mat.Value.intensity_specular_g);
                text.WriteLine("intensity_specular_b:" + mat.Value.intensity_specular_b);
                text.WriteLine("unk_08:" + mat.Value.unk_08);
                text.WriteLine("unk_09:" + mat.Value.unk_09);
                text.WriteLine("specular_scale:" + mat.Value.specular_scale.ToString("X2"));
                text.WriteLine("unk_11:" + mat.Value.unk_11);
                text.WriteLine("custom_specular_map:" + mat.Value.custom_specular_map);

                text.WriteLine();
                text.WriteLine();
            }


            text.Close();
        }

        public static void CreateMTL(IdxMtl idxmtl, string baseDiretory, string baseFileName)
        {   
            var inv = System.Globalization.CultureInfo.InvariantCulture;

            TextWriter text = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".mtl")).CreateText();
            text.WriteLine(Program.headerText());
            text.WriteLine();
            text.WriteLine();

            foreach (var item in idxmtl.MtlDic)
            {
                text.WriteLine("newmtl " + item.Key);
                text.WriteLine("Ka 1.000 1.000 1.000");
                text.WriteLine("Kd 1.000 1.000 1.000");
                text.WriteLine("Ks " + item.Value.Ks);
                text.WriteLine("Ns 0");
                text.WriteLine("d 1");
                text.WriteLine("map_Kd " + item.Value.map_Kd);

                if (item.Value.map_Bump != null)
                {
                    text.WriteLine("map_Bump " + item.Value.map_Bump);
                    text.WriteLine("Bump " + item.Value.map_Bump);
                }

                if (item.Value.map_d != null)
                {
                    text.WriteLine("map_d " + item.Value.map_d);
                }

                if (item.Value.ref_specular_map != null)
                {
                    byte x = (byte)((item.Value.specular_scale & 0xF0) >> 4);
                    byte y = (byte)(item.Value.specular_scale & 0x0F);
                    float fx = x + 1f;
                    float fy = y + 1f;

                    text.WriteLine("map_Ns -s " + fx.ToString("f6", inv)
                        + " " + fy.ToString("f6", inv) + " 1 "+ item.Value.ref_specular_map); //map_ks
                }

                text.WriteLine();
                text.WriteLine();
            }



            text.Close();
        }

        public static void CreateIdxUhdBin(UhdBIN uhdbin, string baseDiretory, string baseFileName) 
        {

            var idx = new FileInfo(Path.Combine(baseDiretory, baseFileName + ".idxuhdbin")).CreateText();
            idx.WriteLine(Program.headerText());

            //idx.WriteLine();
            //idx.WriteLine();
            //idx.WriteLine("#info:");
            //idx.WriteLine("#version_flags:" + uhdbin.Header.version_flags.ToString("X8"));
            //idx.WriteLine("#texture1_flags:" + uhdbin.Header.texture1_flags.ToString("X4"));
            //idx.WriteLine("#texture2_flags:" + uhdbin.Header.texture2_flags.ToString("X4"));
            //idx.WriteLine("#vertex_scale:" + uhdbin.Header.vertex_scale);
            //idx.WriteLine("#TPL_count:" + uhdbin.Header.TPL_count);


            idx.WriteLine();
            idx.WriteLine();
            idx.WriteLine("CompressVertices:True");
            idx.WriteLine("UseExtendedNormals:" + ReturnsIfItIsNormalsExtended(uhdbin.Header.texture2_flags));
            idx.WriteLine("UseWeightMap:" + (uhdbin.Header.weight_count != 0));
            idx.WriteLine("EnableAdjacentBoneTag:" + ReturnsEnableAdjacentBoneTag(uhdbin.Header.texture1_flags));
            idx.WriteLine("EnableBonepairTag:" + ReturnsEnableBonepairTag(uhdbin.Header.texture1_flags)); 


            idx.WriteLine();
            idx.WriteLine();
            idx.WriteLine(": ## Bones ##");
            idx.WriteLine("ObjFileUseBone:0");
            idx.WriteLine(": BonesCount in decimal value");
            idx.WriteLine("BonesCount:" + uhdbin.Bones.Length.ToString());
            idx.WriteLine(": BoneLines -> 16 bytes in hex");
            for (int i = 0; i < uhdbin.Bones.Length; i++)
            {
                idx.WriteLine("BoneLine_" + i + ":" + BitConverter.ToString(uhdbin.Bones[i].boneLine).Replace("-", ""));
            }


            if (uhdbin.bonepairLines != null && uhdbin.bonepairLines.Length != 0)
            {
                idx.WriteLine();
                idx.WriteLine();
                idx.WriteLine(": ## bonepair ##");
                idx.WriteLine(": bonepairCount in decimal value");
                idx.WriteLine("bonepairCount:" + uhdbin.bonepairLines.Length.ToString());

                idx.WriteLine(": bonepairLines -> 8 bytes in hex");
                for (int i = 0; i < uhdbin.bonepairLines.Length; i++)
                {
                    idx.WriteLine("bonepairLine_" + i + ":" + BitConverter.ToString(uhdbin.bonepairLines[i]).Replace("-", ""));
                }
            }

            if (uhdbin.adjacent_bone != null && uhdbin.adjacent_bone.Length != 0)
            {
                idx.WriteLine();
                idx.WriteLine();
                idx.WriteLine(": ## adjacentBone ##");
                idx.WriteLine(": adjacentBoneCount in decimal value");
                idx.WriteLine("adjacentBoneCount:" + uhdbin.adjacent_bone.Length.ToString());

                idx.WriteLine(": adjacentBoneLines -> ushort in hex");

                for (int i = 0; i < uhdbin.adjacent_bone.Length; i++)
                {
                    if (uhdbin.adjacent_bone[i] != 0xFFFF)
                    {
                        idx.WriteLine("adjacentBoneLine_" + i + ":" + uhdbin.adjacent_bone[i].ToString("X4"));
                    }
                }
            }




            idx.Close();
        }


        //no arquivo bin, tem uma flag(bit), que define o proporção da vertex_normal (no caso a normal existe em duas escala)
        private static bool ReturnsIfItIsNormalsExtended(ushort texture2_flag) 
        {
            return (texture2_flag & 0x2000) == 0x2000; // 0x2000 representa qual é o bit ativa para as normals extendidas (escala maior)


        }

        // o metodo de cima verifica, esse metodo retorna no valor a ser usado.
        private static float ReturnsNormalsFixValue(ushort texture2_flag) 
        {
            return ReturnsIfItIsNormalsExtended(texture2_flag) ? CONSTs.GLOBAL_NORMAL_FIX_EXTENDED : CONSTs.GLOBAL_NORMAL_FIX_REDUCED;
        }


        //----
        private static bool ReturnsEnableBonepairTag(ushort texture1_flag) 
        {
            return (texture1_flag & 0x0100) == 0x0100; // BonepairTag
        }

        private static bool ReturnsEnableAdjacentBoneTag(ushort texture1_flag)
        {
            return (texture1_flag & 0x0200) == 0x0200; // AdjacentBoneTag
        }

    }
}
