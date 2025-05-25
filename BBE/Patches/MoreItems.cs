using HarmonyLib;
using BBE.Creators;
using BBE.ModItems;
using UnityEngine;
using MTM101BaldAPI;
using BBE.CustomClasses;
using System.Linq;
using BBE.Extensions;

namespace BBE.Patches
{
    [HarmonyPatch(typeof(PartyEvent))]
    internal class MoreItemsInPartyEvent
    {
        [HarmonyPatch(nameof(PartyEvent.Begin))]
        [HarmonyPrefix]
        private static void AddNewItems(PartyEvent __instance)
        {
            if (BaldiExtraPlugin.CurrentFloorData != null)
                __instance.potentialItems = __instance.potentialItems.AddRangeToArray(BaldiExtraPlugin.CurrentFloorData.partyEventItems.ToArray());
        }
    }
}