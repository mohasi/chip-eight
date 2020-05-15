namespace ChipEight {
  class Consts {
    public const string Title = "Chip 8 Emulator";

    public const int Width = 64;
    public const int Height = 32;
    public const int Scale = 20;
    public const int ScaledWidth = Width * Scale;
    public const int ScaledHeight = Height * Scale;
    
    public const int MemorySize = 4096;
    public const int GeneralRegisterCount = 16;
    public const int StackDepth = 16;
    public const int KeyCount = 16;
  }
}
