namespace FileMerger.Settings
{
    public class CommonSettings
    {
        public string WorkingFolder { get; set; }
        public string CompareTo { get; set; }
        public bool Verbose { get; set; }

        public MergeSettings MergeSettings { get; set; }
    }
}
