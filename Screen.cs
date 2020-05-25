﻿using System;

namespace ChipEight {
  class Screen {
    private readonly bool[,] _pixels = new bool[Consts.Width, Consts.Height];

    public bool Set(int x, int y) {
      return !(_pixels[x, y] ^= true);
    }

    public bool IsSet(int x, int y) {
      return _pixels[x, y];
    }

    public void Clear() {
      Array.Clear(_pixels, 0, _pixels.Length);
    }
  }
}
