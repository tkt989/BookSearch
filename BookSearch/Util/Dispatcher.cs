using System;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace BookSearch.Util
{
    public interface IDispatcher
    {
        void Dispatch(Action action);
    }

    public class CurrentThreadDispatcher : IDispatcher
    {
        void IDispatcher.Dispatch(Action action)
        {
            action?.Invoke();
        }
    }

    public class MainThreadDispatcher : IDispatcher
    {
        void IDispatcher.Dispatch(Action action)
        {
            Task.Run(() => Device.BeginInvokeOnMainThread(action));
        }
    }
}

