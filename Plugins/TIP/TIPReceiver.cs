using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;
using UAMP;

namespace ParserLibrary.TIP
{
    /// <summary>
    /// Receiver for TIP files
    /// </summary>
    public class TIPReceiver : Receiver, IDisposable
    {
        private int _delayTime = 1000;

        private string error_dir;
        private IDisposable logContext;
        private string processed_dir;
        private string upload_dir;
        private FileSystemWatcher watcher;
        private string workdir;

        static TIPReceiver()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public TIPReceiver()
        {
            logContext = LogContext.PushProperty("receiver", "TIP");
        }


        public string tracking_dir;
        /// <summary>
        /// Work dir for TIP. 
        /// </summary>
        /// <remarks>
        /// <list type="bullet|number|table">
        /// <listheader>
        /// <term>Directory</term>
        /// <description>description</description>
        /// </listheader>
        /// <item>
        /// <term>upload</term>
        /// <description> TIP watching this directory and load new files to system</description>
        /// </item>
        /// <item>
        /// <term>processed</term>
        /// <description> Dir for processed files</description>
        /// </item>
        /// <item>
        /// <term>error</term>
        /// <description> Folder for files and records that have errors</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// 
        public string WorkDir
        {
            get => workdir;
            set
            {
                if (value != null)
                {

                    workdir = value;
                    if (!Directory.Exists(value))
                    {
                        throw new DirectoryNotFoundException($"Directory {value} not found!");
                    }

                    upload_dir = Path.Combine(value, "upload");
                    processed_dir = Path.Combine(value, "processed");
                    error_dir = Path.Combine(value, "error");
                }
            }
        }

        /// <summary>
        /// Delay before parse in seconds
        /// </summary>
        public int DelayTime
        {
            get => _delayTime / 1000;
            set => _delayTime = value * 1000;
        }

        public void Dispose()
        {
            logContext.Dispose();
        }

        /// <summary>
        /// Start watcher
        /// </summary>
        protected override async Task startInternal()
        {
            WorkDir = tracking_dir;
            Directory.CreateDirectory(upload_dir);
            Log.Information("Directory for upload: {dir}", upload_dir);
            Directory.CreateDirectory(processed_dir);
            Log.Information("Directory for processed: {dir}", processed_dir);
            Directory.CreateDirectory(error_dir);
            Log.Information("Directory for error: {dir}", error_dir);
            watcher = new FileSystemWatcher(upload_dir);
            watcher.IncludeSubdirectories = false;
            // watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Created += WatcherOnCreated;
            watcher.EnableRaisingEvents = true;
            Log.Information("Start watching: {dir}", upload_dir);
            foreach(var file in Directory.GetFiles(upload_dir))
            {
                Log.Information($"Found file {file}");
                await ParseTIPfile(Path.GetFileName(file));
                Log.Information($"File processed {file}");

            }
        }

        private async void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            using (LogContext.PushProperty("FileName", e.Name))
            {
                Log.Debug("Rised create event");
                await Task.Delay(_delayTime);
                if (ValidateFile(e.FullPath))
                {
                    Log.Information("Find file");
                    await ParseTIPfile(e.Name);
                    Log.Information("File processed");
                }
            }
        }

        private static bool ValidateFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return false;
            }

            return (File.GetAttributes(fullPath) &
                    (FileAttributes.Device | FileAttributes.Directory | FileAttributes.Hidden |
                     FileAttributes.System | FileAttributes.Temporary)) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Send filename as context in signal call</remarks>
        /// <param name="fileName"></param>
        private async Task ParseTIPfile(string fileName)
        {
            string newFileName = $"{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}_{fileName}";
            string upload_file = Path.Combine(upload_dir, fileName);
            string error_file = Path.Combine(error_dir, newFileName);
            string processed_file = Path.Combine(processed_dir, newFileName);

            try
            {
                using (FileStream fileStream = File.OpenRead(upload_file))
                {
                    Log.Debug("Open file at {path}", upload_file);
                    List<byte> buffer = new();
                    int b;

                    #region Parse header

                    do
                    {
                        b = fileStream.ReadByte();
                        if (b == -1)
                        {
                            fileStream.Close();
                            Log.Error("Not TIP message");
                            File.Move(upload_file,
                                error_file);
                            return;
                        }

                        if (b == (int) Symbols.MS)
                        {
                            break;
                        }

                        buffer.Add((byte) b);
                    } while (true);

                    UAMPMessage header;
                    Encoding encoding;

                    try
                    {
                        header = new UAMPMessage(Encoding.UTF8.GetString(buffer.ToArray()));
                        encoding = (header["Encoding"] as UAMPScalar)!.Value.ToLower() switch
                        {
                            "ansi" => Encoding.Default,
                            "cp866" => Encoding.GetEncoding(866),
                            "cp1251" => Encoding.GetEncoding(1251),
                            "utf8" => Encoding.UTF8,
                            _ => throw new NotSupportedException($"Encoding: {header["Encoding"]}")
                        };
                    }
                    catch (Exception e)
                    {
                        fileStream.Close();
                        Log.Error(e, "Error in file");
                        File.Move(upload_file,
                            error_file);
                        return;
                    }

                    #endregion Parse header


                    #region Parse messages

                    buffer.Clear();
                    int recieved_rows = 0;
                    int send_rows = 0;
                    int error_rows = 0;

                    do
                    {
                        b = fileStream.ReadByte();
                        if (b == -1)
                        {
                            fileStream.Close();
                            File.Move(upload_file,
                                processed_file);
                            Log.Information("Rows recieved: {recieved_rows}, send: {send_rows}, error: {error_rows}",
                                recieved_rows, send_rows, error_rows);
                            break;
                        }

                        if (b == (int) Symbols.MS)
                        {
                            try
                            {
                                recieved_rows++;
                                var send = new UAMPMessage();
                                UAMPMessage message = new UAMPMessage(encoding.GetString(buffer.ToArray()));
                                send["header"] = header;
                                send["message"] = message;
                                await signal(JsonSerializer.Serialize(send), fileName);
                                send_rows++;
                                buffer.Clear();
                                continue;
                            }
                            catch (Exception e)
                            {
                                error_rows++;
                                Log.Error(e, "Error in file message: {MessageNum}",
                                    recieved_rows);
                                using (var errorfile =
                                    new FileStream(error_file,
                                        FileMode.Append))
                                {
                                    errorfile.WriteByte((byte) Symbols.MS);
                                    errorfile.Write(buffer.ToArray());
                                    buffer.Clear();
                                }
                            }
                        }

                        buffer.Add((byte) b);
                    } while (true);
                }

                #endregion
            }
            catch (Exception e)
            {
                Log.Error(e, "Not available");
                File.Move(upload_file,
                    error_file);
            }
        }
    }
}