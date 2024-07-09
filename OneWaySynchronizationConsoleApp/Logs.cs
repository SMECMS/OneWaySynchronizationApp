using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace OneWaySynchronizationConsoleApp
{
    public static partial class Logs
    {
        [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Exiting Application")]
        public static partial void ExitApplicationMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Logging Started, LogFile's Path: '{pathLogFile}'")]
        public static partial void LogStartupMessage(this ILogger logger, string pathLogFile);

        [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Critical,
        Message = "The Source Path is not valid")]
        public static partial void NotValidSourcePathMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Critical,
        Message = "The Source Path is not valid")]
        public static partial void NotValidDestinationPathMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Critical,
        Message = "You must pass all the 4 appropriate arguments")]
        public static partial void NotValidIntervalTimeMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "Sync started from: '{SourcePath}' to '{DestinationPath}' every '{IntervalTime}' seconds")]
        public static partial void StartSyncMessage(this ILogger logger, string SourcePath, string DestinationPath, int IntervalTime);

        [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Information,
        Message = "--- Press C to Exit the Application ---")]
        public static partial void PressCToExitMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Information,
        Message = "C Key Pressed")]
        public static partial void CKeyPressedMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 8,
        Level = LogLevel.Information,
        Message = "New Sync cycle Started")]
        public static partial void SyncStartCycleMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 9,
        Level = LogLevel.Debug,
        Message = "Create Folders Start")]
        public static partial void CreateFoldersStartDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 10,
        Level = LogLevel.Information,
        Message = "New Folder from Source Create at Destination Path: '{pathNewFolder}'")]
        public static partial void NewFolderCreatedMessage(this ILogger logger, string pathNewFolder);

        [LoggerMessage(
        EventId = 11,
        Level = LogLevel.Debug,
        Message = "Create Folders End")]
        public static partial void CreateFoldersEndDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 12,
        Level = LogLevel.Debug,
        Message = "Check Files Start")]
        public static partial void CheckFilesStartDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 13,
        Level = LogLevel.Information,
        Message = "New File from Source Copied to Destination Path: '{pathNewFile}'")]
        public static partial void NewFileCopiedMessage(this ILogger logger, string pathNewFile);

        [LoggerMessage(
        EventId = 14,
        Level = LogLevel.Information,
        Message = "A File was Updated from Source to Destination Path: '{pathUpdatedFile}'")]
        public static partial void FileUpdatedMessage(this ILogger logger, string pathUpdatedFile);

        [LoggerMessage(
        EventId = 15,
        Level = LogLevel.Debug,
        Message = "Check Files End")]
        public static partial void CheckFilesEndDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 16,
        Level = LogLevel.Debug,
        Message = "Copy Or Update File Started with Source Path: '{sourceFilePath}' to Destination Path: '{destinationFilePath}'")]
        public static partial void CopyOrUpdateFileStartDebug(this ILogger logger, string sourceFilePath, string destinationFilePath);

        [LoggerMessage(
        EventId = 17,
        Level = LogLevel.Debug,
        Message = "Copy Or Update File Ended with Source Path: '{sourceFilePath}' to Destination Path: '{destinationFilePath}'")]
        public static partial void CopyOrUpdateFileEndDebug(this ILogger logger, string sourceFilePath, string destinationFilePath);

        [LoggerMessage(
        EventId = 18,
        Level = LogLevel.Debug,
        Message = "CheckSum MD5 Compare Started with Source Path: '{sourceFilePath}' and Destination Path: '{destinationFilePath}'")]
        public static partial void ChecksumMD5AreFilesEqualsStartDebug(this ILogger logger, string sourceFilePath, string destinationFilePath);

        [LoggerMessage(
        EventId = 19,
        Level = LogLevel.Debug,
        Message = "CheckSum MD5 Compare Ended with Source Path: '{sourceFilePath}' and MD5 Hash: '{sourceHash}', and Destination Path: '{destinationFilePath}' and MD5 Hash: '{destinationHash}'")]
        public static partial void ChecksumMD5AreFilesEqualsEndDebug(this ILogger logger, string sourceFilePath, string destinationFilePath, string sourceHash, string destinationHash);

        [LoggerMessage(
        EventId = 20,
        Level = LogLevel.Debug,
        Message = "Clean Destination Start")]
        public static partial void CleanDestinationStartDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 21,
        Level = LogLevel.Information,
        Message = "A File was Deleted from Destination not present in Source at Path: '{pathUpdatedFile}'")]
        public static partial void DestinationFileDeletedMessage(this ILogger logger, string pathUpdatedFile);

        [LoggerMessage(
        EventId = 22,
        Level = LogLevel.Information,
        Message = "A Folder was Deleted from Destination not present in Source at Path: '{pathUpdatedFile}'")]
        public static partial void DestinationFolderDeletedMessage(this ILogger logger, string pathUpdatedFile);

        [LoggerMessage(
        EventId = 23,
        Level = LogLevel.Debug,
        Message = "Clean Destination End")]
        public static partial void CleanDestinationEndDebug(this ILogger logger);

        [LoggerMessage(
        EventId = 24,
        Level = LogLevel.Information,
        Message = "Sync cycle ended, going to sleep till next cycle")]
        public static partial void SyncCycleEndMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 25,
        Level = LogLevel.Information,
        Message = "Creating New Folders From Source at Destination")]
        public static partial void CreateFoldersStartMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 26,
        Level = LogLevel.Information,
        Message = "Checking Source Files with Destination")]
        public static partial void CheckFilesStartMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 27,
        Level = LogLevel.Information,
        Message = "Cleaning Destination Folders & Files not present in Source")]
        public static partial void CleanDestinationStartMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 28,
        Level = LogLevel.Information,
        Message = "The Source Path and Destination Path must be different")]
        public static partial void NotValidSameAddressMessage(this ILogger logger);

        [LoggerMessage(
        EventId = 29,
        Level = LogLevel.Information,
        Message = "The Log Path must be different and outside both the Source Path and Destination Path")]
        public static partial void NotValidLogMustBeUniqueMessage(this ILogger logger);
    }
}
