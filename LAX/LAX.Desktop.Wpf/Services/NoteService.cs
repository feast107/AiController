﻿using System.Threading;
using System.Threading.Tasks;
using LAX.Desktop.Wpf.ViewModels;

namespace LAX.Desktop.Wpf.Services
{
    public class NoteService
    {
        public NoteData Data { get; } = new NoteData();

        private CancellationTokenSource? cancellation;

        private async Task ShowCoreAsync(string msg, int timeout, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            Data.Text = msg;
            Data.Show = true;

            try
            {
                await Task.Delay(timeout, token);

                if (token.IsCancellationRequested)
                    return;

                Data.Show = false;
            }
            catch (TaskCanceledException) { }
        }

        public Task ShowAsync(string msg, int timeout)
        {
            cancellation?.Cancel();
            cancellation = new CancellationTokenSource();

            return ShowCoreAsync(msg, timeout, cancellation.Token);
        }

        public void Show(string msg)
        {
            Data.Text = msg;
            Data.Show = true;
        }

        public void Close()
        {
            cancellation?.Cancel();

            Data.Show = false;
        }
    }
}
