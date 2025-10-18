using System;
using System.Collections.Generic;
using System.Text;

namespace SHARED_TOOLS.REPACK.Structures
{
    /// <summary>
    /// Representa o conjunto de pesos associado a um vértice;
    /// </summary>
    public struct PrepareWeightMap : IEquatable<PrepareWeightMap>
    {
        public int Links;

        public int BoneID1;
        public float Weight1;

        public int BoneID2;
        public float Weight2;

        public int BoneID3;
        public float Weight3;

        public PrepareWeightMap(int links, int boneID1, float weight1, int boneID2, float weight2, int boneID3, float weight3)
        {
            Links = links;
            BoneID1 = boneID1;
            Weight1 = weight1;
            BoneID2 = boneID2;
            Weight2 = weight2;
            BoneID3 = boneID3;
            Weight3 = weight3;
        }

        public static bool operator ==(PrepareWeightMap lhs, PrepareWeightMap rhs) => lhs.Equals(rhs);

        public static bool operator !=(PrepareWeightMap lhs, PrepareWeightMap rhs) => !(lhs == rhs);

        public bool Equals(PrepareWeightMap obj)
        {
            return obj.Links == Links
                && obj.BoneID1 == BoneID1
                && obj.BoneID2 == BoneID2
                && obj.BoneID3 == BoneID3
                && obj.Weight1 == Weight1
                && obj.Weight2 == Weight2
                && obj.Weight3 == Weight3;
        }

        public override bool Equals(object obj)
        {
            return obj is PrepareWeightMap map
                && map.Links == Links
                && map.BoneID1 == BoneID1
                && map.BoneID2 == BoneID2
                && map.BoneID3 == BoneID3
                && map.Weight1 == Weight1
                && map.Weight2 == Weight2
                && map.Weight3 == Weight3;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Links.GetHashCode();
                hash = hash * 23 + BoneID1.GetHashCode();
                hash = hash * 23 + Weight1.GetHashCode();
                hash = hash * 23 + BoneID2.GetHashCode();
                hash = hash * 23 + Weight2.GetHashCode();
                hash = hash * 23 + BoneID3.GetHashCode();
                hash = hash * 23 + Weight3.GetHashCode();
                return hash;
            }
        }

        public FinalWeightMap GetFinalWeightMap() 
        {
            byte links = (byte)Links;

            byte boneID1 = (byte)(ushort)BoneID1;
            byte boneID2 = (byte)(ushort)BoneID2;
            byte boneID3 = (byte)(ushort)BoneID3;

            byte weight1 = (byte)(Weight1 * 100);
            byte weight2 = (byte)(Weight2 * 100);
            byte weight3 = (byte)(Weight3 * 100);

            return new FinalWeightMap(links, boneID1, weight1, boneID2, weight2, boneID3, weight3);
        }
    }
}
