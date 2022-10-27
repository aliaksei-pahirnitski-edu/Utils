namespace FileMerger.Settings
{
    public class CommonSettings
    {
        public string WorkingFolder { get; set; }

        /// <summary>
        /// file name of snapshot to compare to when checking if file is existing duplicate or not
        /// </summary>
        public string SnapshotCompareTo { get; set; }
        public bool Verbose { get; set; }

        public MergeSettings MergeSettings { get; set; }
    }
}
