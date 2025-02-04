using loot_master.Commands.Base;
using loot_master.Models;
using loot_master.Service.Data;
using loot_master.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace loot_master.ViewModels
{
    internal class LogWinnerViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        #region ViewName type string -  
        private string _ViewName = "Лог";

        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion
        public LogWinnerViewModel(IDataService dataService)
        {
            _dataService = dataService;
            if (dataService != null)
            {
                dataService.AddWinerInLogEvent += AddFirstRecord;
            }
        }

        private void AddFirstRecord(string arg1, DateTime time)
        {
            _dataService.AddWinerInLogEvent += AddWinerInLog;
            AddWinerInLog(arg1, time);
            _dataService.AddWinerInLogEvent -= AddFirstRecord;
            LambdaCommand.CallAllCanExecute();

        }


        #region WinnerLog type List<Winner> -  
        private ObservableCollection<Winner> _WinnerLog = new ObservableCollection<Winner>();
        public ObservableCollection<Winner> WinnerLog { get => _WinnerLog; set => Set(ref _WinnerLog, value); }
        #endregion


        #region ExportFileCommand - описание команды 
        private LambdaCommand? _ExportFileCommand;

        public ICommand ExportFileCommand => _ExportFileCommand ??=
            new LambdaCommand(OnExportFileCommandExecuted, CanExportFileCommandExecute, true);
        private bool CanExportFileCommandExecute(object? p) => WinnerLog.Count > 0;
        private void OnExportFileCommandExecuted(object? p) { }
        #endregion


        private void AddWinerInLog(string name, DateTime time)
        {
            var winner = new Winner() { Name = name, Date = time };
            winner = _dataService.db.Winners.Add(winner).Entity;
            _dataService.db.SaveChanges();
            WinnerLog.Add(winner);
        }
    }
}
