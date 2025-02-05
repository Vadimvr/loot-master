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
        private readonly IExportData _exportData;
        #region ViewName type string -  
        private string _ViewName = "Лог";
        public string ViewName { get => _ViewName; set => Set(ref _ViewName, value); }
        #endregion

        #region WinnerLog type List<Winner> -  
        private ObservableCollection<Winner> _WinnerLog = new ObservableCollection<Winner>();
        public ObservableCollection<Winner> WinnerLog { get => _WinnerLog; set => Set(ref _WinnerLog, value); }
        #endregion
        public LogWinnerViewModel(IDataService dataService, IExportData exportData)
        {
            _dataService = dataService;
            _exportData = exportData;
            if (dataService != null)
            {
                dataService.AddWinerInLogEvent += AddFirstRecord;
            }
            WinnerLog = new ObservableCollection<Winner>(_dataService.db.Winners.ToList().TakeLast(50));
        }

        private void AddFirstRecord(string arg1, DateTime time)
        {
            _dataService.AddWinerInLogEvent += AddWinerInLog;
            AddWinerInLog(arg1, time);
            _dataService.AddWinerInLogEvent -= AddFirstRecord;
            LambdaCommand.CallAllCanExecute();
        }
        private void AddWinerInLog(string name, DateTime time) => WinnerLog.Add(_dataService.AddWinnerInLog(new Winner() { Name = name, Date = time }));

        #region ExportFileCommand - описание команды 
        private LambdaCommand? _ExportFileCommand;
        public ICommand ExportFileCommand => _ExportFileCommand ??=
            new LambdaCommand(OnExportFileCommandExecuted, CanExportFileCommandExecute, true);
        private bool CanExportFileCommandExecute(object? p) => WinnerLog.Count > 0;
        private void OnExportFileCommandExecuted(object? p)
        {
            _exportData.Export(_dataService.db.Winners.ToList());
        }
        #endregion

        #region ClearLogFileCommand - описание команды 
        private LambdaCommand? _ClearLogFileCommand;
        public ICommand ClearLogFileCommand => _ClearLogFileCommand ??=
            new LambdaCommand(OnClearLogFileCommandExecuted, CanClearLogFileCommandExecute);
        private bool CanClearLogFileCommandExecute(object? p) => true;
        private void OnClearLogFileCommandExecuted(object? p)
        {
            WinnerLog.Clear();
            _dataService.RemoveLog();
        }
        #endregion
    }
}
