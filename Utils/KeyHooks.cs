using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SharpHook;
using SharpHook.Native;

namespace Jido.Utils
{
    public class KeyHooksManager : IDisposable, IKeyHooksManager
    {
        private TaskPoolGlobalHook _hook = new();
        public Dictionary<KeyCode, EventHandler> _keyPressedEvents = new();
        public Dictionary<KeyCode, EventHandler> _keyReleasedEvents = new();

        //private Dictionary<string, KeyHook> _hooks = new Dictionary<string, KeyHook>();

        public KeyHooksManager()
        {
            _hook.KeyPressed += OnKeyPressed;
            _hook.KeyReleased += OnKeyReleased;
        }

        public void RegisterKey(KeyCode key, EventHandler pressed, EventHandler released)
        {
            if (_keyPressedEvents.ContainsKey(key) || _keyReleasedEvents.ContainsKey(key))
            {
                throw new Exception("Key already registered");
            }
            _keyPressedEvents.Add(key, pressed);
            _keyReleasedEvents.Add(key, released);
        }

        public void UnregisterKey(KeyCode key)
        {
            if (_keyPressedEvents.ContainsKey(key) && _keyReleasedEvents.ContainsKey(key))
            {
                _keyPressedEvents.Remove(key);
                _keyReleasedEvents.Remove(key);
            }
            else
            {
                throw new Exception("Key not registered");
            }
        }

        public void OnKeyPressed(object? sender, KeyboardHookEventArgs args)
        {
            if (_keyPressedEvents.ContainsKey(args.RawEvent.Keyboard.KeyCode))
            {
                _keyPressedEvents[args.RawEvent.Keyboard.KeyCode]?.Invoke(sender, args);
            }
            else
            {
                throw new Exception("Key not registered");
            }
        }

        public void OnKeyReleased(object? sender, KeyboardHookEventArgs args)
        {
            if (_keyReleasedEvents.ContainsKey(args.RawEvent.Keyboard.KeyCode))
            {
                _keyReleasedEvents[args.RawEvent.Keyboard.KeyCode]?.Invoke(sender, args);
            }
            else
            {
                throw new Exception("Key not registered");
            }
        }

        public void Dispose()
        {
            _hook.Dispose();
        }
    }

    public interface IKeyHooksManager
    {
        void RegisterKey(KeyCode key, EventHandler pressed, EventHandler released);

        void UnregisterKey(KeyCode key);
    }
}
