using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHARED_UHD_BIN_TPL.EXTRACT
{
    public class MorphBIN
    {
        public MorphGroup[] MorphGroups;
    }

    public class MorphGroup
    {
        public (ushort VertexID, short posX, short posY, short posZ)[] Morph_Vertex;
    }
}
