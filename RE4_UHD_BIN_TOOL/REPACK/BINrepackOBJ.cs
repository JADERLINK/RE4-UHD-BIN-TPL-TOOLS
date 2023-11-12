using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ObjLoader.Loader.Loaders;
using RE4_UHD_BIN_TOOL.REPACK.Structures;

namespace RE4_UHD_BIN_TOOL.REPACK
{
    public static partial class BinRepack
    {
        public static void RepackOBJ(Stream objFile, bool CompressVertices, int ObjFileUseBone, out IntermediaryStructure intermediaryStructure, bool UseExtendedNormals)
        {

            // load .obj file
            var objLoaderFactory = new ObjLoader.Loader.Loaders.ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            var streamReader = new StreamReader(objFile, Encoding.ASCII);
            ObjLoader.Loader.Loaders.LoadResult arqObj = objLoader.Load(streamReader);
            streamReader.Close();


            //--- crio a primeira estrutura:

            StartStructure startStructure = new StartStructure();

            Vector4 color = new Vector4(1, 1, 1, 1);
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
                            Vector3 normal = new Vector3(
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].X,
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Y,
                            arqObj.Normals[arqObj.Groups[iG].Faces[iF][iI].NormalIndex - 1].Z
                            );

                            vertice.Normal = normal;
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




    }
}
