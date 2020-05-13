using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChipEight {
  class Program {
    static void Main(string[] args) {
      var videoMode = new VideoMode(Consts.Width * Consts.Scale, Consts.Height * Consts.Scale);
      using var window = new RenderWindow(videoMode, Consts.Title);
      window.Closed += (s, e) => window.Close();

      var shape = new RectangleShape(new Vector2f(100, 100)) {
        FillColor = Color.Yellow,
        Position = new Vector2f(100, 100)
      };

      var sys = new System();
      sys.Keyboard.Down('A');
      var a = sys.Keyboard.IsKeyDown(9);
      var b = sys.Keyboard.IsKeyDown(10);

      while (window.IsOpen) {
        window.DispatchEvents();
        window.Clear();
        window.Draw(shape);
        window.Display();
      }
    }
  }
}
