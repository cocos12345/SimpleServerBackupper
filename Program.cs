using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace SimpleServerBackupper
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                #region Configure
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false).Build();        //TODO: 添加数据注解一次性配置校验
                ServiceProvider serviceProvider = Extensions.AddServices(new ServiceCollection(), configuration).BuildServiceProvider();    //TODO: 为保存路径添加时间格式
                Appsettings appsetting = serviceProvider.GetRequiredService<IOptions<Appsettings>>().Value;
                LogFacade.Initialize(serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("ServerBackUp"));
                #endregion


                #region BackUp
                string localBackUpFolder = appsetting.BackUpSetting.LocalBackUpFolder;
                CreateLocalBackUpFolder(localBackUpFolder);

                string targetServerFolder = appsetting.BackUpSetting.TargetServerFolder;
                string tempArchiveFileFullPath = appsetting.BackUpSetting.TempArchiveFileFullPath;

                foreach (var setting in appsetting.Hosts)
                {
                    try
                    {
                        string downloadFileFullPath = $"{localBackUpFolder}\\BackUp_{DateTime.Now:MMdd_HHmm}_{setting.HostName}.tar";

                        LogFacade.LogInformation($"----------  BackUpHost-{setting.HostName} Start  ----------");
                        using (SshClient sshClient = new(setting.HostName, setting.UserName, setting.Password))                 //auto disconnect
                        {
                            sshClient.Connect();

                            SshCommandHelper.CompressFile(sshClient, tempArchiveFileFullPath, targetServerFolder, appsetting.BackUpSetting.ExcludeDirectory);
                            LogFacade.LogInformation("CompressFileSucceed");

                            using var sftpClient = new SftpClient(setting.HostName, setting.UserName, setting.Password);
                            sftpClient.Connect();
                            SshCommandHelper.DownLoadCompressedFile(sftpClient, tempArchiveFileFullPath, downloadFileFullPath);
                            LogFacade.LogInformation($"Download Success | Path={downloadFileFullPath}");      //TODO：complete

                            //Cover, rm is dangerous
                            //SshHandler.RemoveCompressedFile(sshClient, tempArchiveFileFullPath);
                            //LogFacade.LogInformation("RemoveCompressedFileSucceed");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        LogFacade.LogInformation($"SshClient [{setting.HostName}] disconnected");
                        LogFacade.LogInformation($"----------  BackUpHost-{setting.HostName} End  ----------");
                    }
                }
                #endregion

                LogFacade.LogInformation("Backup ALL Success");
            }
            catch (Exception ex)
            {
                LogFacade.LogError($"Unexpected error happend | ex={ex}");
            }
        }

        public static void CreateLocalBackUpFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
