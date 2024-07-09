namespace OneWaySynchronizationConsoleApp.Interfaces
{
    public interface IOneWaySyncer
    {
        void StartSync(string sourcePath, string destinationPath, int intervalTime, CancellationToken cancellationToken);

        void CreateFolders(string sourcePath, string destinationPath, CancellationToken cancellationToken);

        void CheckFiles(string sourcePath, string destinationPath, CancellationToken cancellationToken);

        void CopyOrUpdateFile(string sourcePath, string destinationPath, CancellationToken cancellationToken);

        Task<bool> ChecksumMD5AreFilesEquals(string sourcePath, string sourceFilePath, string destinationPath, string destinationFilePath, CancellationToken cancellationToken);

        void CleanDestination(string sourcePath, string destinationPath, CancellationToken cancellationToken);
    }
}
