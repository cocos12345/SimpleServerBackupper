using Renci.SshNet;                         // a namespace in ssh.net
using Renci.SshNet.Common;
using System;

namespace SimpleServerBackupper
{
    public static class SshCommandHelper
    {
        public static void CompressFile(SshClient sshClient, string tempArchiveFilePath, string targetFolder, List<string> excludeDirectories)
        {
            string command = $"tar -cvf {tempArchiveFilePath} -C {targetFolder} .";
            foreach (string dir in excludeDirectories)
            {
                command += $" --exclude='{dir}'";
            }
            ExecuteCommand(sshClient, command);
        }

        public static void DownLoadCompressedFile(SftpClient sftpClient, string compressFilePath, string downloadLocalPath)
        {
            using var fStream = File.Create(downloadLocalPath);
            sftpClient.DownloadFile(compressFilePath, fStream);
        }

        public static void RemoveCompressedFile(SshClient sshClient, string tempArchiveFilePath)
        {
            string command = $"rm {tempArchiveFilePath}";
            ExecuteCommand(sshClient, command);
        }


        private static void ExecuteCommand(SshClient sshClient, string command)
        {
            SshCommand commandWithResult = sshClient.RunCommand(command);
            if (!string.IsNullOrEmpty(commandWithResult.Error))
            {
                throw new SshException(commandWithResult.Error);
            }
            LogFacade.LogInformation($"Command Execute Result | Result={commandWithResult.Result}");
        }
    }
}
