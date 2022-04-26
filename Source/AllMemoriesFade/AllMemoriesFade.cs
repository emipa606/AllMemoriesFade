using System.Linq;
using RimWorld;
using Verse;

namespace AllMemoriesFade;

[StaticConstructorOnStartup]
public class AllMemoriesFade
{
    static AllMemoriesFade()
    {
        foreach (var thoughtDef in DefDatabase<ThoughtDef>.AllDefs.Where(def => def.durationDays > 0))
        {
            thoughtDef.lerpMoodToZero = true;
        }
    }
}