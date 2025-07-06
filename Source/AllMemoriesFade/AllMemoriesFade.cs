using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
public class AllMemoriesFade
{
    private static readonly Dictionary<string, List<float>> vanillaMoodValues;
    private static readonly Dictionary<string, float> vanillaLengthValues;
    private static readonly Dictionary<string, bool> vanillaLerpValues;
    private static readonly List<ThoughtDef> allMoodThoughts;
    public static float MaxDuration;

    static AllMemoriesFade()
    {
        allMoodThoughts = DefDatabase<ThoughtDef>.AllDefs.Where(def => def.durationDays > 0.1f).ToList();
        MaxDuration = allMoodThoughts.Max(def => def.durationDays) * 1.1f;

        vanillaMoodValues = new Dictionary<string, List<float>>();
        vanillaLerpValues = new Dictionary<string, bool>();
        vanillaLengthValues = new Dictionary<string, float>();
        foreach (var thoughtDef in allMoodThoughts)
        {
            vanillaLerpValues[thoughtDef.defName] = thoughtDef.lerpMoodToZero;
            vanillaLengthValues[thoughtDef.defName] = thoughtDef.durationDays;
            vanillaMoodValues[thoughtDef.defName] = new List<float>();
            if (thoughtDef.stages == null || thoughtDef.stages.Count == 0)
            {
                continue;
            }

            foreach (var thoughtStage in thoughtDef.stages)
            {
                if (thoughtStage == null)
                {
                    vanillaMoodValues[thoughtDef.defName].Add(-1f);
                    continue;
                }

                vanillaMoodValues[thoughtDef.defName].Add(thoughtStage.baseMoodEffect);
            }
        }

        UpdateLerpValues();
        UpdateLengthValues();
        UpdateEffectValues();
    }

    public static void UpdateLerpValues()
    {
        var currentAffectedThoughts = GetCurrentAffectedThoughts();
        foreach (var thoughtDef in allMoodThoughts)
        {
            if (currentAffectedThoughts.Contains(thoughtDef))
            {
                thoughtDef.lerpMoodToZero = true;
                continue;
            }

            thoughtDef.lerpMoodToZero = vanillaLerpValues[thoughtDef.defName];
        }
    }

    public static void UpdateLengthValues()
    {
        var currentAffectedThoughts = GetCurrentAffectedThoughts();
        if (AllMemoriesFadeMod.Instance.Settings.MemoryLengthOffset != 1f)
        {
            Log.Message(
                $"[AllMemoriesFade]: Changed {currentAffectedThoughts.Count} length-offsets by {AllMemoriesFadeMod.Instance.Settings.MemoryLengthOffset.ToStringPercent()}");
        }

        foreach (var thoughtDef in allMoodThoughts)
        {
            if (currentAffectedThoughts.Contains(thoughtDef))
            {
                thoughtDef.durationDays = vanillaLengthValues[thoughtDef.defName] *
                                          AllMemoriesFadeMod.Instance.Settings.MemoryLengthOffset;
                continue;
            }

            thoughtDef.durationDays = vanillaLengthValues[thoughtDef.defName];
        }

        MaxDuration = allMoodThoughts.Max(def => def.durationDays) * 1.1f;
    }

    public static void UpdateEffectValues()
    {
        var currentAffectedThoughts = GetCurrentAffectedThoughts();
        if (AllMemoriesFadeMod.Instance.Settings.MemoryMoodOffset != 1f)
        {
            Log.Message(
                $"[AllMemoriesFade]: Changed {currentAffectedThoughts.Count} mood-offsets by {AllMemoriesFadeMod.Instance.Settings.MemoryMoodOffset.ToStringPercent()}");
        }

        foreach (var moodThought in allMoodThoughts)
        {
            var customValue = currentAffectedThoughts.Contains(moodThought);
            for (var i = 0; i < moodThought.stages.Count; i++)
            {
                if (vanillaMoodValues[moodThought.defName][i] == -1f)
                {
                    continue;
                }

                if (customValue)
                {
                    moodThought.stages[i].baseMoodEffect = vanillaMoodValues[moodThought.defName][i] *
                                                           AllMemoriesFadeMod.Instance.Settings.MemoryMoodOffset;
                    continue;
                }

                moodThought.stages[i].baseMoodEffect = vanillaMoodValues[moodThought.defName][i];
            }
        }
    }

    public static List<ThoughtDef> GetCurrentAffectedThoughts()
    {
        return allMoodThoughts.Where(def =>
                def.durationDays >= AllMemoriesFadeMod.Instance.Settings.DurationRange.min &&
                def.durationDays <= AllMemoriesFadeMod.Instance.Settings.DurationRange.max)
            .OrderBy(def => def.durationDays)
            .ToList();
    }
}