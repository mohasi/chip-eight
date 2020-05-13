using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

namespace ChipEight {
  class Program {
    static void Main(string[] args) {
      using var window = new RenderWindow(new VideoMode(640, 320), "Chip 8 Emulator");
      window.Closed += (s, e) => window.Close();

      var shape = new RectangleShape(new Vector2f(100, 100)) {
        FillColor = Color.Yellow,
        Position = new Vector2f(100, 100)
      };

      var fpsText = new Text("FPS", new Font("consola.ttf")) {
        CharacterSize = 22,
        FillColor = Color.Red,
        Style = Text.Styles.Regular,
        Position = new Vector2f(10, 2)
      };

      var frameCount = 0L;
      var clock = new Stopwatch();
      clock.Start();

      while (window.IsOpen) {
        window.DispatchEvents();
        window.Clear();
        window.Draw(shape);
        window.Draw(fpsText);
        window.Display();

        fpsText.DisplayedString = $"FPS:{(int) (frameCount++ / clock.Elapsed.TotalSeconds)}";
      }
    }
  }
}
