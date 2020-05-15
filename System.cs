namespace ChipEight {
  class System {
    public Memory Memory { get; private set; }
    public Registers Registers { get; private set; }
    public Stack Stack { get; private set; }
    public Keyboard Keyboard { get; private set; }
    public Screen Screen { get; private set; }

    public System() {
      Memory = new Memory();
      Registers = new Registers();
      Stack = new Stack();
      Keyboard = new Keyboard();
      Screen = new Screen();
    }
  }
}
