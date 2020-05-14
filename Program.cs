using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace ChipEight {
  class Program {
    static void Main(string[] args) {
      var sys = new System();
      using var window = new RenderWindow(new VideoMode(Consts.Width * Consts.Scale, Consts.Height * Consts.Scale), Consts.Title);

      var keyMappings = new[] { Key.Num0, Key.Num1, Key.Num2, Key.Num3, Key.Num4, Key.Num5, Key.Num6, Key.Num7, Key.Num8, Key.Num9, Key.A, Key.B, Key.C, Key.D, Key.E, Key.F }; // TODO: load from file
      window.KeyPressed += (s, e) => {
        for (var i = 0; i < Consts.KeyCount; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Down(i);
          }
        }
      };

      window.KeyReleased += (s, e) => {
        for (var i = 0; i < Consts.KeyCount; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Up(i);
          }
        }
      };

      window.Closed += (s, e) => window.Close();

      var shape = new RectangleShape(new Vector2f(100, 100)) {
        FillColor = Color.Yellow,
        Position = new Vector2f(100, 100)
      };

      while (window.IsOpen) {
        window.DispatchEvents();
        window.Clear();
        window.Draw(shape);
        window.Display();
      }
    }
  }
}
