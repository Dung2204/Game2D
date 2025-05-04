using UnityEngine;
using UnityEditor;

public class TextureAssetImporter : AssetPostprocessor
{
    private static string environmentsDir = "Assets/Game/Environments";
    private static string AllskyDir = "Assets/Allsky";
    private static string GameDir = "Assets/Game";
    private static string UiDir = "Assets/Game/UIs";
    private static string ViewDir = "Assets/Game/UIs";
    private static string IconsDir = "Assets/Game/UIs/Icons";
    private static string NPCIconsDir = IconsDir + "/Npc";
    private static string NPCIconsDir2 = IconsDir + "/NpcIcon";
    private static string SkillIconsDir = IconsDir + "/Skill";
    private static string FontsDir = "Assets/Game/UIs/Fonts";
    private static string CommonImagesDir = "Assets/Game/UIs/CommonImages";
    private static string UIImagesDir = "Assets/Game/UIs/Images";
    private static string ItemDir = "Assets/Game/UIs/Icons/Item";
    private static string ViewImagesDir = "Assets/Game/UIs/ViewImages";
    private static string BigFaceDir = ViewImagesDir + "/ChatPanel/BigFace";
    private static string ActorsDir = "Assets/Game/Actors";
    private static string RoleDir = "Assets/Game/Actors/Role";
    private static string OptimizeImagesDir = "Assets/Game/UIs/OptimizeImages";
    private static string OptimizeArtFontImagesDir = OptimizeImagesDir + "/artfonts";
    private static string EffectTextutrDir = "Assets/Game/Effects/Textures";
	private static string BigImageDir = "Assets/Game/UIs/BigImages";
    private static string FontImageDir = "Assets/Game/UIs/Fonts";
    private static string OldFontsImageDir = "Assets/Game/UIs/OldFonts";
	private static string TitleDir = IconsDir + "/Title";
    //private static string T4MOBJ = "Assets/T4MOBJ/Terrains/Texture";
    private static string T4M = "Assets/T4M";
    private static string T4MOBJ = "Assets/T4MOBJ";
    private static string Grounds_Texture = "Assets/Game/Grounds_Texture";

    private void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        if (HideFlags.NotEditable == textureImporter.hideFlags)
        {
            return;
        }

        if (ImporterUtils.CheckLabel(textureImporter))
        {
            return;
        }

        if (textureImporter.assetPath.StartsWith(FontImageDir) || textureImporter.assetPath.StartsWith(OldFontsImageDir))
        {
            return;
        }

        if (textureImporter.assetPath.StartsWith(T4MOBJ) || textureImporter.assetPath.StartsWith(T4M))
        {
            return;
        }

        ProcessTextureType(textureImporter);
        ProcessPackingTag(textureImporter);
        ProcessMipmap(textureImporter);
        //ProcessReadable(textureImporter);
        ProcessFilterMode(textureImporter);
        ProcessCompression(textureImporter);
		ProcessPlatformTextureSetting(textureImporter);
	}

    void OnPostprocessTexture(Texture2D texture)
    {
        //ImporterUtils.SetLabel(assetPath);
    }

    private void ProcessTextureType(TextureImporter textureImporter)
    {
        if (textureImporter.assetPath.StartsWith(ViewDir)
            || textureImporter.assetPath.StartsWith(IconsDir))
        {
            textureImporter.textureType = TextureImporterType.Sprite;
        }
    }

    private void ProcessPackingTag(TextureImporter textureImporter)
    {
        if (TextureImporterType.Sprite != textureImporter.textureType
            || textureImporter.assetPath.Contains("/nopack/")
            //|| textureImporter.assetPath.Contains(ItemDir)
            || textureImporter.assetPath.StartsWith(FontsDir)
            || textureImporter.assetPath.StartsWith(EffectTextutrDir)
            || textureImporter.assetPath.StartsWith(NPCIconsDir)
            || textureImporter.assetPath.StartsWith(NPCIconsDir2)
            || textureImporter.assetPath.StartsWith(SkillIconsDir))
          //  || textureImporter.assetPath.StartsWith(OptimizeArtFontImagesDir))
        {
            textureImporter.spritePackingTag = string.Empty;
            return;
        }

        if (textureImporter.assetPath.StartsWith(CommonImagesDir))
        {
            textureImporter.spritePackingTag = "uis/common_images";
            return;
        }

        if (textureImporter.assetPath.StartsWith(BigFaceDir))
        {
            string pack_name = textureImporter.assetPath.Replace(BigFaceDir + "/", "").ToLower();
            pack_name = pack_name.Substring(0, pack_name.IndexOf("/"));
            textureImporter.spritePackingTag = "uis/view/bigface/" + pack_name;
            return;
        }

        if (textureImporter.assetPath.StartsWith(ViewImagesDir))
        {
            string pack_name = textureImporter.assetPath.Replace(ViewImagesDir + "/", "").ToLower();
            pack_name = pack_name.Substring(0, pack_name.IndexOf("/"));
            textureImporter.spritePackingTag = "uis/view/" + pack_name;
            return;
        }

        if (textureImporter.assetPath.StartsWith(OptimizeImagesDir))
        {
            string pack_name = textureImporter.assetPath.Replace(OptimizeImagesDir + "/", "").ToLower();
            pack_name = pack_name.Substring(0, pack_name.IndexOf("/"));
            textureImporter.spritePackingTag = "uis/optimizeimage/" + pack_name;
            return;
        }

        if (textureImporter.assetPath.StartsWith(UIImagesDir))
        {
            textureImporter.spritePackingTag = "uis/images";
            return;
        }

		if (textureImporter.assetPath.Contains(ItemDir))
		{
			var id = textureImporter.assetPath.Replace(ItemDir + "/Item_", "");
			id = id.Replace(".png", "");
			int result = 0;
			if (int.TryParse(id,out result))
			{
				textureImporter.spritePackingTag = "uis/icons/item/item_" + result / 50;
			}
			else
			{
				textureImporter.spritePackingTag = "uis/icons/item/item_other";
			}
			return;
		}
		if (textureImporter.assetPath.Contains(TitleDir))
		{
			string num = System.Text.RegularExpressions.Regex.Replace(textureImporter.assetPath, @"[^0-9]+", "");
			int result = 0;
			if (int.TryParse(num, out result))
			{
				textureImporter.spritePackingTag = "uis/icons/title_" + result / 1000;
			}
			else
			{
				textureImporter.spritePackingTag = "uis/icons/title_other";
			}
			return;
		}


		string pack_tag = textureImporter.assetPath.Replace(GameDir + "/", "").ToLower();
        pack_tag = pack_tag.Substring(0, pack_tag.LastIndexOf("/"));
        textureImporter.spritePackingTag = pack_tag;
    }

    private void ProcessMipmap(TextureImporter textureImporter)
    {
        if (textureImporter.assetPath.StartsWith(environmentsDir)
            || textureImporter.textureShape != TextureImporterShape.Texture2D
            || textureImporter.textureType == TextureImporterType.Lightmap
            || textureImporter.assetPath.StartsWith(AllskyDir)) 
        {
            textureImporter.mipmapEnabled = true;
            return;
        }

        textureImporter.mipmapEnabled = false;
    }

    private void ProcessReadable(TextureImporter textureImporter)
    {
        if (!textureImporter.assetPath.StartsWith(FontsDir) && !textureImporter.assetPath.StartsWith(RoleDir) && !textureImporter.assetPath.StartsWith(T4MOBJ) && !textureImporter.assetPath.StartsWith(Grounds_Texture))
        {
            textureImporter.isReadable = false;
        }
    }

    private void ProcessFilterMode(TextureImporter textureImporter)
    {
        if (TextureImporterType.Sprite == textureImporter.textureType)
        {
            textureImporter.filterMode = FilterMode.Bilinear;
        }
    }

    private void ProcessCompression(TextureImporter textureImporter)
    {
        if (textureImporter.assetPath.StartsWith(FontsDir))
        {
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            return;
        }

        if (textureImporter.textureCompression < TextureImporterCompression.Compressed)
        {
            textureImporter.textureCompression = TextureImporterCompression.Compressed;
        }
    }

    private void ProcessPlatformTextureSetting(TextureImporter textureImporter)
    {
        if (textureImporter.assetPath.StartsWith(ViewDir) && !textureImporter.assetPath.StartsWith(BigImageDir)
            && !textureImporter.assetPath.StartsWith(FontImageDir) && !textureImporter.assetPath.StartsWith(OldFontsImageDir))
        {
            TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings("iPhone");
            settings.overridden = true;
            settings.format = TextureImporterFormat.ASTC_RGBA_4x4;
            textureImporter.SetPlatformTextureSettings(settings);
        }

        //if (textureImporter.assetPath.StartsWith(ActorsDir))
        //{
        //    TextureImporterPlatformSettings settings = textureImporter.GetDefaultPlatformTextureSettings();
        //    settings.overridden = true;
        //    settings.maxTextureSize = 512;
        //    textureImporter.SetPlatformTextureSettings(settings);
        //}
    }

}