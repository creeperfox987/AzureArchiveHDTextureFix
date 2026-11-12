// Copyright (c) 2026 creeperfox987.
// Co-authored-by: DeepSeek AI.
// Licensed under the MIT License.

using HarmonyLib;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace AzureArchiveHDTextureFix
{
    [HarmonyPatch(typeof(RuntimeAtlasTexture), nameof(RuntimeAtlasTexture.CanLoadPngAsDxt5))]
    public class Patch_DisableDxtCompression
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(RuntimeAtlasTexture), nameof(RuntimeAtlasTexture.TryReadCache))]
    public class Patch_DestroyReadCache
    {
        public static bool Prefix(ref Il2CppStructArray<byte> __result)
        {
            __result = null; 
            Plugin.Logger.LogWarning("Bypassed old texture cache. Forcing regeneration from source image.");
            return false;
        }
    }

    [HarmonyPatch(typeof(RuntimeAtlasTexture), nameof(RuntimeAtlasTexture.WriteCache))]
    public class Patch_DestroyWriteCache
    {
        public static bool Prefix()
        {
            return false; 
        }
    }
    
    [HarmonyPatch(typeof(RuntimeAtlasTexture), nameof(RuntimeAtlasTexture.Load))]
    public class Patch_ForceHighQualityTexture
    {
        public static void Postfix(ref Texture2D __result)
        {
            if (__result != null)
            {
                __result.filterMode = FilterMode.Trilinear;
                __result.anisoLevel = 16;
                Plugin.Logger.LogWarning($"HD Texture Applied: {__result.name} | Size: {__result.width}x{__result.height} | Format: {__result.format}");
            }
        }
    }
}