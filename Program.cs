using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
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
      window.SetFramerateLimit(60);

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

      // overlapping & wrapping sprites
      sys.DrawSprite(60, 10, 5);
      sys.DrawSprite(63, 14, 5);

      // ready made pixel
      var pixelSize = new Vector2f(Consts.Scale, Consts.Scale);
      var pixel = new RectangleShape {
        Size = pixelSize,
        FillColor = Color.White
      };

      #region FPS
      var fpsText = new Text("FPS", new Font("consola.ttf")) {
        CharacterSize = 22,
        FillColor = Color.Green,
        Style = Text.Styles.Regular,
        Position = new Vector2f(10, 4)
      };

      var frameCount = 0L;
      var clock = new Stopwatch();
      clock.Start();
      #endregion

      sys.Registers.ST = 120; // beep for 2 secs;

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

        window.Draw(fpsText);
        window.Display();

        // update fps
        fpsText.DisplayedString = $"FPS:{Math.Ceiling(frameCount++ / clock.Elapsed.TotalSeconds)}";

        // count down delay timer (relies on 60Hz rate)
        if (sys.Registers.DT > 0)
          sys.Registers.DT--;

        // count down sound timer (relies on 60Hz rate)
        if (sys.Registers.ST > 0) {
          sys.Beep(); // f&f beep in bg
        }
      }
    }
  }
}
