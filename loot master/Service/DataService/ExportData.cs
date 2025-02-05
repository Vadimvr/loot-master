using loot_master.Models;
using SwiftExcel;

namespace loot_master.Service.Data
{
    interface IExportData
    {
        void Export(IEnumerable<Winner> winners);
    }
    class ExportToTxt : IExportData
    {
        public void Export(IEnumerable<Winner> winners)
        {
            Task.Run(async () =>
            {
                string fn = "Log.txt";
                string file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllLines(file, winners.Select(x => string.Format("{0,-5}{1,-20}  {2}", x.Id, x.Name, x.Date)));

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share text file",
                    File = new ShareFile(file)
                });
            });
        }
    }
    class ExportToExcel : IExportData
    {
        public void Export(IEnumerable<Winner> winners)
        {
            Task.Run(async () =>
            {
                string fn = "Log.xlsx";
                string file = Path.Combine(FileSystem.CacheDirectory, fn);
                using (var ew = new ExcelWriter(file))
                {
                    int row = 1;
                    foreach (Winner winner in winners)
                    {
                        ew.Write(winner.Id.ToString(), 1, row);
                        ew.Write(winner.Name, 2, row);
                        ew.Write(winner.Date.ToString("f"), 3, row);
                        row++;
                    }
                }
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share xlsx file",
                    File = new ShareFile(file)
                });
            });
        }
    }
}
