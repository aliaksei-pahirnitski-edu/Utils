namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Generates hash and created sqlite db
    /// </summary>
    public class ScanController
    {
        public void Scan(string folder)
        {
            Console.WriteLine($"start scan [{folder}] ..");
        }
    }
}
