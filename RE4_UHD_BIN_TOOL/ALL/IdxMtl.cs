using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RE4_UHD_BIN_TOOL
{
    public class IdxMtl
    {
        // material name, MtlObj
        public Dictionary<string, MtlObj> MtlDic;

    }


    public class MtlObj 
    {
        /*
        // ambient color
        #white
        Ka 1.000 1.000 1.000
        
        // diffuse color
        #white
        Kd 1.000 1.000 1.000
        
        // specular color
        #black (off)
        Ks 0.000 0.000 0.000

        //specular exponent
        # ranges between 0 and 1000
        Ns 10.000

        //Materials can be transparent.
        //A value of 1.0 for "d" (dissolve) is the default and means fully opaque, as does a value of 0.0 for "Tr". Dissolve works on all illumination models.
        # some implementations use 'd'
        d 0.9
        # others use 'Tr' (inverted: Tr = 1 - d)
        Tr 0.1

        //Texture maps

        # the ambient texture map
        map_Ka lemur.tga
   
        //Blender base Color
        # the diffuse texture map (most of the time, it will be the same as the
        # ambient texture map)
        map_Kd lemur.tga
   
        //Blender Specular
        # specular color texture map
        map_Ks lemur.tga
   
        //blender Roughness
        # specular highlight component
        map_Ns lemur_spec.tga
   
        //blender Alpha
        # the alpha texture map
        map_d lemur_alpha.tga
   
        # some implementations use 'map_bump' instead of 'bump' below
        map_bump lemur_bump.tga
   
        # bump map (which by default uses luminance channel of the image)
        bump lemur_bump.tga
   
        // não funciona no blender
        # displacement map
        disp lemur_disp.tga
   
        // não funciona no blender
        # stencil decal texture (defaults to 'matte' channel of the image)
        decal lemur_stencil.tga

        //Blender metalic
        refl -s 16 16 1 07000000/0001.dds

        */

        public TexPathRef map_Kd = null; //diffuse_texture

        public TexPathRef map_Bump = null; //bump_texture (bump)

        public TexPathRef map_d = null; //opacity_map alpha_texture

        // map_Ks  // map_Ns
        public TexPathRef ref_specular_map = null; //generic_specular_map

        public byte specular_scale = 0; // specular_scale

        public KsClass Ks = null; // intensity_specular_r, intensity_specular_g, intensity_specular_b

    }


    // é usado para definir o caminho das texturas no mtl
    public class TexPathRef 
    {
        public uint PackID { get; private set; }
        public uint TextureID { get; private set; }
        public string Format { get; private set; }

        public TexPathRef(uint PackID, uint TextureID, uint FormatType) 
        {
            this.PackID = PackID;
            this.TextureID = TextureID;
            this.Format = FormatType == 0xE ? "dds" : "tga";
        }

        public TexPathRef(uint PackID, uint TextureID, string ImageFormat)
        {
            this.PackID = PackID;
            this.TextureID = TextureID;
            this.Format = ImageFormat;
        }

        public TexPathRef(string texturePath) 
        {
            texturePath = texturePath.Replace("\\", "/");
            var split = texturePath.Split('/').Where(s => s.Length != 0).ToArray();

            try
            {
                var last = split.Last().Split('.').Where(s => s.Length != 0).ToArray();
                TextureID = uint.Parse(Utils.ReturnValidDecValue(last[0]), NumberStyles.Integer, CultureInfo.InvariantCulture);
                Format = last.Last();
            }
            catch (Exception)
            {
            }

            if (split.Length -1 > 0)
            {
                try
                {
                    var resplit = split[split.Length - 2].Split(' ').Where(s => s.Length != 0).ToArray();
                    PackID = uint.Parse(Utils.ReturnValidHexValue(resplit.Last()), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
            }
        }

        public override string ToString()
        {
            return GetPath();
        }
        
        public string GetPath() 
        {
            return PackID.ToString("x8") + "/" + TextureID.ToString("D4") + "." + Format;
        }

    }

    public class KsClass 
    {
        private byte r, g, b;

        public KsClass(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public KsClass(float r, float g, float b)
        {
            this.r = (byte)(r * 255f);
            this.g = (byte)(g * 255f);
            this.b = (byte)(b * 255f);
        }

        public override string ToString()
        {
            return GetKs();
        }

        public string GetKs()
        {
           return (r / 255f).ToString("f6", CultureInfo.InvariantCulture)
          + " " + (g / 255f).ToString("f6", CultureInfo.InvariantCulture)
          + " " + (b / 255f).ToString("f6", CultureInfo.InvariantCulture);
        }

        public byte GetR() 
        {
            return r;
        }

        public byte GetG()
        {
            return g;
        }

        public byte GetB()
        {
            return b;
        }


    }


}
