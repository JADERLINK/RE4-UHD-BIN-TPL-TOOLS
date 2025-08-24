using SimpleEndianBinaryIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SHARED_UHD_BIN_TPL.EXTRACT
{
    public static class MorphBinDecoder
    {
        public static MorphBIN Decoder(Stream stream, long startOffset, UhdBinHeader header, Endianness endianness) 
        {
            if (header.morph_offset != 0)
            {
                MorphBIN morphBIN = new MorphBIN();

                EndianBinaryReader br = new EndianBinaryReader(stream, endianness);
                br.BaseStream.Position = startOffset + header.morph_offset;

                uint morphcount = br.ReadUInt32();
                morphBIN.MorphGroups = new MorphGroup[morphcount];

                (uint offset, uint count)[] parts = new (uint offset, uint count)[morphcount];

                for (int i = 0; i < morphcount; i++)
                {
                    parts[i].offset = br.ReadUInt32();
                    parts[i].count = br.ReadUInt32();
                }

                for (int i = 0; i < morphcount; i++)
                {
                    morphBIN.MorphGroups[i] = new MorphGroup();
                    morphBIN.MorphGroups[i].Morph_Vertex = new (ushort VertexID, short posX, short posY, short posZ)[parts[i].count];
                    br.BaseStream.Position = startOffset + header.morph_offset + parts[i].offset + 4;

                    for (int j = 0; j < parts[i].count; j++)
                    {
                        morphBIN.MorphGroups[i].Morph_Vertex[j].VertexID = br.ReadUInt16();
                        morphBIN.MorphGroups[i].Morph_Vertex[j].posX = br.ReadInt16();
                        morphBIN.MorphGroups[i].Morph_Vertex[j].posY = br.ReadInt16();
                        morphBIN.MorphGroups[i].Morph_Vertex[j].posZ = br.ReadInt16();
                    }
                }

                return morphBIN;
            }

            return null;
        }
    }
}
