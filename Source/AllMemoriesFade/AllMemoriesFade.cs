using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
public class AllMemoriesFade
{
    public static readonly Dictionary<string, List<float>> VanillaMoodValues;
    public static readonly Dictionary<string, bool> VanillaLerpValues;
    public static readonly List<ThoughtDef> AllMoodThoughts;
    public static readonly float MaxDuration;

    static AllMemoriesFade()
    {
        AllMoodThoughts = DefDatabase<ThoughtDef>.AllDefs.Where(def => def.durationDays > 0.1f).ToList();
        MaxDuration = AllMoodThoughts.Max(def => def.durationDays) * 1.1f;

        VanillaMoodValues = new Dictionary<string, List<float>>();
        VanillaLerpValues = new Dictionary<string, bool>();
        foreach (var thoughtDef in AllMoodThoughts)
        {
            VanillaLerpValues[thoughtDef.defName] = thoughtDef.lerpMoodToZero;
            VanillaMoodValues[thoughtDef.defName] = new List<float>();
            if (thoughtDef.stages == null || thoughtDef.stages.Count == 0)
            {
                continue;
            }

            foreach (var thoughtStage in thoughtDef.stages)
            {
                if (thoughtStage == null)
                {
                    VanillaMoodValues[thoughtDef.defName].Add(-1f);
                    continue;
                }

                VanillaMoodValues[thoughtDef.defName].Add(thoughtStage.baseMoodEffect);
            }
        }

        UpdateAffectedThoughts();
        UpdateMoodEffects();
    }

    public static void UpdateAffectedThoughts()
    {
        var currentAffectedThoughts = GetCurrentAffectedThoughts();
        foreach (var thoughtDef in AllMoodThoughts)
        {
            if (currentAffectedThoughts.Contains(thoughtDef))
            {
                thoughtDef.lerpMoodToZero = true;
                continue;
            }

            thoughtDef.lerpMoodToZero = VanillaLerpValues[thoughtDef.defName];
        }
    }

    public static void UpdateMoodEffects()
    {
        var currentAffectedThoughts = GetCurrentAffectedThoughts();
        if (AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset != 1f)
        {
            Log.Message(
                $"[AllMemoriesFade]: Changed {currentAffectedThoughts.Count} mood-offsets by {AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset.ToStringPercent()}");
        }

        foreach (var moodThought in AllMoodThoughts)
        {
            var customValue = currentAffectedThoughts.Contains(moodThought);
            for (var i = 0; i < moodThought.stages.Count; i++)
            {
                if (VanillaMoodValues[moodThought.defName][i] == -1f)
                {
                    continue;
                }

                if (customValue)
                {
                    moodThought.stages[i].baseMoodEffect = VanillaMoodValues[moodThought.defName][i] *
                                                           AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset;
                    continue;
                }

                moodThought.stages[i].baseMoodEffect = VanillaMoodValues[moodThought.defName][i];
            }
        }
    }

    public static List<ThoughtDef> GetCurrentAffectedThoughts()
    {
        return AllMoodThoughts.Where(def =>
                def.durationDays >= AllMemoriesFadeMod.instance.Settings.DurationRange.min &&
                def.durationDays <= AllMemoriesFadeMod.instance.Settings.DurationRange.max)
            .OrderBy(def => def.durationDays)
            .ToList();
    }
}