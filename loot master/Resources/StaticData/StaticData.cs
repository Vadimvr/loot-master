namespace loot_master.Resources.StaticData
{
    internal static class StaticData
    {
        public readonly static string ProgramName = "Loot master";

        public readonly static string connectionString = "Data Source=usersdata.db";

        public static int Gp { get; internal set; } = 500;
        public static int Ep { get; internal set; } = 5000;
    }
}
