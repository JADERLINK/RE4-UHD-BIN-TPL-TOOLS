using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RE4_UHD_BIN_TOOL.REPACK.Structures;

namespace RE4_UHD_BIN_TOOL.REPACK
{
    public static partial class BinRepack
    {
        private static IntermediaryStructure MakeIntermediaryStructure(StartStructure startStructure, bool UseExtendedNormals)
        {
            float NORMAL_FIX = UseExtendedNormals ? CONSTs.GLOBAL_NORMAL_FIX_EXTENDED : CONSTs.GLOBAL_NORMAL_FIX_REDUCED;

            IntermediaryStructure intermediary = new IntermediaryStructure();

            foreach (var item in startStructure.FacesByMaterial)
            {
                IntermediaryMesh mesh = new IntermediaryMesh();

                for (int i = 0; i < item.Value.Faces.Count; i++)
                {
                    IntermediaryFace face = new IntermediaryFace();

                    for (int iv = 0; iv < item.Value.Faces[i].Count; iv++)
                    {
                        IntermediaryVertex vertex = new IntermediaryVertex();

                        vertex.PosX = item.Value.Faces[i][iv].Position.X * CONSTs.GLOBAL_POSITION_SCALE;
                        vertex.PosY = item.Value.Faces[i][iv].Position.Y * CONSTs.GLOBAL_POSITION_SCALE;
                        vertex.PosZ = item.Value.Faces[i][iv].Position.Z * CONSTs.GLOBAL_POSITION_SCALE;

                        vertex.NormalX = item.Value.Faces[i][iv].Normal.X * NORMAL_FIX;
                        vertex.NormalY = item.Value.Faces[i][iv].Normal.Y * NORMAL_FIX;
                        vertex.NormalZ = item.Value.Faces[i][iv].Normal.Z * NORMAL_FIX;

                        vertex.TextureU = item.Value.Faces[i][iv].Texture.U;
                        vertex.TextureV = item.Value.Faces[i][iv].Texture.V;

                        vertex.ColorR = (byte)(item.Value.Faces[i][iv].Color.R * 255);
                        vertex.ColorG = (byte)(item.Value.Faces[i][iv].Color.G * 255);
                        vertex.ColorB = (byte)(item.Value.Faces[i][iv].Color.B * 255);
                        vertex.ColorA = (byte)(item.Value.Faces[i][iv].Color.A * 255);

                        vertex.Links = (byte)item.Value.Faces[i][iv].WeightMap.Links;


                        vertex.BoneID1 = (ushort)item.Value.Faces[i][iv].WeightMap.BoneID1;
                        vertex.BoneID2 = (ushort)item.Value.Faces[i][iv].WeightMap.BoneID2;
                        vertex.BoneID3 = (ushort)item.Value.Faces[i][iv].WeightMap.BoneID3;

                        vertex.Weight1 = (byte)(item.Value.Faces[i][iv].WeightMap.Weight1 * 100);
                        vertex.Weight2 = (byte)(item.Value.Faces[i][iv].WeightMap.Weight2 * 100);
                        vertex.Weight3 = (byte)(item.Value.Faces[i][iv].WeightMap.Weight3 * 100);

                        face.Vertexs.Add(vertex);
                    }

                    mesh.Faces.Add(face);
                }

                mesh.MaterialName = item.Key.ToUpperInvariant();
                intermediary.Groups.Add(mesh.MaterialName, mesh);
            }

            return intermediary;
        }

        public static IntermediaryLevel2 MakeIntermediaryLevel2(IntermediaryStructure intermediaryStructure) 
        {
            IntermediaryLevel2 level2 = new IntermediaryLevel2();

            foreach (var item in intermediaryStructure.Groups)
            {
                level2.Groups.Add(item.Key, MakeIntermediaryLevel2Mesh(item.Value));
            }

            return level2;
        }

        private static IntermediaryLevel2Mesh MakeIntermediaryLevel2Mesh(IntermediaryMesh intermediaryMesh) 
        {
            IntermediaryLevel2Mesh mesh = new IntermediaryLevel2Mesh();
            mesh.MaterialName = intermediaryMesh.MaterialName;

            for (int i = 0; i < intermediaryMesh.Faces.Count; i++)
            {
                ushort count = (ushort)intermediaryMesh.Faces[i].Vertexs.Count;

                if (count == 3) // triangulo
                {
                    var res = (from obj in mesh.Faces
                              where obj.Type == CONSTs.FACE_TYPE_TRIANGLE_LIST && obj.Count < short.MaxValue
                              select obj).ToList();

                    if (res.Count != 0)
                    {
                        res[0].Count += count;
                        res[0].Vertexs.AddRange(intermediaryMesh.Faces[i].Vertexs);
                    }
                    else // é o primeiro tem que colocar um novo.
                    {
                        IntermediaryLevel2Face level2Face = new IntermediaryLevel2Face();
                        level2Face.Count = count;
                        level2Face.Type = CONSTs.FACE_TYPE_TRIANGLE_LIST;
                        level2Face.Vertexs.AddRange(intermediaryMesh.Faces[i].Vertexs);
                        mesh.Faces.Add(level2Face);
                    }

                }
                else if (count == 4) //vai virar guad
                {
                    List<IntermediaryVertex> reordered = new List<IntermediaryVertex>();
                    reordered.Add(intermediaryMesh.Faces[i].Vertexs[3]);
                    reordered.Add(intermediaryMesh.Faces[i].Vertexs[2]);
                    reordered.Add(intermediaryMesh.Faces[i].Vertexs[0]);
                    reordered.Add(intermediaryMesh.Faces[i].Vertexs[1]);

                    var res = (from obj in mesh.Faces
                               where obj.Type == CONSTs.FACE_TYPE_QUAD_LIST && obj.Count < short.MaxValue
                               select obj).ToList();

                    if (res.Count != 0)
                    {
                        res[0].Count += count;
                        res[0].Vertexs.AddRange(reordered);
                    }
                    else // é o primeiro tem que colocar um novo.
                    {
                        IntermediaryLevel2Face level2Face = new IntermediaryLevel2Face();
                        level2Face.Count = count;
                        level2Face.Type = CONSTs.FACE_TYPE_QUAD_LIST;
                        level2Face.Vertexs.AddRange(reordered);
                        mesh.Faces.Add(level2Face);
                    }

                }
                else if (count > 4) // se for maior que 4 é porque é triangle strip
                {
                    IntermediaryLevel2Face level2Face = new IntermediaryLevel2Face();
                    level2Face.Count = count;
                    level2Face.Type = CONSTs.FACE_TYPE_TRIANGLE_STRIP;
                    level2Face.Vertexs.AddRange(intermediaryMesh.Faces[i].Vertexs);
                    mesh.Faces.Add(level2Face);

                }
            }


            return mesh;
        }


    }
}
