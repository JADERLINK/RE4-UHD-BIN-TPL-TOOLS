using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SHARED_UHD_BIN.REPACK.Structures;
using SHARED_UHD_BIN.ALL;
using SimpleEndianBinaryIO;

namespace SHARED_UHD_BIN.REPACK
{
    public static partial class BinRepack
    {
        public static void RepackSMD(Stream smdFile, bool CompressVertices, out IntermediaryStructure intermediaryStructure, out FinalBoneLine[] bones, bool UseExtendedNormals, Endianness endianness)
        {
            //carrega o arquivo smd;
            StreamReader stream = null;
            SMD_READER_LIB.SMD smd = null;

            try
            {
                stream = new StreamReader(smdFile, Encoding.ASCII);
                smd = SMD_READER_LIB.SmdReader.Reader(stream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                } 
            }

            //lista de materiais usados no modelo
            HashSet<string> ModelMaterials = new HashSet<string>();

            //--- crio a primeira estrutura:

            StartStructure startStructure = new StartStructure();

            Vector4 color = new Vector4(1, 1, 1, 1);

            for (int i = 0; i < smd.Triangles.Count; i++)
            {
                string materialNameInvariant = smd.Triangles[i].Material.ToUpperInvariant().Trim();

                List<StartVertex> verticeList = new List<StartVertex>();

                for (int t = 0; t < smd.Triangles[i].Vertexs.Count; t++)
                {

                    StartVertex vertice = new StartVertex();
                    vertice.Color = color;


                    Vector3 position = new Vector3(
                            smd.Triangles[i].Vertexs[t].PosX,
                            smd.Triangles[i].Vertexs[t].PosZ,
                            smd.Triangles[i].Vertexs[t].PosY * -1
                            );

                    vertice.Position = position;

                    float nx = smd.Triangles[i].Vertexs[t].NormX;
                    float ny = smd.Triangles[i].Vertexs[t].NormZ;
                    float nz = smd.Triangles[i].Vertexs[t].NormY * -1;
                    float NORMAL_FIX = (float)Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                    NORMAL_FIX = (NORMAL_FIX == 0) ? 1 : NORMAL_FIX;
                    nx /= NORMAL_FIX;
                    ny /= NORMAL_FIX;
                    nz /= NORMAL_FIX;

                    vertice.Normal = new Vector3(nx, ny, nz);

                    Vector2 texture = new Vector2(
                    smd.Triangles[i].Vertexs[t].U,
                    ((smd.Triangles[i].Vertexs[t].V - 1) * -1)
                    );

                    vertice.Texture = texture;

                    //cria o objetos com os weight
                    // e corrige a soma total para dar 1

                    if (smd.Triangles[i].Vertexs[t].Links.Count == 0)
                    {
                        StartWeightMap weightMap = new StartWeightMap();
                        weightMap.Links = 1;
                        weightMap.BoneID1 = smd.Triangles[i].Vertexs[t].ParentBone;
                        weightMap.Weight1 = 1f;

                        vertice.WeightMap = weightMap;
                    }
                    else
                    {
                        StartWeightMap weightMap = new StartWeightMap();

                        var links = (from link in smd.Triangles[i].Vertexs[t].Links
                                     orderby link.Weight
                                     select link).ToArray();

                        if (links.Length >= 1)
                        {
                            weightMap.Links = 1;
                            weightMap.BoneID1 = links[0].BoneID;
                            weightMap.Weight1 = links[0].Weight;
                        }
                        if (links.Length >= 2)
                        {
                            weightMap.Links = 2;
                            weightMap.BoneID2 = links[1].BoneID;
                            weightMap.Weight2 = links[1].Weight;
                        }
                        if (links.Length >= 3)
                        {
                            weightMap.Links = 3;
                            weightMap.BoneID3 = links[2].BoneID;
                            weightMap.Weight3 = links[2].Weight;
                        }

                        // verificação para soma total dar 1

                        float sum = weightMap.Weight1 + weightMap.Weight2 + weightMap.Weight3;

                        if (sum > 1  // se por algum motivo aleatorio ficar maior que 1
                            || sum < 1) // ou se caso for menor que 1
                        {
                            float difference = sum - 1; // se for maior diferença é positiva, e se for menor é positiva
                            float average = difference / weightMap.Links; // aqui mantem o sinal da operação

                            if (weightMap.Links >= 1)
                            {
                                weightMap.Weight1 -= average; // se for positivo tem que dimiuir,
                                                              // porem se for negativo tem que aumentar,
                                                              // porem menos com menos da mais, então esta certo.
                            }
                            if (weightMap.Links >= 2)
                            {
                                weightMap.Weight2 -= average;
                            }
                            if (weightMap.Links >= 3)
                            {
                                weightMap.Weight3 -= average;
                            }

                            //re verifica se ainda tem diferença
                            float newSum = weightMap.Weight1 + weightMap.Weight2 + weightMap.Weight3;
                            float newDifference = newSum - 1;

                            if (newDifference != 1)
                            {
                                weightMap.Weight1 -= newDifference;
                            }
                        }

                        vertice.WeightMap = weightMap;
                    }


                    verticeList.Add(vertice);

                }

                if (startStructure.FacesByMaterial.ContainsKey(materialNameInvariant))
                {
                    startStructure.FacesByMaterial[materialNameInvariant].Faces.Add(verticeList);
                }
                else // cria novo
                {
                    ModelMaterials.Add(materialNameInvariant);

                    StartFacesGroup facesGroup = new StartFacesGroup();
                    facesGroup.Faces.Add(verticeList);
                    startStructure.FacesByMaterial.Add(materialNameInvariant, facesGroup);
                }

            }


            // faz a compressão das vertives
            if (CompressVertices == true)
            {
                startStructure.CompressAllFaces();
            }

            // estrutura intermediaria
            intermediaryStructure = MakeIntermediaryStructure(startStructure, UseExtendedNormals);

            //FinalBoneLine é usado os bones do arquivo smd
            bones = GetBoneLines(smd, endianness);
        }

        private static FinalBoneLine[] GetBoneLines(SMD_READER_LIB.SMD smd, Endianness endianness)
        {
            List<FinalBoneLine> bones = new List<FinalBoneLine>();

            SMD_READER_LIB.Time time = (from tt in smd.Times
                                        where tt.ID == 0
                                        select tt).FirstOrDefault();

            for (int i = 0; i < smd.Nodes.Count; i++)
            {

                (float X, float Y, float Z) bonePos = (0, 0, 0);

                if (time != null)
                {
                    SMD_READER_LIB.Skeleton skeleton = (from ss in time.Skeletons
                                                        where ss.BoneID == smd.Nodes[i].ID
                                                        select ss).FirstOrDefault();
                    if (skeleton != null)
                    {
                        bonePos.X = skeleton.PosX * CONSTs.GLOBAL_POSITION_SCALE;
                        bonePos.Y = skeleton.PosZ * CONSTs.GLOBAL_POSITION_SCALE;
                        bonePos.Z = skeleton.PosY * -1 * CONSTs.GLOBAL_POSITION_SCALE;

                        if (bonePos.Z == 0f * -1f) { bonePos.Z = 0; }

                    }
                }

                byte ParentID = (byte)smd.Nodes[i].ParentID;
                if (smd.Nodes[i].ParentID < 0)
                {
                    ParentID = 0xFF;
                }

                bones.Add(new FinalBoneLine((byte)(ushort)smd.Nodes[i].ID, ParentID, bonePos.X, bonePos.Y, bonePos.Z, endianness));
            }

            return bones.ToArray();
        }


    }
}
