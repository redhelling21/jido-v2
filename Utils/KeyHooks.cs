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
    public class KeyHooksManager : IKeyHooksManager
    {
        private TaskPoolGlobalHook _hook = new();
        public Dictionary<KeyCode, EventHandler> _keyPressedEvents = new();

        //private Dictionary<string, KeyHook> _hooks = new Dictionary<string, KeyHook>();

        public KeyHooksManager()
        {
            _hook.KeyPressed += OnKeyPressed;
            _hook.RunAsync();
        }

        public void RegisterKey(KeyCode key, EventHandler pressed)
        {
            if (_keyPressedEvents.ContainsKey(key))
            {
                throw new Exception("Key already registered");
            }
            _keyPressedEvents.Add(key, pressed);
        }

        public void UnregisterKey(KeyCode key)
        {
            if (_keyPressedEvents.ContainsKey(key))
            {
                _keyPressedEvents.Remove(key);
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
        }

        public Task<KeyCode> ListenNextKey()
        {
            var tcs = new TaskCompletionSource<KeyCode>();
            EventHandler<KeyboardHookEventArgs> handler = null;
            handler = (sender, e) =>
            {
                tcs.SetResult(e.RawEvent.Keyboard.KeyCode);
                _hook.KeyPressed -= handler;
            };
            _hook.KeyPressed += handler;
            return tcs.Task;
        }

        public void Dispose()
        {
            _hook.Dispose();
        }
    }

    public interface IKeyHooksManager : IDisposable
    {
        void RegisterKey(KeyCode key, EventHandler pressed);

        void UnregisterKey(KeyCode key);

        Task<KeyCode> ListenNextKey();
    }
}
