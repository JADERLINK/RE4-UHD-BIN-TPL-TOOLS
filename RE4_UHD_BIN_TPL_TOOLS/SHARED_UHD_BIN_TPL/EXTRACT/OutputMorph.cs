using SHARED_TOOLS.ALL;
using SHARED_UHD_BIN_TPL.ALL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SHARED_UHD_BIN_TPL.EXTRACT
{
    public static class OutputMorph
    {
        private static float get_scale_from_vertex_scale(byte vertex_scale)
        {
            return (float)Math.Pow(2, vertex_scale);
        }

        public static void CreateMorphFiles(UhdBIN bin, MorphBIN morph, string baseDirectory, string baseFileName) 
        {
            if (morph != null)
            {
                for (int i = 0; i < morph.MorphGroups.Length; i++)
                {
                    string name = baseFileName + "_morph_" + i.ToString("D2");
                    CreateMorphOBJ(bin, morph.MorphGroups[i], baseDirectory, name, baseFileName);
                }

                CreateMorphVTA(bin, morph, baseDirectory, baseFileName);
            }    
        }

        private static void CreateMorphOBJ(UhdBIN uhdbin, MorphGroup morphGroup, string baseDirectory, string baseFileName, string mtlName)
        {
            var obj = new FileInfo(Path.Combine(baseDirectory, baseFileName + ".obj")).CreateText();

            obj.WriteLine(SHARED_TOOLS.Shared.HeaderText());

            obj.WriteLine("mtllib " + mtlName + ".mtl");

            float extraScale = get_scale_from_vertex_scale(uhdbin.Header.vertex_scale);

            Dictionary<int, (short x, short y, short z)> morph = new Dictionary<int, (short x, short y, short z)>();
            foreach (var item in morphGroup.Morph_Vertex)
            {
                if ( ! morph.ContainsKey(item.VertexID))
                {
                    morph.Add(item.VertexID, (item.posX, item.posY, item.posZ));
                }
            }

            for (int i = 0; i < uhdbin.Vertex_Position_Array.Length; i++)
            {
                float e_x = (morph.ContainsKey(i) ? morph[i].x : 0) / extraScale;
                float e_y = (morph.ContainsKey(i) ? morph[i].y : 0) / extraScale;
                float e_z = (morph.ContainsKey(i) ? morph[i].z : 0) / extraScale;

                float vx = (uhdbin.Vertex_Position_Array[i].vx + e_x) / CONSTs.GLOBAL_POSITION_SCALE;
                float vy = (uhdbin.Vertex_Position_Array[i].vy + e_y) / CONSTs.GLOBAL_POSITION_SCALE;
                float vz = (uhdbin.Vertex_Position_Array[i].vz + e_z) / CONSTs.GLOBAL_POSITION_SCALE;

                string v = "v " + vx.ToFloatString() + " " + vy.ToFloatString() + " " + vz.ToFloatString();

                if (uhdbin.Header.ReturnsHasEnableVertexColorsTag() && uhdbin.Vertex_Color_Array.Length > i)
                {
                    float r = uhdbin.Vertex_Color_Array[i].r / 255f;
                    float g = uhdbin.Vertex_Color_Array[i].g / 255f;
                    float b = uhdbin.Vertex_Color_Array[i].b / 255f;
                    float a = uhdbin.Vertex_Color_Array[i].a / 255f;

                    v += " " + r.ToFloatString() + " " + g.ToFloatString() + " " + b.ToFloatString() + " " + a.ToFloatString();
                }

                obj.WriteLine(v);
            }

            for (int i = 0; i < uhdbin.Vertex_Normal_Array.Length; i++)
            {
                float nx = uhdbin.Vertex_Normal_Array[i].nx;
                float ny = uhdbin.Vertex_Normal_Array[i].ny;
                float nz = uhdbin.Vertex_Normal_Array[i].nz;

                float NORMAL_FIX = (float)Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                NORMAL_FIX = (NORMAL_FIX == 0) ? 1 : NORMAL_FIX;
                nx /= NORMAL_FIX;
                ny /= NORMAL_FIX;
                nz /= NORMAL_FIX;

                obj.WriteLine("vn " + nx.ToFloatString() + " " + ny.ToFloatString() + " " + nz.ToFloatString());
            }

            for (int i = 0; i < uhdbin.Vertex_UV_Array.Length; i++)
            {
                float tu = uhdbin.Vertex_UV_Array[i].tu;
                float tv = (uhdbin.Vertex_UV_Array[i].tv - 1) * -1;
                obj.WriteLine("vt " + tu.ToFloatString() + " " + tv.ToFloatString());
            }


            for (int g = 0; g < uhdbin.Materials.Length; g++)
            {
                obj.WriteLine("g " + CONSTs.MATERIAL + g.ToString("D3"));
                obj.WriteLine("usemtl " + CONSTs.MATERIAL + g.ToString("D3"));

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

        private static void CreateMorphVTA(UhdBIN uhdbin, MorphBIN morph, string baseDirectory, string baseFileName)
        {
            TextWriter text = new FileInfo(Path.Combine(baseDirectory, baseFileName + ".vta")).CreateText();
            text.WriteLine("version 1");
            text.WriteLine("nodes");

            //Bones Fix
            (uint BoneID, short BoneParent)[] FixedBones = new (uint BoneID, short BoneParent)[uhdbin.Bones.Length];

            // Bone ID, number of times found
            Dictionary<byte, int> BoneCheck = new Dictionary<byte, int>();
            for (int i = uhdbin.Bones.Length - 1; i >= 0; i--)
            {
                byte InBoneID = uhdbin.Bones[i].BoneID;
                uint OutBoneID = InBoneID;
                if (BoneCheck.ContainsKey(InBoneID))
                {
                    OutBoneID += (uint)(0x100u * BoneCheck[InBoneID]);
                    BoneCheck[InBoneID]++;
                }
                else
                {
                    BoneCheck.Add(InBoneID, 1);
                }

                short BoneParent = uhdbin.Bones[i].BoneParent;
                if (BoneParent == 0xFF)
                {
                    BoneParent = -1;
                }

                FixedBones[i] = (OutBoneID, BoneParent);
            }

            for (int i = 0; i < FixedBones.Length; i++)
            {
                text.WriteLine(FixedBones[i].BoneID + " \"BONE_" + FixedBones[i].BoneID.ToString("D3") + "\" " + FixedBones[i].BoneParent);
            }

            text.WriteLine("end");

            text.WriteLine("skeleton");
            text.WriteLine("time 0");

            for (int m = 0; m < morph.MorphGroups.Length; m++)
            {
                text.WriteLine("time " + (m + 1));
            }

            text.WriteLine("end");

            text.WriteLine("vertexanimation");

            {
                int indexcounter = 0;

                text.WriteLine("time 0");
                for (int g = 0; g < uhdbin.Materials.Length; g++)
                {
                    for (int l = 0; l < uhdbin.Materials[g].face_index_array.Length; l++)
                    {
                        int[] indexs = new int[3];
                        indexs[0] = uhdbin.Materials[g].face_index_array[l].i1;
                        indexs[1] = uhdbin.Materials[g].face_index_array[l].i2;
                        indexs[2] = uhdbin.Materials[g].face_index_array[l].i3;

                        for (int i = 0; i < indexs.Length; i++)
                        {
                            float vx = uhdbin.Vertex_Position_Array[indexs[i]].vx / CONSTs.GLOBAL_POSITION_SCALE;
                            float vy = uhdbin.Vertex_Position_Array[indexs[i]].vy / CONSTs.GLOBAL_POSITION_SCALE;
                            float vz = uhdbin.Vertex_Position_Array[indexs[i]].vz / CONSTs.GLOBAL_POSITION_SCALE * -1;

                            float nx = uhdbin.Vertex_Normal_Array[indexs[i]].nx;
                            float ny = uhdbin.Vertex_Normal_Array[indexs[i]].ny;
                            float nz = uhdbin.Vertex_Normal_Array[indexs[i]].nz;

                            float NORMAL_FIX = (float)Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                            NORMAL_FIX = (NORMAL_FIX == 0) ? 1 : NORMAL_FIX;
                            nx /= NORMAL_FIX;
                            ny /= NORMAL_FIX;
                            nz /= NORMAL_FIX * -1;

                            string res = indexcounter
                            + " " + vx.ToFloatString()
                            + " " + vz.ToFloatString()
                            + " " + vy.ToFloatString()
                            + " " + nx.ToFloatString()
                            + " " + nz.ToFloatString()
                            + " " + ny.ToFloatString();

                            text.WriteLine(res);

                            indexcounter++;
                        }
                    }

                }
            }

            float extraScale = get_scale_from_vertex_scale(uhdbin.Header.vertex_scale);

            for (int m = 0; m < morph.MorphGroups.Length; m++)
            {
                Dictionary<int, (short x, short y, short z)> morphDic = new Dictionary<int, (short x, short y, short z)>();
                foreach (var item in morph.MorphGroups[m].Morph_Vertex)
                {
                    if (!morphDic.ContainsKey(item.VertexID))
                    {
                        morphDic.Add(item.VertexID, (item.posX, item.posY, item.posZ));
                    }
                }

                text.WriteLine("time " + (m + 1));

                int indexcounter = 0;

                for (int g = 0; g < uhdbin.Materials.Length; g++)
                {
                    for (int l = 0; l < uhdbin.Materials[g].face_index_array.Length; l++)
                    {
                        int[] indexs = new int[3];
                        indexs[0] = uhdbin.Materials[g].face_index_array[l].i1;
                        indexs[1] = uhdbin.Materials[g].face_index_array[l].i2;
                        indexs[2] = uhdbin.Materials[g].face_index_array[l].i3;

                        for (int i = 0; i < indexs.Length; i++)
                        {
                            float e_x = (morphDic.ContainsKey(indexs[i]) ? morphDic[indexs[i]].x : 0) / extraScale;
                            float e_y = (morphDic.ContainsKey(indexs[i]) ? morphDic[indexs[i]].y : 0) / extraScale;
                            float e_z = (morphDic.ContainsKey(indexs[i]) ? morphDic[indexs[i]].z : 0) / extraScale;

                            float vx = (uhdbin.Vertex_Position_Array[indexs[i]].vx + e_x) / CONSTs.GLOBAL_POSITION_SCALE;
                            float vy = (uhdbin.Vertex_Position_Array[indexs[i]].vy + e_y) / CONSTs.GLOBAL_POSITION_SCALE;
                            float vz = (uhdbin.Vertex_Position_Array[indexs[i]].vz + e_z) / CONSTs.GLOBAL_POSITION_SCALE * -1;

                            float nx = uhdbin.Vertex_Normal_Array[indexs[i]].nx;
                            float ny = uhdbin.Vertex_Normal_Array[indexs[i]].ny;
                            float nz = uhdbin.Vertex_Normal_Array[indexs[i]].nz;

                            float NORMAL_FIX = (float)Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                            NORMAL_FIX = (NORMAL_FIX == 0) ? 1 : NORMAL_FIX;
                            nx /= NORMAL_FIX;
                            ny /= NORMAL_FIX;
                            nz /= NORMAL_FIX * -1;

                            if (morphDic.ContainsKey(indexs[i]))
                            {
                                string res = indexcounter
                                + " " + vx.ToFloatString()
                                + " " + vz.ToFloatString()
                                + " " + vy.ToFloatString()
                                + " " + nx.ToFloatString()
                                + " " + nz.ToFloatString()
                                + " " + ny.ToFloatString();
                                text.WriteLine(res);
                            }

                            indexcounter++;
                        }
                    }

                }

            }

            text.WriteLine("end");

            text.Write(SHARED_TOOLS.Shared.HeaderTextSmd());
            text.Close();
        }

    }
}
