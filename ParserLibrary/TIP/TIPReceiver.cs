using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using UAMP;

namespace ParserLibrary.TIP
{
    public class TIPReceiver : Receiver
    {
        /// <summary>
        /// Delay before parse in seconds
        /// </summary>
        private int _delayTime = 1000;

        private string error_dir;
        private string processed_dir;
        private string upload_dir;
        private FileSystemWatcher watcher;
        private string workdir;

        public string WorkDir
        {
            get => workdir;
            set
            {
                workdir = value;
                upload_dir = Path.Combine(value, "upload");
                processed_dir = Path.Combine(value, "processed");
                error_dir = Path.Combine(value, "error");
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

        public override async Task start()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Directory.CreateDirectory(upload_dir);
            Log.Information("Directory for upload: {dir}", upload_dir);
            Directory.CreateDirectory(processed_dir);
            Log.Information("Directory for processed: {dir}", upload_dir);
            Directory.CreateDirectory(error_dir);
            Log.Information("Directory for error: {dir}", upload_dir);
            watcher = new FileSystemWatcher(upload_dir);
            watcher.Created += WatcherOnCreated;
            watcher.EnableRaisingEvents = true;
            Log.Information("Start watching: {dir}", upload_dir);
        }

        private async void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            FileAttributes attributes = File.GetAttributes(e.FullPath);
            if (attributes.HasFlag(FileAttributes.Normal))
            {
                Log.Information("Find file {FileName}.", e.Name);
                await ParseTIPfile(e.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Send filename as context in signal call</remarks>
        /// <param name="fileName"></param>
        private async Task ParseTIPfile(string fileName)
        {
            string upload_file = Path.Combine(upload_dir, fileName);

            string error_file = Path.Combine(error_dir, fileName + '_' + DateTime.UtcNow.ToString("O"));
            try
            {
                await Task.Delay(_delayTime);
                using (FileStream fileStream = File.OpenRead(upload_file))
                {
                    List<byte> buffer = new();
                    int b;
                    // Parse header
                    do
                    {
                        b = fileStream.ReadByte();
                        if (b == -1)
                        {
                            fileStream.Close();
                            Log.Error("Not TIP message: {FileName}", fileName);
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
                            "utf8" => Encoding.UTF8
                        };
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "File: {FileName}", fileName);
                        File.Move(upload_file,
                            error_file);
                        return;
                    }

                    // Parse messages
                    buffer.Clear();
                    int i = 0;
                    do
                    {
                        b = fileStream.ReadByte();
                        if (b == -1)
                        {
                            File.Move(upload_file,
                                Path.Combine(processed_dir, fileName + '_' + DateTime.UtcNow.ToString("O")));
                            break;
                        }

                        if (b == (int) Symbols.MS)
                        {
                            try
                            {
                                i++;
                                UAMPMessage message = new UAMPMessage(encoding.GetString(buffer.ToArray()));
                                message["header"] = header;
                                await signal(JsonSerializer.Serialize(message), fileName);
                                buffer.Clear();
                                continue;
                            }
                            catch (Exception e)
                            {
                                Log.Error(e, "Error in file: {FileName}, message {MessageNum}",
                                    upload_file, i);
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

                Log.Information("File processed: {FileName}", fileName);
            }
            catch (Exception e)
            {
                Log.Error("Not available: {FileName}", fileName);
                File.Move(upload_file,
                    error_file);
            }
        }

        private async Task<bool> IsFileAvailable(string fileName)
        {
            foreach (int time in new[] {300, 90000, 600000})
            {
                try
                {
                    FileStream fileStream = File.Open(Path.Combine(upload_dir, fileName), FileMode.Open,
                        FileAccess.ReadWrite, FileShare.None);

                    fileStream.Close();
                    return true;
                }
                catch (IOException e)
                {
                    Log.Warning("Wait {time} ms. Not available: {FileName}", time, fileName);
                    await Task.Delay(time);
                }
            }

            return false;
        }
    }
}