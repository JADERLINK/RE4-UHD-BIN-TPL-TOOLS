using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SHARED_UHD_BIN_TPL.REPACK.Structures;
using SimpleEndianBinaryIO;
using SHARED_UHD_BIN_TPL.ALL;

namespace SHARED_UHD_BIN_TPL.REPACK
{
    public static partial class BinRepack
    {
        public static void RepackOBJ(Stream objFile, bool CompressVertices, int ObjFileUseBone, out IntermediaryStructure intermediaryStructure, bool UseExtendedNormals, bool UseColors)
        {
            // load .obj file
            var objLoaderFactory = new ObjLoader.Loader.Loaders.ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            StreamReader streamReader = null;
            ObjLoader.Loader.Loaders.LoadResult arqObj = null;
           
            try
            {
                streamReader = new StreamReader(objFile, Encoding.ASCII);
                arqObj = objLoader.Load(streamReader);
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                streamReader?.Close();
            }

            //--- crio a primeira estrutura:

            StartStructure startStructure = new StartStructure();

            StartWeightMap weightMap = new StartWeightMap(1, ObjFileUseBone, 1, 0, 0, 0, 0);

            for (int iG = 0; iG < arqObj.Groups.Count; iG++)
            {
                string materialNameInvariant = arqObj.Groups[iG].MaterialName.ToUpperInvariant().Trim();

                List<List<StartVertex>> facesList = new List<List<StartVertex>>();

                for (int iF = 0; iF < arqObj.Groups[iG].Faces.Count; iF++)
                {
                    List<StartVertex> face = new List<StartVertex>();

                    for (int iI = 0; iI < arqObj.Groups[iG].Faces[iF].Count; iI++)
                    {
                        StartVertex vertice = new StartVertex();

                        if (arqObj.Groups[iG].Faces[iF][iI].VertexIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1 >= arqObj.Vertices.Count)
                        {
                            throw new ArgumentException("Vertex Position Index is invalid! Value: " + arqObj.Groups[iG].Faces[iF][iI].VertexIndex);
                        }

                        Vector3 position = new Vector3(
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].X,
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].Y,
                            arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].Z
                            );

                        vertice.Position = position;


                        if (arqObj.Groups[iG].Faces[iF][iI].TextureIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1 >= arqObj.Textures.Count)
                        {
                            vertice.Texture = new Vector2(0, 0);
                        }
                        else
                        {
                            Vector2 texture = new Vector2(
                            arqObj.Textures[arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1].U,
                            ((arqObj.Textures[arqObj.Groups[iG].Faces[iF][iI].TextureIndex - 1].V - 1) * -1)
                            );

                            vertice.Texture = texture;
                        }


                        if (arqObj.Groups[iG].Faces[iF][iI].NormalIndex <= 0 || arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1 >= arqObj.Normals.Count)
                        {
                            vertice.Normal = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            float nx = arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].X;
                            float ny = arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Y;
                            float nz = arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Z;
                            float NORMAL_FIX = (float)Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));
                            NORMAL_FIX = (NORMAL_FIX == 0) ? 1 : NORMAL_FIX;
                            nx /= NORMAL_FIX;
                            ny /= NORMAL_FIX;
                            nz /= NORMAL_FIX;

                            vertice.Normal = new Vector3(nx, ny, nz);
                        }

                        Vector4 color = new Vector4(1, 1, 1, 1);
                        if (UseColors)
                        {
                           color = new Vector4(
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].R,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].G,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].B,
                           arqObj.Vertices[arqObj.Groups[iG].Faces[iF][iI].VertexIndex - 1].A
                           );
                        }

                        vertice.Color = color;
                        vertice.WeightMap = weightMap;

                        face.Add(vertice);

                    }

                    if (face.Count != 0)
                    {
                        facesList.Add(face);
                    }

                }

                if (startStructure.FacesByMaterial.ContainsKey(materialNameInvariant))
                {
                    startStructure.FacesByMaterial[materialNameInvariant].Faces.AddRange(facesList);
                }
                else
                {
                    startStructure.FacesByMaterial.Add(materialNameInvariant, new StartFacesGroup(facesList));
                }

            }


            // faz a compressão das vertives
            if (CompressVertices == true)
            {
                startStructure.CompressAllFaces();
            }

            // estrutura intermediaria
            intermediaryStructure = MakeIntermediaryStructure(startStructure, UseExtendedNormals);

        }

        public static FinalBoneLine[] GetBoneLines((int ID, int parent, float x, float y, float z)[] Bones, Endianness endianness)
        {
            List<FinalBoneLine> bones = new List<FinalBoneLine>();

            for (int i = 0; i < Bones.Length; i++)
            {

                (float X, float Y, float Z) bonePos = (0, 0, 0);

                bonePos.X = Bones[i].x * CONSTs.GLOBAL_POSITION_SCALE;
                bonePos.Y = Bones[i].z * CONSTs.GLOBAL_POSITION_SCALE;
                bonePos.Z = Bones[i].y * -1 * CONSTs.GLOBAL_POSITION_SCALE;

                if (bonePos.Z == 0f * -1f) { bonePos.Z = 0; }

                byte ParentID = (byte)Bones[i].parent;
                if (Bones[i].parent < 0)
                {
                    ParentID = 0xFF;
                }

                bones.Add(new FinalBoneLine((byte)(ushort)Bones[i].ID, ParentID, bonePos.X, bonePos.Y, bonePos.Z, endianness));
            }

            return bones.ToArray();
        }

    }
}
