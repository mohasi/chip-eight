using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace ChipEight {
  class Program {
    static void Main(string[] args) {
      var sys = new System();

      #region KEYMAP
      // TODO: load from file
      var keyMappings = new[] {
        Key.Num0,
        Key.Num1,
        Key.Num2,
        Key.Num3,
        Key.Num4,
        Key.Num5,
        Key.Num6,
        Key.Num7,
        Key.Num8,
        Key.Num9,
        Key.A,
        Key.B,
        Key.C,
        Key.D,
        Key.E,
        Key.F
      };
      #endregion

      using var window = new RenderWindow(new VideoMode(Consts.ScaledWidth, Consts.ScaledHeight), Consts.Title);

      #region EVENT HANDLING
      window.KeyPressed += (s, e) => {
        for (var i = 0; i < keyMappings.Length; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Down(i);
          }
        }
      };

      window.KeyReleased += (s, e) => {
        for (var i = 0; i < keyMappings.Length; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Up(i);
          }
        }
      };

      window.Closed += (s, e) => window.Close();
      #endregion

      // stripes
      for (int x = 0; x < Consts.Width; x += 2) {
        for (int y = 0; y < Consts.Height; y++) {
          sys.Screen.Set(x, y);
        }
      }

      var pixelSize = new Vector2f(Consts.Scale, Consts.Scale);
      var pixel = new RectangleShape {
        Size = pixelSize,
        FillColor = Color.White
      };

      while (window.IsOpen) {
        window.DispatchEvents();
        window.Clear();

        // draw pixels
        // TODO: experiment array -> image for faster draw?
        for (int x = 0; x < Consts.Width; x++) {
          for (int y = 0; y < Consts.Height; y++) {
            if (sys.Screen.IsSet(x, y)) {
              pixel.Position = new Vector2f(x * Consts.Scale, y * Consts.Scale);
              window.Draw(pixel);
            }
          }
        }

        window.Display();
      }
    }
  }
}
