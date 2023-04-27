namespace SourceSharp.Sdk.Structs;

public readonly struct ConVarBounds
{
    public float Min { get; } = 0.0f;
    public float Max { get; } = 0.0f;

    public bool HasMin { get; } = false;

    public bool HasMax { get; } = false;

    public ConVarBounds(float min, float max, bool hasMin, bool hasMax)
    {
        Min = min;
        Max = max;
        HasMin = hasMin;
        HasMax = hasMax;
    }
}
