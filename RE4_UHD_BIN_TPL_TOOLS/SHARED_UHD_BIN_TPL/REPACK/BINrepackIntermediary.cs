using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHARED_UHD_BIN_TPL.REPACK.Structures;
using SHARED_UHD_BIN_TPL.ALL;

namespace SHARED_UHD_BIN_TPL.REPACK
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


                        vertex.BoneID1 = (byte)(ushort)item.Value.Faces[i][iv].WeightMap.BoneID1;
                        vertex.BoneID2 = (byte)(ushort)item.Value.Faces[i][iv].WeightMap.BoneID2;
                        vertex.BoneID3 = (byte)(ushort)item.Value.Faces[i][iv].WeightMap.BoneID3;

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

    }
}
