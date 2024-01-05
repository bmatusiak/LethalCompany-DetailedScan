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
using TerminalApi.Classes;

namespace DetailedScan
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("atomic.terminalapi")]
    public class LSDetailedScan : BaseUnityPlugin
    {
        private const string modGUID = "fivetoofive.DetailedScan";
        private const string modName = "DetailedScan";
        private const string modVersion = "1.2.2";

        private readonly Harmony harmony = new Harmony(modGUID);

        public bool isSet;

        void Awake()
        {
            BepInEx.Logging.Logger.CreateLogSource(modGUID).LogInfo("DetailedScan is loaded!");

            harmony.PatchAll(typeof(LSDetailedScan));

            TerminalBeginUsing += OnBeginUsing;
            TerminalAwake += TerminalIsAwake;
        }

        CommandInfo info = new CommandInfo()
        {
            DisplayTextSupplier = () =>
            {
                return "Ship is not Landed!\n\n";
            },
            Category = "Other",
            Description = "Shows a detailed list of all the scrap still remaining outside the ship."

        };

        CommandInfo info2 = new CommandInfo()
        {
            DisplayTextSupplier = () =>
            {
                return "Ship is not Landed!\n\n";
            },
            Category = "Other",
            Description = "A shortform version of the command 'detailed'."

        };



        private void TerminalIsAwake(object sender, TerminalEventArgs e)
        {
            if (isSet == false)
            {
                AddCommand("detailed", info, "ftf1", true);
                AddCommand("ds", info2, null, true);
                isSet = true;
            }
        }

        private void OnBeginUsing(object sender, TerminalEventArgs e)
        {
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

            info.DisplayTextSupplier = () =>
            {
                return finStr;
            };

            info2.DisplayTextSupplier = () =>
            {
                return finStr;
            };


        }
    }
}
