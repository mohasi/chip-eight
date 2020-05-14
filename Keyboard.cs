namespace ChipEight {
  class Keyboard {
    private readonly bool[] _keyStates = new bool[Consts.KeyCount];

    public void Down(int key) {
      _keyStates[key] = true;
    }

    public void Up(int key) {
      _keyStates[key] = false;
    }

    public bool IsKeyDown(int key) {
      return _keyStates[key];
    }
  }
}
