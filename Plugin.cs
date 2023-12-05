using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi;
using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;

namespace DetailedScan
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("atomic.terminalapi")]
    public class LSDetailedScan : BaseUnityPlugin
    {
        private const string modGUID = "fivetoofive.DetailedScan";
        private const string modName = "DetailedScan";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        void Awake()
        {
            BepInEx.Logging.Logger.CreateLogSource(modGUID).LogInfo("DetailedScan is loaded!");

            harmony.PatchAll(typeof(LSDetailedScan));

            TerminalBeginUsing += OnBeginUsing;
            TerminalAwake += TerminalIsAwake;
        }

        private void TerminalIsAwake(object sender, TerminalEventArgs e)
        {            
            BepInEx.Logging.Logger.CreateLogSource(modGUID).LogInfo("Terminal is started");
            AddCommand("detailed", "Ship is not Landed!\n\n", "kcehc", true);
        }

        private void OnBeginUsing(object sender, TerminalEventArgs e)
        {
            BepInEx.Logging.Logger.CreateLogSource(modGUID).LogInfo("Terminel Used!");
            System.Collections.Generic.List<GrabbableObject> sortedItems = new System.Collections.Generic.List<GrabbableObject>();
            GrabbableObject[] unSortedItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            int totalValue = 0;
            for (int n = 0; n < unSortedItems.Length; n++)
            {
                if (unSortedItems[n].itemProperties.isScrap && !unSortedItems[n].scrapPersistedThroughRounds && !unSortedItems[n].isInShipRoom)
                {
                    sortedItems.Add(unSortedItems[n]);
                    totalValue += unSortedItems[n].scrapValue;
                }
            }

            string itemStr = string.Join("\n", sortedItems.Select(x => x.itemProperties.itemName + " : " + x.scrapValue.ToString() + " Value"));
            string finStr = "Scrap not in ship: " + sortedItems.Count().ToString() + "\n\n" + itemStr + "\n\nWith a total value of: " + totalValue.ToString()+"\n\n";

            UpdateKeywordCompatibleNoun("kcehc", "detailed", CreateTerminalNode($"{finStr}", true));
        }




    }
}
