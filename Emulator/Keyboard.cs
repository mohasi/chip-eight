using System.Threading.Tasks;

namespace ChipEight {
  class Keyboard {
    private readonly bool[] _keyStates = new bool[Consts.KeyCount];
    private static readonly object _lockObj = new object();
    private static TaskCompletionSource<byte> _tcs;

    public void Press(byte key) {
      lock (_lockObj) {
        _keyStates[key] = true;
      }
    }

    public void Release(byte key) {
      lock (_lockObj) {
        _keyStates[key] = false;
      }

      if (_tcs != null && !_tcs.Task.IsCompleted) {
        _tcs.SetResult(key);
      }
    }

    public bool IsKeyPressed(byte key) {
      lock (_lockObj) {
        return _keyStates[key];
      }
    }

    public Task<byte> WaitForKeyPressAsync() {
      _tcs = new TaskCompletionSource<byte>();
      return _tcs.Task;
    }
  }
}
