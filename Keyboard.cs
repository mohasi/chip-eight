using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChipEight {
  class Keyboard {
    private readonly char[] _keyMappings = new [] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
    private readonly bool[] _keyStates = new bool[Consts.KeyCount];

    private int GetVirtualKey(char keyboardKey) {
      for (int i = 0; i < Consts.KeyCount; i++) {
        if (_keyMappings[i] == keyboardKey) {
          return i;
        }
      }

      return -1;
    }

    public void Down(char keyboardKey) {
      var virtualKey = GetVirtualKey(keyboardKey);
      if (virtualKey != -1) {
        _keyStates[virtualKey] = true;
      }
    }

    public void Up(char keyboardKey) {
      var virtualKey = GetVirtualKey(keyboardKey);
      if (virtualKey != -1) {
        _keyStates[virtualKey] = false;
      }
    }

    public bool IsKeyDown(int virtualKey) {
      return _keyStates[virtualKey];
    }
  }
}
