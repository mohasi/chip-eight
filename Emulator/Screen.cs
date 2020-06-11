using System;

namespace ChipEight {
  class Screen {
    private readonly bool[,] _pixels = new bool[Consts.Width, Consts.Height];
    private static readonly object _lockObj = new object();

    public bool Set(int x, int y) {
      lock (_lockObj) {
        return !(_pixels[x, y] ^= true);
      }
    }

    public bool IsSet(int x, int y) {
      lock (_lockObj) {
        return _pixels[x, y];
      }
    }

    public void Clear() {
      lock (_lockObj) {
        Array.Clear(_pixels, 0, _pixels.Length);
      }
    }
  }
}
