namespace ChipEight {
  class Registers {
    public byte[] V { get; set; } = new byte[Consts.GeneralRegisterCount];
    public ushort I { get; set; }
    public byte DT { get; set; }
    public byte ST { get; set; }
    public ushort PC { get; set; }
  }
}
