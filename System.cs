using System;
using System.IO;
using System.Threading.Tasks;

namespace ChipEight {
  class System {
    private readonly Random _rng = new Random();

    public Memory Memory { get; private set; }
    public Registers Registers { get; private set; }
    public Stack Stack { get; private set; }
    public Keyboard Keyboard { get; private set; }
    public Screen Screen { get; private set; }
    public bool IsWaitingForKey { get; set; }
    public System() {
      Memory = new Memory();
      Registers = new Registers();
      Stack = new Stack();
      Keyboard = new Keyboard();
      Screen = new Screen();
    }

    public bool DrawSprite(byte x, byte y, byte maxRows) {
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

      return collision;
    }

    public void Beep() {
      var msToBeep = (int)Math.Ceiling(Registers.ST * Consts.MillisecondsPerFrame);
      Registers.ST = 0;
      _ = Task.Run(() => {
        Console.Beep(Consts.SoundFrequency, msToBeep);
      });
    }

    public async Task Load(string filePath) {
      var programBytes = await File.ReadAllBytesAsync(filePath);
      Memory.Set(Consts.ProgramLoadAddress, programBytes);
      Registers.PC = Consts.ProgramLoadAddress;
    }

    public void Execute() {
      var highByte = Memory.Get(Registers.PC);
      var lowByte = Memory.Get(Registers.PC + 1);
      var opcode = new Opcode(highByte, lowByte);

      #region PROCESS OPCODE
      // do not increment PC for jumps
      if (opcode.A != 0x1 && opcode.A != 0xB) {
        Registers.PC += 2;
      }

      switch (opcode.A) {
        case 0x0:
          switch (opcode.Full) {
            case 0x00E0: // 00E0 - CLS (clear the display)
              Screen.Clear();
              break;
            case 0x00EE: // 00EE - RET (return from a subroutine)
              Registers.PC = Stack.Pop();
              break;
            default: // 0nnn - SYS addr (jump to a machine code routine at nnn)
              // ignore.
              break;
          }

          break;
        case 0x1: // 1nnn - JP addr (jump to location nnn)
          Registers.PC = opcode.NNN;
          break;
        case 0x2: // 2nnn - CALL addr (call subroutine at nnn)
          Stack.Push(Registers.PC);
          Registers.PC = opcode.NNN;
          break;
        case 0x3: // 3xkk - SE Vx, byte (skip next instruction if Vx = kk)
          if (Registers.V[opcode.X] == opcode.KK) {
            Registers.PC += 2;
          }
          break;
        case 0x4: // SNE Vx, byte (skip next instruction if Vx != kk)
          if (Registers.V[opcode.X] != opcode.KK) {
            Registers.PC += 2;
          }
          break;
        case 0x5: // 5xy0 - SE Vx, Vy (skip next instruction if Vx = Vy)
          if (Registers.V[opcode.X] == Registers.V[opcode.Y]) {
            Registers.PC += 2;
          }
          break;
        case 0x6: // 6xkk - LD Vx, byte (set Vx = kk)
          Registers.V[opcode.X] = opcode.KK;
          break;
        case 0x7: // 7xkk - ADD Vx, byte (set Vx = Vx + kk)
          Registers.V[opcode.X] += opcode.KK;
          break;
        case 0x8:
          switch (opcode.N) {
            case 0x0: // 8xy0 - LD Vx, Vy (set Vx = Vy)
              Registers.V[opcode.X] = Registers.V[opcode.Y];
              break;
            case 0x1: // 8xy1 - OR Vx, Vy (set Vx = Vx OR Vy)
              Registers.V[opcode.X] |= Registers.V[opcode.Y];
              break;
            case 0x2: // 8xy2 - AND Vx, Vy (set Vx = Vx AND Vy)
              Registers.V[opcode.X] &= Registers.V[opcode.Y];
              break;
            case 0x3: // 8xy3 - XOR Vx, Vy (set Vx = Vx XOR Vy)
              Registers.V[opcode.X] ^= Registers.V[opcode.Y];
              break;
            case 0x4: // 8xy4 - ADD Vx, Vy (set Vx = Vx + Vy, set VF = carry)
              var result = Registers.V[opcode.X] + Registers.V[opcode.Y];
              Registers.V[opcode.X] = (byte)result; // low byte
              Registers.V[0xF] = result > 0xFF ? (byte)1 : (byte)0;
              break;
            case 0x5: // 8xy5 - SUB Vx, Vy (set Vx = Vx - Vy, set VF = NOT borrow)
              Registers.V[0xF] = Registers.V[opcode.X] > Registers.V[opcode.Y] ? (byte)1 : (byte)0;
              Registers.V[opcode.X] -= Registers.V[opcode.Y];
              break;
            case 0x6: // SHR Vx {, Vy} (set Vx = Vx SHR 1)
              Registers.V[0xF] = (byte)(Registers.V[opcode.X] & 0x01);
              Registers.V[opcode.X] /= 2;
              break;
            case 0x7: // 8xy7 - SUBN Vx, Vy (set Vx = Vy - Vx, set VF = NOT borrow)
              Registers.V[0xF] = Registers.V[opcode.Y] > Registers.V[opcode.X] ? (byte)1 : (byte)0;
              Registers.V[opcode.X] = (byte)(Registers.V[opcode.Y] - Registers.V[opcode.X]);
              break;
            case 0xE: // 8xyE - SHL Vx {, Vy} (set Vx = Vx SHL 1)
              Registers.V[0xF] = (byte)(Registers.V[opcode.X] & 0xF0);
              Registers.V[opcode.X] *= 2;
              break;
          }
          break;
        case 0x9: // SNE Vx, Vy (skip next instruction if Vx != Vy)
          if (Registers.V[opcode.X] != Registers.V[opcode.Y]) {
            Registers.PC += 2;
          }
          break;
        case 0xA: // LD I, addr (set I = nnn)
          Registers.I = opcode.NNN;
          break;
        case 0xB: // JP V0, addr (jump to location nnn + V0)
          Registers.PC = (ushort)(opcode.NNN + Registers.V[0x0]);
          break;
        case 0xC: // RND Vx, byte (set Vx = random byte AND kk)
          Registers.V[opcode.X] = (byte)(_rng.Next(0, 255) & opcode.KK);
          break;
        case 0xD: // DRW Vx, Vy, nibble (display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision)
          Registers.V[0xF] = DrawSprite(Registers.V[opcode.X], Registers.V[opcode.Y], opcode.N) ? (byte)1 : (byte)0;
          break;
        case 0xE:
          switch (opcode.LowByte) {
            case 0x9E: // SKP Vx (skip next instruction if key with the value of Vx is pressed)
              if (Keyboard.IsKeyPressed(Registers.V[opcode.X])) {
                Registers.PC += 2;
              }
              break;
            case 0xA1: // ExA1 - SKNP Vx (skip next instruction if key with the value of Vx is not pressed)
              if (!Keyboard.IsKeyPressed(Registers.V[opcode.X])) {
                Registers.PC += 2;
              }
              break;
          }
          break;
        case 0xF:
          switch (opcode.LowByte) {
            case 0x07: // Fx07 - LD Vx, DT (set Vx = delay timer value)
              Registers.V[opcode.X] = Registers.DT;
              break;
            case 0x0A: // Fx0A - LD Vx, K (wait for a key press, store the value of the key in Vx)
              var key = Keyboard.WaitForKeyPressAsync().Result;
              Registers.V[opcode.X] = key;
              break;
            case 0x15: // Fx15 - LD DT, Vx (set delay timer = Vx)
              Registers.DT = Registers.V[opcode.X];
              break;
            case 0x18: // Fx18 - LD ST, Vx (set sound timer = Vx)
              Registers.ST = Registers.V[opcode.X];
              break;
            case 0x1E: // Fx1E - ADD I, Vx (set I = I + Vx)
              Registers.I += Registers.V[opcode.X];
              break;
            case 0x29: // Fx29 - LD F, Vx (set I = location of sprite for digit Vx)
              Registers.I = (ushort)(Consts.BytesPerSpriteRow * Registers.V[opcode.X]);
              break;
            case 0x33: // Fx33 - LD B, Vx (store BCD representation of Vx in memory locations I, I+1, and I+2)
              Memory.Set(Registers.I, (byte)(Registers.V[opcode.X] / 100)); // hundreds
              Memory.Set(Registers.I + 1, (byte)(Registers.V[opcode.X] % 100 / 10)); // tens
              Memory.Set(Registers.I + 2, (byte)(Registers.V[opcode.X] % 10)); // ones
              break;
            case 0x55: // Fx55 - LD [I], Vx (store registers V0 through Vx in memory starting at location I)
              for (int i = 0; i < opcode.X; i++) {
                Memory.Set(Registers.I + i, Registers.V[i]);
              }
              break;
            case 0x65: // Fx65 - LD Vx, [I] (read registers V0 through Vx from memory starting at location I)
              for (int i = 0; i <= opcode.X; i++) {
                Registers.V[i] = Memory.Get(Registers.I + i);
              }
              break;
          }
          break;
      }
      #endregion
    }
  }
}
