namespace ChipEight {
  class Screen {
    private readonly bool[,] _pixels = new bool[Consts.Width, Consts.Height];

    public void Set(int x, int y) {
      _pixels[x, y] = true;
    }

    public bool IsSet(int x, int y) {
      return _pixels[x, y];
    }
  }
}
