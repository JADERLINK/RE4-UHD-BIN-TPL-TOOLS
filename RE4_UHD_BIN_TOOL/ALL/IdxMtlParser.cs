using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RE4_UHD_BIN_TOOL
{
    public static class IdxMtlParser
    {
        public static IdxMtl Parser(IdxMaterial idxMaterial, UhdTPL uhdTPL)
        {
            IdxMtl idx = new IdxMtl();
            idx.MtlDic = new Dictionary<string, MtlObj>();

            foreach (var mat in idxMaterial.MaterialDic)
            {
                MtlObj mtl = new MtlObj();

                var diffuse = uhdTPL.TplArray[mat.Value.diffuse_map];

                mtl.map_Kd = new TexPathRef(diffuse.PackID, diffuse.TextureID, diffuse.PixelFormatType);
               
                mtl.Ks = new KsClass(mat.Value.intensity_specular_r, mat.Value.intensity_specular_g, mat.Value.intensity_specular_b);

                mtl.specular_scale = mat.Value.specular_scale;

                if (mat.Value.bump_map != 255)
                {
                    var bump = uhdTPL.TplArray[mat.Value.bump_map];
                    mtl.map_Bump = new TexPathRef(bump.PackID, bump.TextureID, bump.PixelFormatType);         
                }

                if (mat.Value.opacity_map != 255)
                {
                    var alpha = uhdTPL.TplArray[mat.Value.opacity_map];
                    mtl.map_d = new TexPathRef(alpha.PackID, alpha.TextureID, alpha.PixelFormatType);   
                }

                if (mat.Value.generic_specular_map != 255)
                {
                    mtl.ref_specular_map = new TexPathRef(0x07000000, mat.Value.generic_specular_map, "dds");
                }

                if (mat.Value.custom_specular_map != 255)
                {
                    var custom = uhdTPL.TplArray[mat.Value.custom_specular_map];
                    mtl.ref_specular_map = new TexPathRef(custom.PackID, custom.TextureID, custom.PixelFormatType);
                }

                idx.MtlDic.Add(mat.Key, mtl);
            }

            return idx;
        }

    }
}
