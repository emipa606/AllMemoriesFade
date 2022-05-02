using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
public class AllMemoriesFade
{
    public static readonly Dictionary<string, List<float>> VanillaMoodValues;
    public static readonly List<ThoughtDef> AllMoodThoughts;

    static AllMemoriesFade()
    {
        AllMoodThoughts = DefDatabase<ThoughtDef>.AllDefs.Where(def => def.durationDays > 0).ToList();
        VanillaMoodValues = new Dictionary<string, List<float>>();
        foreach (var thoughtDef in AllMoodThoughts)
        {
            thoughtDef.lerpMoodToZero = true;
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

        UpdateMoodEffects();
    }

    public static void UpdateMoodEffects()
    {
        if (AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset != 1f)
        {
            Log.Message(
                $"[AllMemoriesFade]: Changed all time-based mood-offsets by {AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset.ToStringPercent()}");
        }

        foreach (var moodThought in AllMoodThoughts)
        {
            for (var i = 0; i < moodThought.stages.Count; i++)
            {
                if (VanillaMoodValues[moodThought.defName][i] == -1f)
                {
                    continue;
                }

                moodThought.stages[i].baseMoodEffect = VanillaMoodValues[moodThought.defName][i] *
                                                       AllMemoriesFadeMod.instance.Settings.MemoryMoodOffset;
            }
        }
    }
}