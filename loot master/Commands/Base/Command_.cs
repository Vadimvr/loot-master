using System.Windows.Input;

namespace loot_master.Commands.Base
{
    internal abstract class Command_ : ICommand
    {
        private bool _Executable = true;
        public bool Executable
        {
            get { return _Executable; }
            set
            {
                if (_Executable == value) { return; }
                _Executable = value;
                // update subscribers
                CallAllCanExecuteEvent?.Invoke();
            }
        }
        readonly WeakEventManager _weakEventManager = new WeakEventManager();
        public event EventHandler? CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value, nameof(ChangeCanExecute));
            remove => _weakEventManager.RemoveEventHandler(value, nameof(ChangeCanExecute));
        }
        bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);
        void ICommand.Execute(object? parameter)
        {
            if (!((ICommand)this).CanExecute(parameter)) { return; };
            try
            {
                Executable = false;
                Execute(parameter);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Executable = true;
            }
        }
        protected virtual bool CanExecute(object? p) => true;
        protected abstract void Execute(object? p);


        protected static event Action? CallAllCanExecuteEvent;
        public static void CallAllCanExecute() => CallAllCanExecuteEvent?.Invoke();
        public void ChangeCanExecute()
        {
            _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(ChangeCanExecute));
        }
    }
    internal class LambdaCommand : Command_
    {
        private readonly Action<object?> _Execute;
        private readonly Func<object?, bool>? _CanExecute;

        /// <summary>
        /// Creating a command using a lambda expression
        /// </summary>
        /// <param name="Execute">Action</param>
        /// <param name="CanExecute">Can the command be executed</param>
        /// <param name="unitedCallCanExecute" default="false">Subscribe to update CanExecute after running other command instances</param>
        public LambdaCommand(Action Execute, Func<bool>? CanExecute = null, bool unitedCallCanExecute = false)
            : this(p => Execute(), CanExecute is null ? (Func<object?, bool>?)null : p => CanExecute(), unitedCallCanExecute) { }

        /// <summary>
        /// Creating a command using a lambda expression
        /// </summary>
        /// <param name="Execute">Action</param>
        /// <param name="CanExecute">Can the command be executed</param>
        /// <param name="unitedCallCanExecute">Subscribe to update CanExecute after running other command instances</param>
        public LambdaCommand(Action<object?> Execute, Func<object?, bool>? CanExecute = null, bool unitedCallCanExecute = false)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
            if (unitedCallCanExecute)
            {
                CallAllCanExecuteEvent += this.ChangeCanExecute;
            }
        }
        protected override bool CanExecute(object? p) => _CanExecute?.Invoke(p) ?? true;
        protected override void Execute(object? p) => _Execute(p);
    }
}
