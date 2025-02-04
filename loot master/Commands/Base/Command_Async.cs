using System.Windows.Input;

namespace loot_master.Commands.Base
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
    internal abstract class CommandAsync : ICommand
    {
        private bool _Executable = true;
        public bool Executable
        {
            get => _Executable;
            set
            {
                if (_Executable == value) return;
                _Executable = value;
                // update self
                this.ChangeCanExecute();
                // update subscribers
                CallAllCanExecuteEvent?.Invoke();
            }
        }
      //  public event EventHandler? ExecutableChanged;

        readonly WeakEventManager _weakEventManager = new WeakEventManager();
        event EventHandler? ICommand.CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value, nameof(ChangeCanExecute));
            remove => _weakEventManager.RemoveEventHandler(value, nameof(ChangeCanExecute));
        }

        bool ICommand.CanExecute(object? parameter) => _Executable && CanExecute(parameter);
        async void ICommand.Execute(object? parameter)
        {
            if (!((ICommand)this).CanExecute(parameter)) return;
            try
            {
                Executable = false;
                await ExecuteAsync(parameter);
            }
            catch
            {
                throw;
            }
            finally
            {
                Executable = true;
            }
        }
        protected virtual bool CanExecute(object? p) => true;
        protected abstract Task ExecuteAsync(object? p);

        protected static event Action? CallAllCanExecuteEvent;
        public static void CallAllCanExecute() => CallAllCanExecuteEvent?.Invoke();
        public void ChangeCanExecute()
        {
            _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(ChangeCanExecute));
        }
    }

    internal class LambdaCommandAsync : CommandAsync
    {
        private readonly ActionAsync<object?> _Execute;
        private readonly Func<object?, bool>? _CanExecute;
        public LambdaCommandAsync(ActionAsync Execute, Func<bool>? CanExecute = null)
            : this(async p => await Execute(), CanExecute is null ? (Func<object?, bool>?)null : p => CanExecute()) { }
        public LambdaCommandAsync(ActionAsync<object?> Execute, Func<object?, bool>? CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }
        protected override bool CanExecute(object? p) => _CanExecute?.Invoke(p) ?? true;
        protected override Task ExecuteAsync(object? p) => _Execute(p);
    }
}
