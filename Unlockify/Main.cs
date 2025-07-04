using BepInEx;
using RoR2;
using RoR2.UI;
using RoR2.UI.LogBook;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

namespace Unlockify
{
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class Main : BaseUnityPlugin
  {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Nuxlar";
    public const string PluginName = "Unlockify";
    public const string PluginVersion = "1.0.1";

    internal static Main Instance { get; private set; }
    public static string PluginDirectory { get; private set; }

    public void Awake()
    {
      Instance = this;

      Stopwatch stopwatch = Stopwatch.StartNew();

      Log.Init(Logger);

      On.RoR2.Run.IsUnlockableUnlocked_UnlockableDef += TrueifyRun;
      On.RoR2.Run.IsUnlockableUnlocked_string += TrueifyRun2;
      On.RoR2.Run.DoesEveryoneHaveThisUnlockableUnlocked_UnlockableDef += TrueifyRunAll;
      On.RoR2.Run.DoesEveryoneHaveThisUnlockableUnlocked_string += TrueifyRunAll2;

      On.RoR2.UnlockableCatalog.GetUnlockableDef_string += PreventAchievement1;
      On.RoR2.UnlockableCatalog.GetUnlockableDef_UnlockableIndex += PreventAchievement2;

      On.RoR2.UserProfile.HasUnlockable_UnlockableDef += TrueifyProfile;
      On.RoR2.UserProfile.HasUnlockable_string += TrueifyProfile2;
      On.RoR2.UserProfile.HasDiscoveredPickup += TrueifyPickup;
      On.RoR2.UserProfile.HasSurvivorUnlocked += TrueifySurvivor;
      On.RoR2.UserProfile.HasAchievement += TrueifyAchievement;
      On.RoR2.UserProfile.CanSeeAchievement += TrueifyAchievement2;

      On.RoR2.Stats.StatSheet.HasUnlockable += TrueifyStatSheet;

      On.RoR2.PreGameController.AnyUserHasUnlockable += TrueifyPreGameController;

      On.RoR2.AchievementSystemSteam.AddAchievement += DontGrantAchievement;

      stopwatch.Stop();
      Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
    }

    private void DontGrantAchievement(On.RoR2.AchievementSystemSteam.orig_AddAchievement orig, AchievementSystemSteam self, string achievementName)
    {

    }

    private bool TrueifyStatSheet(On.RoR2.Stats.StatSheet.orig_HasUnlockable orig, RoR2.Stats.StatSheet self, UnlockableDef def)
    {
      return true;
    }

    private UnlockableDef PreventAchievement1(On.RoR2.UnlockableCatalog.orig_GetUnlockableDef_string orig, string defName)
    {
      return null;
    }

    private UnlockableDef PreventAchievement2(On.RoR2.UnlockableCatalog.orig_GetUnlockableDef_UnlockableIndex orig, UnlockableIndex idx)
    {
      return null;
    }

    private bool TrueifyRun(On.RoR2.Run.orig_IsUnlockableUnlocked_UnlockableDef orig, Run self, UnlockableDef unlockableDef)
    {
      if (NetworkServer.active)
        return true;
      Log.Warning("[Server] function 'Unlockify.TrueifyRun' called on client");
      return false;
    }

    private bool TrueifyRun2(On.RoR2.Run.orig_IsUnlockableUnlocked_string orig, Run self, string unlockableName)
    {
      if (NetworkServer.active)
        return true;
      Log.Warning("[Server] function 'Unlockify.TrueifyRun2' called on client");
      return false;
    }

    private bool TrueifyPreGameController(On.RoR2.PreGameController.orig_AnyUserHasUnlockable orig, UnlockableDef unlockableDef)
    {
      if (NetworkServer.active)
        return true;
      Log.Warning("[Server] function 'Unlockify.TrueifyPreGameController' called on client");
      return false;
    }

    private bool TrueifyRunAll(On.RoR2.Run.orig_DoesEveryoneHaveThisUnlockableUnlocked_UnlockableDef orig, Run self, UnlockableDef unlockableDef)
    {
      if (NetworkServer.active)
        return true;
      Log.Warning("[Server] function 'Unlockify.Trueify' called on client");
      return false;
    }

    private bool TrueifyRunAll2(On.RoR2.Run.orig_DoesEveryoneHaveThisUnlockableUnlocked_string orig, Run self, string unlockableName)
    {
      if (NetworkServer.active)
        return true;
      Log.Warning("[Server] function 'Unlockify.Trueify' called on client");
      return false;
    }

    private bool TrueifyProfile(On.RoR2.UserProfile.orig_HasUnlockable_UnlockableDef orig, UserProfile self, UnlockableDef unlockableDef)
    {
      return true;
    }

    private bool TrueifyProfile2(On.RoR2.UserProfile.orig_HasUnlockable_string orig, UserProfile self, string unlockableName)
    {
      return true;
    }

    private bool TrueifyAchievement(On.RoR2.UserProfile.orig_HasAchievement orig, UserProfile self, string achievementName)
    {
      return true;
    }

    private bool TrueifyAchievement2(On.RoR2.UserProfile.orig_CanSeeAchievement orig, UserProfile self, string achievementName)
    {
      return true;
    }

    private bool TrueifySurvivor(On.RoR2.UserProfile.orig_HasSurvivorUnlocked orig, UserProfile self, SurvivorIndex survivorIndex)
    {
      return true;
    }

    private bool TrueifyPickup(On.RoR2.UserProfile.orig_HasDiscoveredPickup orig, UserProfile self, PickupIndex pickupIndex)
    {
      return true;
    }
  }
}