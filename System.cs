using System.Diagnostics;

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

    public void DrawSprite(int x, int y, int maxRows) {
      var collision = false;

      for (var row = 0; row < maxRows; row++) {
        var spriteRowByte = Memory.Get(Registers.I + row);
        for (var bit = 0; bit < 8; bit++) {
          var bitmask = 0b1000_0000 >> bit;
          var isSpriteBitSet = (spriteRowByte & bitmask) != 0;
          if (isSpriteBitSet) {
            var xPos = (x + bit) % Consts.Width; // % for wrapping
            var yPos = (y + row) % Consts.Height; // % for wrapping
            var pixelErased = Screen.Set(xPos, yPos);
            if (pixelErased) {
              collision = true;
            }
          }
        }
      }

      // VF = 1 if pixels collided else it should be 0
      Registers.V[0xF] = collision ? (byte)1 : (byte)0;
    }
  }
}
