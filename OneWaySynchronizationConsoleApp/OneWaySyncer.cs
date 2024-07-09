using OneWaySynchronizationConsoleApp.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace OneWaySynchronizationConsoleApp
{
    public class OneWaySyncer : IOneWaySyncer
    {
        ILogger _logger;
        public OneWaySyncer(ILogger logger)
        {
            _logger = logger;
        }

        public async void StartSync(string sourcePath, string destinationPath, int intervalTime, CancellationToken cancellationToken)
        {
            _logger.StartSyncMessage(sourcePath, destinationPath, intervalTime);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.SyncStartCycleMessage();

                    _logger.CreateFoldersStartMessage();
                    CreateFolders(sourcePath: sourcePath,destinationPath: destinationPath, cancellationToken: cancellationToken);

                    _logger.CheckFilesStartMessage();
                    CheckFiles(sourcePath: sourcePath, destinationPath: destinationPath, cancellationToken: cancellationToken);

                    _logger.CleanDestinationStartMessage();
                    CleanDestination(sourcePath: sourcePath, destinationPath: destinationPath, cancellationToken: cancellationToken);

                    _logger.SyncCycleEndMessage();
                    for (int i = 0; i < intervalTime; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Thread.Sleep(1000);
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.ExitApplicationMessage();
                    Environment.Exit(0);
                }
            }
            _logger.ExitApplicationMessage();
            Environment.Exit(0);
        }

        public void CreateFolders(string sourcePath, string destinationPath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.CreateFoldersStartDebug();

                //get all source folders
                List<string> sourceFolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).OrderBy(x => x).ToList();

                //if !exist in destination CreateFolder 
                for (int i = 0; i < sourceFolders.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string filePath = sourceFolders[i].Substring(sourcePath.Length);
                    if (!Directory.Exists(destinationPath + filePath))
                    {
                        Directory.CreateDirectory(destinationPath + filePath);
                        _logger.NewFolderCreatedMessage(destinationPath + filePath);
                    }
                }
                cancellationToken.ThrowIfCancellationRequested();

                _logger.CreateFoldersEndDebug();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public void CheckFiles(string sourcePath, string destinationPath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.CheckFilesStartDebug();

                // get all source files
                List<string> sourceFiles = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).OrderBy(x => x).ToList();


                //if !exist in destination CreateFile / else checksumMD5 Update if different
                for (int i = 0; i < sourceFiles.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string filePath = sourceFiles[i].Substring(sourcePath.Length);
                    if (!File.Exists(destinationPath + filePath))
                    {
                        CopyOrUpdateFile(sourceFilePath: sourceFiles[i], destinationFilePath: destinationPath + filePath, cancellationToken: cancellationToken);
                        _logger.NewFileCopiedMessage(destinationPath + filePath);
                    }
                    else
                    {
                        bool result = ChecksumMD5AreFilesEquals(sourcePath: sourcePath, sourceFilePath: sourceFiles[i], destinationPath: destinationPath, destinationFilePath: destinationPath + filePath, cancellationToken: cancellationToken).Result;
                        if (!result)
                        {
                            CopyOrUpdateFile(sourceFilePath: sourceFiles[i], destinationFilePath: destinationPath + filePath, cancellationToken: cancellationToken);
                            _logger.FileUpdatedMessage(destinationPath + filePath);
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }

                _logger.CheckFilesEndDebug();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public void CopyOrUpdateFile(string sourceFilePath, string destinationFilePath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.CopyOrUpdateFileStartDebug(sourceFilePath, destinationFilePath);

                cancellationToken.ThrowIfCancellationRequested();
                using (FileStream writeStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    int bufferSize = 1024 * 1024;
                    FileStream readStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    writeStream.SetLength(readStream.Length);
                    int bytesRead = -1;
                    byte[] bytes = new byte[bufferSize];

                    while ((bytesRead = readStream.Read(bytes, 0, bufferSize)) > 0)
                    {
                        writeStream.Write(bytes, 0, bytesRead);
                    }
                    writeStream.Close();
                    readStream.Close();
                }

                _logger.CopyOrUpdateFileEndDebug(sourceFilePath, destinationFilePath);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<bool> ChecksumMD5AreFilesEquals(string sourcePath, string sourceFilePath, string destinationPath, string destinationFilePath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.ChecksumMD5AreFilesEqualsStartDebug(sourceFilePath, destinationFilePath);

                var md5 = MD5.Create();
                using (var cs = new CryptoStream(Stream.Null, md5, CryptoStreamMode.Write))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var file = new FileInfo(sourceFilePath);

                    // hash file path
                    string filePath = sourceFilePath.Substring(sourcePath.Length);
                    byte[] pathBytes = Encoding.UTF8.GetBytes(filePath.ToLower());
                    cs.Write(pathBytes, 0, pathBytes.Length);

                    //hash file binary
                    using (var fs = file.OpenRead())
                        await fs.CopyToAsync(cs, cancellationToken);


                    cancellationToken.ThrowIfCancellationRequested();

                    cs.FlushFinalBlock();
                }
                var sourceHash = md5.Hash;

                md5 = MD5.Create();
                using (var cs = new CryptoStream(Stream.Null, md5, CryptoStreamMode.Write))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var file = new FileInfo(destinationFilePath);

                    // hash file path
                    string filePath = destinationFilePath.Substring(destinationPath.Length);
                    byte[] pathBytes = Encoding.UTF8.GetBytes(filePath.ToLower());
                    cs.Write(pathBytes, 0, pathBytes.Length);

                    //hash file binary
                    using (var fs = file.OpenRead())
                        await fs.CopyToAsync(cs, cancellationToken);


                    cancellationToken.ThrowIfCancellationRequested();

                    cs.FlushFinalBlock();
                }

                if (sourceHash == null || md5.Hash == null)
                    return false;

                _logger.ChecksumMD5AreFilesEqualsEndDebug(sourceFilePath, destinationFilePath, BitConverter.ToString(sourceHash).Replace("-", "").ToLower(), BitConverter.ToString(md5.Hash).Replace("-", "").ToLower());
                return BitConverter.ToString(sourceHash).Replace("-", "").ToLower() == BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public void CleanDestination(string sourcePath, string destinationPath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.CleanDestinationStartDebug();

                cancellationToken.ThrowIfCancellationRequested();

                //Get all destination files
                List<string> destinationFiles = Directory.GetFiles(destinationPath, "*", SearchOption.AllDirectories).OrderBy(x => x).ToList();

                //if !exists in source then Delete
                for (int i = 0; i < destinationFiles.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string filePath = destinationFiles[i].Substring(destinationPath.Length);
                    bool x = File.Exists(sourcePath + filePath);
                    if (!File.Exists(sourcePath + filePath))
                    {
                        File.Delete(destinationPath + filePath);
                        _logger.DestinationFileDeletedMessage(destinationPath + filePath);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }

                cancellationToken.ThrowIfCancellationRequested();

                //Get all destination folders
                List<string> destinationFolders = Directory.GetDirectories(destinationPath, "*", SearchOption.AllDirectories).OrderByDescending(o => o).ToList();

                //if !exists in source then Delete
                for (int i = 0; i < destinationFolders.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string filePath = destinationFolders[i].Substring(destinationPath.Length);
                    if (!Directory.Exists(sourcePath + filePath))
                    {
                        Directory.Delete(destinationPath + filePath);
                        _logger.DestinationFolderDeletedMessage(destinationPath + filePath);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                }

                _logger.CleanDestinationEndDebug();

            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }
    }
}