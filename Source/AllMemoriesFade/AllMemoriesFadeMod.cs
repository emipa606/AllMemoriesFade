using System;
using System.Linq;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
internal class AllMemoriesFadeMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static AllMemoriesFadeMod Instance;

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
        Instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal AllMemoriesFadeSettings Settings
    {
        get
        {
            settings ??= GetSettings<AllMemoriesFadeSettings>();

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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.Label("AMF.MoodOffset".Translate(Settings.MemoryMoodOffset.ToStringPercent()), -1,
            "AMF.MoodOffset.Tooltip".Translate());
        Settings.MemoryMoodOffset = listingStandard.Slider(Settings.MemoryMoodOffset, 0f, 2f);
        listingStandard.Label("AMF.Example".Translate(Math.Round(-3f * Settings.MemoryMoodOffset),
            Math.Round(10 * Settings.MemoryMoodOffset), Math.Round(-20f * Settings.MemoryMoodOffset),
            Math.Round(40f * Settings.MemoryMoodOffset), Math.Round(-80f * Settings.MemoryMoodOffset)));

        listingStandard.Gap();
        var originalLength = Settings.MemoryLengthOffset;
        listingStandard.Label("AMF.LengthOffset".Translate(Settings.MemoryLengthOffset.ToStringPercent()), -1,
            "AMF.LengthOffset.Tooltip".Translate());
        Settings.MemoryLengthOffset = listingStandard.Slider(Settings.MemoryLengthOffset, 0f, 2f);
        listingStandard.Label("AMF.Example".Translate(getTimeFromLength(1f), getTimeFromLength(8f),
            getTimeFromLength(180f), getTimeFromLength(30f), getTimeFromLength(120f)));
        if (Settings.MemoryLengthOffset != originalLength)
        {
            AllMemoriesFade.UpdateLengthValues();
        }

        listingStandard.Gap();

        listingStandard.Label("AMF.DurationExclusion".Translate(), -1,
            "AMF.DurationExclusion.Tooltip".Translate());
        Widgets.FloatRange(listingStandard.GetRect(30f), SettingsCategory().GetHashCode(), ref Settings.DurationRange,
            0, AllMemoriesFade.MaxDuration);
        var currentThougths = AllMemoriesFade.GetCurrentAffectedThoughts();
        var lowThoughts = string.Join("\n", currentThougths.Where(def =>
                def.durationDays == currentThougths.Min(thoughtDef => thoughtDef.durationDays))
            .Select(def => $"{def.defName}: {def.DurationTicks.ToStringTicksToDays()}"));
        var highThoughts = string.Join("\n", currentThougths.Where(def =>
                def.durationDays == currentThougths.Max(thoughtDef => thoughtDef.durationDays))
            .Select(def => $"{def.defName}: {def.DurationTicks.ToStringTicksToDays()}"));

        listingStandard.Label("AMF.IncludesThoughts".Translate(currentThougths.Count), -1f,
            $"{lowThoughts}\n...\n{highThoughts}");
        listingStandard.Gap();
        if (listingStandard.ButtonText("AMF.Reset".Translate()))
        {
            settings.ResetSettings();
        }

        listingStandard.Gap();
        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("AMF.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    private string getTimeFromLength(float timeInDays)
    {
        return ((int)Math.Round(Settings.MemoryLengthOffset * timeInDays * GenDate.TicksPerDay))
            .ToStringTicksToPeriod();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        AllMemoriesFade.UpdateEffectValues();
        AllMemoriesFade.UpdateLengthValues();
    }
}