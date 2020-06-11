namespace ChipEight {
  struct Opcode {
    public Opcode(byte high, byte low) {
      HighByte = high;
      LowByte = low;
      Full = (ushort)(HighByte << 8 | LowByte);
      A = (byte)(HighByte >> 4);
      X = (byte)(HighByte & 0xF);
      Y = (byte)(LowByte >> 4);
      N = (byte)(LowByte & 0xF);
      KK = LowByte;
      NNN = (ushort)(Full & 0x0FFF);
    }

    public byte HighByte { get; }
    public byte LowByte { get; }
    public ushort Full { get; }
    public byte A { get; }
    public byte X { get; }
    public byte Y { get; }
    public byte N { get; }
    public byte KK { get; }
    public ushort NNN { get; }

    public override string ToString() => $"0x{Full:X}";
  }
}
