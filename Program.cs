﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace ChipEight {
  class Program {
    static async Task Main(string[] args) {
      // init system
      var sys = new System();
      await sys.Load("Games/BRIX");

      // create window
      using var window = new RenderWindow(new VideoMode(Consts.ScaledWidth, Consts.ScaledHeight), Consts.Title);
      window.SetFramerateLimit(Consts.FrameRate);

      #region KEYMAP & EVENT HANDLING
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

      window.KeyPressed += (s, e) => {
        for (byte i = 0; i < keyMappings.Length; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Press(i);
          }
        }
      };

      window.KeyReleased += (s, e) => {
        for (byte i = 0; i < keyMappings.Length; i++) {
          if (keyMappings[i] == e.Code) {
            sys.Keyboard.Release(i);
          }
        }
      };

      window.Closed += (s, e) => window.Close();
      #endregion

      var bgColour = new Color(143, 145, 133);
      var fgColour = new Color(17, 29, 43);

      // ready made pixel
      var pixelSize = new Vector2f(Consts.Scale, Consts.Scale);
      var pixel = new RectangleShape {
        Size = pixelSize,
        FillColor = fgColour
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

      // cpu loop in another thread
      _ = Task.Run(async () => {
        var sw = new Stopwatch();
        sw.Start();

        var ticksPerCycle = TimeSpan.FromTicks(Consts.TicksPerCycle);
        while (true) {
          await sys.Execute();

          // count down delay timer (relies on 60Hz rate)
          if (sys.Registers.DT > 0)
            sys.Registers.DT--;

          // count down sound timer (relies on 60Hz rate)
          if (sys.Registers.ST > 0) {
            sys.Beep(); // f&f beep in bg
          }

          // control speed of cpu, each cpu cycle should take about 1.85ms (18518 ticks)
          // if it takes less, then wait for the remaining period
          sw.Stop();
          var ticksToSleep = Consts.TicksPerCycle > sw.ElapsedTicks ? Consts.TicksPerCycle - sw.ElapsedTicks : 0;
          // Debug.WriteLine($"cycle took: {sw.ElapsedTicks} ticks, sleeping {ticksToSleep} ticks");
          Thread.Sleep(TimeSpan.FromTicks(ticksToSleep));
          sw.Restart();
        }
      });

      // gfx loop
      while (window.IsOpen) {
        window.DispatchEvents();
        window.Clear(bgColour);

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
      }
    }
  }
}
