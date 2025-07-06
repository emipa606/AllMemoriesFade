using Verse;

namespace AllMemoriesFade;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class AllMemoriesFadeSettings : ModSettings
{
    public FloatRange DurationRange = new(0, float.MaxValue);
    public float MemoryLengthOffset = 1f;
    public float MemoryMoodOffset = 1f;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref MemoryMoodOffset, "MemoryMoodOffset", 1f);
        Scribe_Values.Look(ref MemoryLengthOffset, "MemoryLengthOffset", 1f);
        Scribe_Values.Look(ref DurationRange, "DurationRange", new FloatRange(0, float.MaxValue));
    }

    public void ResetSettings()
    {
        DurationRange = new FloatRange(0, float.MaxValue);
        MemoryLengthOffset = 1f;
        AllMemoriesFade.UpdateLengthValues();
        DurationRange = new FloatRange(0, AllMemoriesFade.MaxDuration);
        MemoryMoodOffset = 1f;
        AllMemoriesFade.UpdateLengthValues();
        AllMemoriesFade.UpdateLerpValues();
        AllMemoriesFade.UpdateEffectValues();
    }
}