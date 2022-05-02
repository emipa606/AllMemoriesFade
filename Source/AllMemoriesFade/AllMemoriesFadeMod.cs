using System;
using Mlie;
using UnityEngine;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
internal class AllMemoriesFadeMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static AllMemoriesFadeMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private AllMemoriesFadeSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public AllMemoriesFadeMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(ModLister.GetActiveModWithIdentifier("Mlie.AllMemoriesFade"));
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal AllMemoriesFadeSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<AllMemoriesFadeSettings>();
            }

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "All Memories Fade";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.Label("AMF.MoodOffset".Translate(Settings.MemoryMoodOffset.ToStringPercent()), -1,
            "AMF.MoodOffset.Tooltip".Translate());
        Settings.MemoryMoodOffset = listing_Standard.Slider(Settings.MemoryMoodOffset, 0f, 2f);
        listing_Standard.Label("AMF.Example".Translate(Math.Round(-3f * Settings.MemoryMoodOffset),
            Math.Round(10 * Settings.MemoryMoodOffset), Math.Round(-20f * Settings.MemoryMoodOffset),
            Math.Round(40f * Settings.MemoryMoodOffset), Math.Round(-80f * Settings.MemoryMoodOffset)));

        listing_Standard.Gap();
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("AMF.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        AllMemoriesFade.UpdateMoodEffects();
    }
}