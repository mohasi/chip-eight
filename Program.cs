using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChipEight {
  class Program {
    static void Main(string[] args) {
      using var window = new RenderWindow(new VideoMode(800, 600), "Hello SFML");
      window.Closed += (s, e) => window.Close();

      var shape = new RectangleShape(new Vector2f(100, 100)) {
        FillColor = Color.Cyan
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
