using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xal.Extensions;

namespace Xal.Data
{
    /// <summary>
    /// Provides a class that executes a group of actions based on the key-value of a dictionary.
    /// </summary>
    public class KeyValueSwitch
    {
        private readonly List<SwitchCase> _actions = new List<SwitchCase>();
        private readonly CultureInfo _culture;
        private readonly Dictionary<string, string> _dictionary;
        private Action _default;

        /// <summary>
        /// Initializes a new instance of <see cref="KeyValueSwitch"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary whose keys and values will be used on the test cases.</param>
        /// <param name="preserve">When <c>true</c> preserves the original <paramref name="dictionary"/> as it is; otherwise, creates a copy of it using the <see cref="StringComparer.OrdinalIgnoreCase"/> as the <see cref="IEqualityComparer{T}"/>.</param>
        /// <param name="culture">The culture that must be used to convert the values found on the <paramref name="dictionary"/>.</param>
        public KeyValueSwitch(Dictionary<string, string> dictionary, bool preserve, CultureInfo culture = null)
        {
            _dictionary = preserve ? dictionary : new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
            _culture = culture ?? CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="KeyValueSwitch"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary whose keys and values will be used on the test cases.</param>
        /// <param name="culture">The culture that must be used to convert the values found on the <paramref name="dictionary"/>.</param>
        public KeyValueSwitch(Dictionary<string, string> dictionary, CultureInfo culture = null) : this(dictionary, false, culture)
        {
        }

        /// <summary>
        /// Returns the number of queued actions to be executed except the default one.
        /// </summary>
        public int Count => _actions.Count;

        /// <summary>
        /// Marks the last queued action to be exclusive aborting all the other queued actions if this last one is executed.
        /// </summary>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch Break()
        {
            if (_actions.Count > 0)
                _actions[_actions.Count - 1].Break();

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed if the <paramref name="key"/> is present on the dictionary and its value is not null or empty.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> exists on the dictionary and its value is not null or empty.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch Case(string key, Action<string> action)
        {
            if (_dictionary.TryGetValue(key, out string value))
                Append(value, v => !v.IsNullOrEmpty(), action);

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed if the <paramref name="key"/> is present on the dictionary and its value is convertible to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type that the value should be converted.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> exists on the dictionary and its value is convertible to specified type <typeparamref name="T"/>.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch Case<T>(string key, Action<T> action) where T : IConvertible
        {
            if (_dictionary.TryGetValue(key, out string value))
            {
                var response = ConvertTo<T>(value, _culture, out T result);
                Append(result, _ => response, action);
            }

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed if the <paramref name="key"/> is present on the dictionary.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> exists on the dictionary.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch CaseKey(string key, Action<string> action)
        {
            if (_dictionary.TryGetValue(key, out string value))
                _actions.Add(new SwitchCase(() => action(value)));

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed if the <paramref name="key"/> is not present on the dictionary.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> does not exists on the dictionary.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch CaseNot(string key, Action action)
        {
            if (!_dictionary.ContainsKey(key))
                _actions.Add(new SwitchCase(action));

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed when the value of the dictionary <paramref name="key"/> is equals to the <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="T">The type that the value should be converted.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> exists on the dictionary and its value is equals to the <paramref name="expected"/>.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch CaseWhen<T>(string key, T expected, Action<T> action) where T : IConvertible
        {
            if (_dictionary.TryGetValue(key, out string value) && ConvertTo<T>(value, _culture, out T result))
                Append(result, r => Equals(r, expected), action);

            return this;
        }

        /// <summary>
        /// Queues the <paramref name="action"/> to be executed when the value of the dictionary <paramref name="key"/> is validated by the specified <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T">The type that the value should be converted.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <param name="condition">The condition to be tested.</param>
        /// <param name="action">The action that should be executed when the <paramref name="key"/> exists on the dictionary and its value is is validated by the specified <paramref name="condition"/>.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch CaseWhen<T>(string key, Func<T, bool> condition, Action<T> action) where T : IConvertible
        {
            if (_dictionary.TryGetValue(key, out string value) && ConvertTo<T>(value, _culture, out T result))
                Append(result, condition, action);

            return this;
        }

        /// <summary>
        /// The default action to be executed if there is no queued actions.
        /// </summary>
        /// <param name="action">The default action.</param>
        /// <returns>The current instance of <see cref="KeyValueSwitch"/></returns>
        public KeyValueSwitch Default(Action action)
        {
            _default = action;
            return this;
        }

        /// <summary>
        /// Executes the queued actions.
        /// </summary>
        public void Run()
        {
            if (Count == 0)
            {
                _default?.Invoke();
                return;
            }

            foreach (var action in _actions.Where(p => p.BreakAfterRun))
            {
                action.Action();
                return;
            }

            foreach (var action in _actions)
                action.Action();
        }

        private static bool ConvertTo<T>(string value, CultureInfo culture, out T result) where T : IConvertible
        {
            result = default(T);

            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value; // faster
                return true;
            }

            try
            {
                result = (T)Convert.ChangeType(value, typeof(T), culture);
                return true;
            }
            catch { }

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    result = (T)converter.ConvertFrom(null, culture, value);
                    return true;
                }

                converter = TypeDescriptor.GetConverter(typeof(string));
                if (!converter.CanConvertTo(typeof(T)))
                    return false;

                result = (T)converter.ConvertTo(value, typeof(T));
                return true;
            }
            catch { }

            return false;
        }

        private KeyValueSwitch Append<T>(T value, Func<T, bool> condition, Action<T> action) where T : IConvertible
        {
            if (condition(value))
                _actions.Add(new SwitchCase(() => action(value)));

            return this;
        }

        private class SwitchCase
        {
            public SwitchCase(Action action)
            {
                Action = action;
                BreakAfterRun = false;
            }

            public Action Action { get; }

            public bool BreakAfterRun { get; private set; }

            public void Break()
            {
                BreakAfterRun = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="KeyValueSwitch"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary whose keys and values will be used on the test cases.</param>
        /// <param name="preserve">When <c>true</c> preserves the original <paramref name="dictionary"/> as it is; otherwise, creates a copy of it using the <see cref="StringComparer.OrdinalIgnoreCase"/> as the <see cref="IEqualityComparer{T}"/>.</param>
        /// <param name="culture">The culture that must be used to convert the values found on the <paramref name="dictionary"/>.</param>
        /// <returns>A <see cref="KeyValueSwitch"/> instance.</returns>
        public static KeyValueSwitch From(Dictionary<string, string> dictionary, bool preserve, CultureInfo culture = null) => new KeyValueSwitch(dictionary, preserve, culture);

        /// <summary>
        /// Initializes a new instance of <see cref="KeyValueSwitch"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary whose keys and values will be used on the test cases.</param>
        /// <param name="culture">The culture that must be used to convert the values found on the <paramref name="dictionary"/>.</param>
        /// <returns>A <see cref="KeyValueSwitch"/> instance.</returns>
        public static KeyValueSwitch From(Dictionary<string, string> dictionary, CultureInfo culture = null) => From(dictionary, false, culture);
    }
}