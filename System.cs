using System;
using System.Collections.Generic;
using System.Text;

namespace ChipEight {
  class System {
    public Memory Memory { get; set; }
    public Registers Registers { get; set; }

    public System() {
      Memory = new Memory();
      Registers = new Registers();
    }
  }
}
