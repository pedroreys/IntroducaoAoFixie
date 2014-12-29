namespace BlogEngine.Infrastructure
{
    using System;

    public class SystemTime
    {
        private static Func<DateTimeOffset> _now = () => DateTimeOffset.Now;

        public static DateTimeOffset Now { get { return _now.Invoke(); } }

        public static IDisposable WithNowAs(DateTimeOffset now)
        {
            _now = () => now;
            return DisposableAction.Do(() => _now = () => DateTimeOffset.Now);
        }
    }
}