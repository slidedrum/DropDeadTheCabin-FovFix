using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using KINEMATION.FPSAnimationFramework.Runtime.Camera;

namespace FovFix
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
    }
    [HarmonyLib.HarmonyPatch(typeof(FPSCameraController), nameof(FPSCameraController.UpdateTargetFOV))]
    public class FovPatch
    {
        public static float fovOffset = 1.5f;
        public static bool Prefix(ref float newFov)
        {
            Plugin.Logger.LogInfo("Offsetting FOV");
            newFov = newFov * fovOffset;
            return true;
        }
    }
    [HarmonyLib.HarmonyPatch(typeof(FPSCameraController), nameof(FPSCameraController.Initialize))]
    public class initPatch
    {
        public static void Postfix(FPSCameraController __instance)
        {
            __instance.UpdateTargetFOV(70);
        }
    }
}
