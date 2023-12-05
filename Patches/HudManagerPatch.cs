using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailedScan.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HudManagerPatch
    {
        
        [HarmonyPatch("SetClock")]
        [HarmonyPostfix]
        static void listScrapPatch()
        {
            System.Collections.Generic.List<GrabbableObject> sortedItems = new System.Collections.Generic.List<GrabbableObject>();
            GrabbableObject[] unSortedItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            for (int n = 0; n < unSortedItems.Length; n++)
            {
                string newItemStr = string.Concat(unSortedItems[n].itemProperties.itemName + " : " + unSortedItems[n].scrapValue.ToString() + "\n");
                if (unSortedItems[n].itemProperties.isScrap && !unSortedItems[n].scrapPersistedThroughRounds && !unSortedItems[n].isInShipRoom)
                {
                    sortedItems.Add(unSortedItems[n]);
                }
            }

            string itemStr = string.Join("\n", sortedItems.Select(x => x.itemProperties.itemName + " : " + x.scrapValue.ToString() + " Value"));
            string finStr = "Scrap not in ship: \n\n" + itemStr;

            TerminalApi.TerminalApi.UpdateKeywordCompatibleNoun("check", "detailed", TerminalApi.TerminalApi.CreateTerminalNode($"{finStr}", true));

        }
    }
}
