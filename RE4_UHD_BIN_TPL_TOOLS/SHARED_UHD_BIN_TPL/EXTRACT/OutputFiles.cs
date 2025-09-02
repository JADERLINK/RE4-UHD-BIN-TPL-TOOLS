using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SHARED_UHD_BIN_TPL.ALL;
using SHARED_TOOLS.ALL;

namespace SHARED_UHD_BIN_TPL.EXTRACT
{
    public static class OutputFiles
    {

        //Studiomdl Data
        public static void CreateSMD(UhdBIN uhdbin, string baseDirectory, string baseFileName)
        {
            TextWriter text = new FileInfo(Path.Combine(baseDirectory, baseFileName + ".smd")).CreateText();
            text.WriteLine("version 1");
            text.WriteLine("nodes");

            //Bones Fix
            (uint BoneID, short BoneParent, float p1, float p2, float p3)[] FixedBones = new (uint BoneID, short BoneParent, float p1, float p2, float p3)[uhdbin.Bones.Length];

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
                
                float p1 = uhdbin.Bones[i].PositionX / CONSTs.GLOBAL_POSITION_SCALE;
                float p2 = uhdbin.Bones[i].PositionZ * -1 / CONSTs.GLOBAL_POSITION_SCALE;
                float p3 = uhdbin.Bones[i].PositionY / CONSTs.GLOBAL_POSITION_SCALE;

                FixedBones[i] = (OutBoneID, BoneParent, p1, p2, p3);
            }

            for (int i = 0; i < FixedBones.Length; i++)
            {
                text.WriteLine(FixedBones[i].BoneID + " \"BONE_" + FixedBones[i].BoneID.ToString("D3") + "\" " + FixedBones[i].BoneParent);
            }

            text.WriteLine("end");

            text.WriteLine("skeleton");
            text.WriteLine("time 0");

            for (int i = 0; i < FixedBones.Length; i++)
            {
                text.WriteLine(FixedBones[i].BoneID + "  " +
                               FixedBones[i].p1.ToFloatString() + " " +
                               FixedBones[i].p2.ToFloatString() + " " +
                               FixedBones[i].p3.ToFloatString() + "  0.0 0.0 0.0");
            }

            text.WriteLine("end");

            text.WriteLine("triangles");

            for (int g = 0; g < uhdbin.Materials.Length; g++)
            {
                for (int l = 0; l < uhdbin.Materials[g].face_index_array.Length; l++)
                {
                    text.WriteLine(CONSTs.MATERIAL + g.ToString("D3"));

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

                        float tu = uhdbin.Vertex_UV_Array[indexs[i]].tu;
                        float tv = (uhdbin.Vertex_UV_Array[indexs[i]].tv - 1) * -1;

                        string res = "0"
                        + " " + vx.ToFloatString()
                        + " " + vz.ToFloatString()
                        + " " + vy.ToFloatString()
                        + " " + nx.ToFloatString()
                        + " " + nz.ToFloatString()
                        + " " + ny.ToFloatString()
                        + " " + tu.ToFloatString()
                        + " " + tv.ToFloatString();

                        if (uhdbin.WeightMaps != null && uhdbin.WeightIndex.Length != 0)
                        {
                            ushort indexw = uhdbin.WeightIndex[indexs[i]];

                            int links = uhdbin.WeightMaps[indexw].count;

                            res += " " + links;

                            if (links >= 1)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId1 + " " + (uhdbin.WeightMaps[indexw].weight1 / 100f).ToFloatString();
                            }

                            if (links >= 2)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId2 + " " + (uhdbin.WeightMaps[indexw].weight2 / 100f).ToFloatString();
                            }

                            if (links >= 3)
                            {
                                res += " " + uhdbin.WeightMaps[indexw].boneId3 + " " + (uhdbin.WeightMaps[indexw].weight3 / 100f).ToFloatString();
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
            text.Write(SHARED_TOOLS.Shared.HeaderTextSmd());
            text.Close();
        }

        public static void CreateOBJ(UhdBIN uhdbin, string baseDirectory, string baseFileName)
        {
            var obj = new FileInfo(Path.Combine(baseDirectory, baseFileName + ".obj")).CreateText();

            obj.WriteLine(SHARED_TOOLS.Shared.HeaderText());

            obj.WriteLine("mtllib " + baseFileName + ".mtl");

            for (int i = 0; i < uhdbin.Vertex_Position_Array.Length; i++)
            {
                float vx = uhdbin.Vertex_Position_Array[i].vx / CONSTs.GLOBAL_POSITION_SCALE;
                float vy = uhdbin.Vertex_Position_Array[i].vy / CONSTs.GLOBAL_POSITION_SCALE;
                float vz = uhdbin.Vertex_Position_Array[i].vz / CONSTs.GLOBAL_POSITION_SCALE;

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
                float tv = (uhdbin.Vertex_UV_Array[i].tv -1) *-1;
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

        public static void CreateIdxBin(UhdBIN uhdbin, string baseDirectory, string baseFileName) 
        {

            var idx = new FileInfo(Path.Combine(baseDirectory, baseFileName + ".idxuubin")).CreateText();
            idx.WriteLine(SHARED_TOOLS.Shared.HeaderText());
            idx.WriteLine();
            idx.WriteLine();

            idx.WriteLine("UseAlternativeNormals:" + uhdbin.Header.ReturnsHasNormalsAlternativeTag());
            idx.WriteLine("UseWeightMap:" + (uhdbin.Header.weightmap_count != 0));
            idx.WriteLine("EnableAdjacentBoneTag:" + uhdbin.Header.ReturnsHasEnableAdjacentBoneTag());
            idx.WriteLine("EnableBonepairTag:" + uhdbin.Header.ReturnsHasEnableBonepairTag());
            idx.WriteLine("UseVertexColor:False");
            idx.WriteLine("ObjFileUseBone:" + uhdbin.Bones.Min(x => x.BoneID).ToString());

            idx.WriteLine();
            idx.WriteLine();
            idx.WriteLine("## BoneLine: <boneId:number> <ParentId:number> <x:float> <-z:float> <y:float>");
            for (int i = 0; i < uhdbin.Bones.Length; i++)
            {
                float p1 = uhdbin.Bones[i].PositionX / CONSTs.GLOBAL_POSITION_SCALE;
                float p2 = uhdbin.Bones[i].PositionZ * -1 / CONSTs.GLOBAL_POSITION_SCALE;
                float p3 = uhdbin.Bones[i].PositionY / CONSTs.GLOBAL_POSITION_SCALE;

                idx.WriteLine("BoneLine:" +
                    uhdbin.Bones[i].BoneID.ToString().PadLeft(4) + " " +
                    (uhdbin.Bones[i].BoneParent != 0xFF ? uhdbin.Bones[i].BoneParent : -1).ToString().PadLeft(4) + "   " +
                    p1.ToFloatString() + "  " +
                    p2.ToFloatString() + "  " +
                    p3.ToFloatString()
                    );
            }


            if (uhdbin.BonePairs != null && uhdbin.BonePairs.Length != 0)
            {
                idx.WriteLine();
                idx.WriteLine();
                idx.WriteLine("## BonePair: <bone1:number> <bone2:number> <bone3:number> <unk:number>");

                for (int i = 0; i < uhdbin.BonePairs.Length; i++)
                {
                    idx.WriteLine("BonePair:" +
                       uhdbin.BonePairs[i].Bone1.ToString().PadLeft(4) + " " +
                       uhdbin.BonePairs[i].Bone2.ToString().PadLeft(4) + " " +
                       uhdbin.BonePairs[i].Bone3.ToString().PadLeft(4) + " " +
                       uhdbin.BonePairs[i].Bone4.ToString().PadLeft(4)
                       );
                }
            }

            idx.Close();
        }
    }
}
