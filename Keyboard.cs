using System.Threading.Tasks;

namespace ChipEight {
  class Keyboard {
    private readonly bool[] _keyStates = new bool[Consts.KeyCount];
    private TaskCompletionSource<byte> _tcs;

    public void Press(byte key) {
      _keyStates[key] = true;
      if (_tcs != null) {
        _tcs.SetResult(key);
      }
    }

    public void Release(byte key) {
      _keyStates[key] = false;
    }

    public bool IsKeyPressed(byte key) {
      return _keyStates[key];
    }

    public Task<byte> WaitForKeyPressAsync() {
      _tcs = new TaskCompletionSource<byte>();
      return _tcs.Task;
    }
  }
}
