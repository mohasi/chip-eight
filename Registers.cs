using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChipEight {
  class Registers {
    public byte[] V { get; set; } = new byte[Consts.GeneralRegisterCount];
    public ushort I { get; set; }
    public ushort DT { get; set; }
    public ushort ST { get; set; }
    public ushort PC { get; set; }
  }
}
