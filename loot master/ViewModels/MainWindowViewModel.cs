using System.Diagnostics;
using System.Windows.Input;
using loot_master.Commands.Base;
using loot_master.Resources.StaticData;
using loot_master.Service.Data;
using loot_master.ViewModels.Base;

namespace loot_master.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {

        private string _Title = StaticData.ProgramName;
        public string Title { get => _Title; set => Set(ref _Title, value); }


        public MainWindowViewModel(IDataService dataService)
        {
        }

        private void DoSomething()
        {
            Command_.CallAllCanExecute();
        }

        private int _Count = default!;
        public int Count { get => _Count; set => Set(ref _Count, value); }


        private DateTime _Time = default!;
        public DateTime Time { get => _Time; set => Set(ref _Time, value); }

        bool work = false;

        #region UpCommand - описание команды 
        private LambdaCommand? _UpCommand;
        public ICommand UpCommand => _UpCommand ??=
            new LambdaCommand(OnUpCommandExecuted, CanUpCommandExecute, true);
        private bool CanUpCommandExecute(object? p) => work;
        private void OnUpCommandExecuted(object? p)
        {
            Count++;
            work = !work;
        }
        #endregion



        #region DownCommand - описание команды 
        private LambdaCommand? _DownCommand;
        public ICommand DownCommand => _DownCommand ??=
            new LambdaCommand(OnDownCommandExecuted, CanDownCommandExecute, true);
        private bool CanDownCommandExecute(object? p) => !work;
        private void OnDownCommandExecuted(object? p)
        {
            Count++;
            work = !work;
        }
        #endregion


        #region TimeCommand - описание команды 
        private LambdaCommand? _TimeCommand;
        private IDispatcherTimer? timer;

        public ICommand TimeCommand => _TimeCommand ??=
            new LambdaCommand(OnTimeCommandExecuted, CanTimeCommandExecute);
        private bool CanTimeCommandExecute(object? p) => true;
        private void OnTimeCommandExecuted(object? p)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= SetTime;
                timer = null;
            }
            else
            {
                if (Application.Current != null && Application.Current.Dispatcher != null)
                {
                    timer = Application.Current.Dispatcher.CreateTimer();
                    Debug.WriteLine(TimeSpan.FromSeconds(1).Ticks);
                    Debug.WriteLine(TimeSpan.FromMilliseconds(1000).Ticks);
                    Debug.WriteLine(TimeSpan.FromMicroseconds(1000).Ticks);
                    timer.Interval = TimeSpan.FromMilliseconds(330);
                    timer.Tick += SetTime;
                    timer.Start();
                }
            }
        }

        private void SetTime(object? sender, EventArgs e)
        {
            Time = DateTime.Now;
            Command_.CallAllCanExecute();
        }
        #endregion



        #region AsyncCommand - описание команды 
        private LambdaCommandAsync? _AsyncCommand;
        public ICommand AsyncCommand => _AsyncCommand ??=
            new LambdaCommandAsync(OnAsyncCommandExecuted, CanAsyncCommandExecute);
        private bool CanAsyncCommandExecute(object? p) => true;
        private async Task OnAsyncCommandExecuted(object? p)
        {
            await Task.Run(() => { Thread.Sleep(2000); });
        }
        #endregion

    }
}
