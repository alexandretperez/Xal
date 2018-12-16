using System;
using System.Globalization;
using System.Threading;

namespace Xal.Util
{
    /// <summary>
    /// A utility class for <see cref="Thread"/> objects.
    /// </summary>
    public static class ThreadUtils
    {
        /// <summary>
        /// Executes an <paramref name="action"/> inside a configured scope with the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="action">The action.</param>
        public static void Localize(CultureInfo culture, Action action)
        {
            var original = CultureInfo.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = culture;
                action?.Invoke();
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        /// <summary>
        /// Executes an <paramref name="action"/> inside a configured scope with the specified culture.
        /// </summary>
        /// <param name="cultureName">The name of the culture.</param>
        /// <param name="action">The action.</param>
        public static void Localize(string cultureName, Action action)
        {
            Localize(CultureInfo.GetCultureInfo(cultureName), action);
        }

        /// <summary>
        /// Repeatedly invokes an action with the specified time <paramref name="interval"/> between each call.
        /// </summary>
        /// <param name="interval">The time, in milliseconds, between events. The value must be greater than zero and less than or equal to <see cref="int.MaxValue"/>.</param>
        /// <param name="action">The action.</param>
        /// <returns>A <see cref="System.Timers.Timer"/></returns>
        public static System.Timers.Timer SetInterval(int interval, Action<System.Timers.Timer> action)
        {
            return TimerCreate(true, interval, action);
        }
        /// <summary>
        /// Invokes an action after the spcified time <paramref name="delay"/>.
        /// </summary>
        /// <param name="delay">The time to wait, in milliseconds, before the <paramref name="action"/> is invoked. The value must be greater than zero and less than or equal to <see cref="int.MaxValue"/>.</param>
        /// <param name="action">The action.</param>
        /// <returns>A <see cref="System.Timers.Timer"/></returns>
        public static System.Timers.Timer SetTimeout(int delay, Action<System.Timers.Timer> action)
        {
            return TimerCreate(false, delay, action);
        }

        private static System.Timers.Timer TimerCreate(bool autoReset, int interval, Action<System.Timers.Timer> action)
        {
            var timer = new System.Timers.Timer(interval)
            {
                AutoReset = autoReset,
                Enabled = true
            };

            timer.Elapsed += (source, e) => action(timer);
            timer.Start();
            return timer;
        }
    }
}