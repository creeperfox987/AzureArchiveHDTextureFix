// Copyright (c) 2026 creeperfox987.
// Co-authored-by: DeepSeek AI.
// Licensed under the MIT License.

using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace AzureArchiveHDTextureFix
{
    [BepInPlugin("com.yourname.azurearchive.hdfix", "Azure Archive HD Texture Fix", "1.0.0")]
    public class Plugin : BasePlugin
    {
        internal static ManualLogSource Logger;

        public override void Load()
        {
            Logger = BepInEx.Logging.Logger.CreateLogSource("AA-HD-Fix");
            
            Logger.LogInfo("==========================================");
            Logger.LogInfo(" Azure Archive HD Texture Fix Loaded");
            Logger.LogInfo(" Initializing Harmony patches...");
            Logger.LogInfo("==========================================");

            var harmony = new Harmony("com.yourname.azurearchive.hdfix");
            harmony.PatchAll();
        }
    }
}