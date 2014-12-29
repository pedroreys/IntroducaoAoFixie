namespace BlogEngine.Infrastructure
{
    using System;

    public static class DisposableAction
    {
        public static IDisposable Do(Action action)
        {
            return new InnerDisposable(action);
        }

        private class InnerDisposable : IDisposable
        {
            private readonly Action _action;

            public InnerDisposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}