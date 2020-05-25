using System;

namespace ChipEight {
  class Consts {
    public const string Title = "Chip-8 Emulator";

    // display
    public const int Width = 64;
    public const int Height = 32;
    public const int Scale = 15;
    public const int ScaledWidth = Width * Scale;
    public const int ScaledHeight = Height * Scale;

    // memory related
    public const int MemorySize = 4096;
    public const int GeneralRegisterCount = 16;
    public const int StackDepth = 16;
    public const int KeyCount = 16;
    public const int BytesPerSpriteRow = 5;
    public const int ProgramLoadAddress = 512; // 0x200

    // sound
    public const int SoundFrequency = 800;

    // timing
    public const int TargetSystemFrameRate = 60;
    public const double MillisecondsPerFrame = 1000D / TargetSystemFrameRate;
    public const int CpuClockSpeedHertz = 540;
    public const int CyclesPerFrame = CpuClockSpeedHertz / TargetSystemFrameRate;
    public const double MillisecondsPerCycle = MillisecondsPerFrame / CyclesPerFrame;
    public const int TicksPerCycle = (int) (TimeSpan.TicksPerMillisecond * MillisecondsPerCycle);
  }
}
