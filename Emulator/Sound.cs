using System;
using System.Threading.Tasks;

namespace ChipEight {
  class Sound {
    public void Beep(int msToBeep) {
      Task.Run(() => {
        Console.Beep(Consts.SoundFrequency, msToBeep);
      });
    }
  }
}
