using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChipEight {
  class Memory {
    private readonly byte[] _memory = new byte[Consts.MemorySize];

    public byte Get(int index) {
      AssertIndexInBounds(index);
      return _memory[index];
    }

    public void Set(int index, byte value) {
      AssertIndexInBounds(index);
      _memory[index] = value;
    }

    private void AssertIndexInBounds(int index) {
      Debug.Assert(index >= 0 && index < Consts.MemorySize);
    }
  }
}
