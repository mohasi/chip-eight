namespace ChipEight {
  class Stack {
    private readonly ushort[] _stack = new ushort[Consts.StackDepth];

    public byte SP { get; private set; }

    public void Push(ushort value) {
      _stack[++SP] = value;
    }

    public ushort Pop() {
      return _stack[SP--];
    }
  }
}
