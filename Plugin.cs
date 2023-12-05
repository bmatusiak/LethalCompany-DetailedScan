using BepInEx;
using DetailedScan.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailedScan
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LSDetailedScan : BaseUnityPlugin
    {
        private const string modGUID = "fivetoofive.DetailedScan";
        private const string modName = "DetailedScan";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        void Awake()
        {
            BepInEx.Logging.Logger.CreateLogSource(modGUID).LogInfo("DetailedScan is awake!");

            harmony.PatchAll(typeof(LSDetailedScan));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(HudManagerPatch));
        }


    }
}
