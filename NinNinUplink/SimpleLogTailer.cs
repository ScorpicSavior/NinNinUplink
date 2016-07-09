/*
 * Copyright (C) 2016 ScorpicSavior
 * 
 * This file is part of NinNinUplink.
 * 
 * NinNinUplink is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * NinNinUplink is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with NinNinUplink.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NinNinUplink
{
    // Credits go to https://github.com/emertechie/CsvLogTailer
    public class SimpleLogTailer
    {
        public static IObservable<string> Tail(string filePath)
        {
            return Observable.Create<string>(observer => {
                var disposable = new CompositeDisposable();
                var cancellationTokenSource = new CancellationTokenSource();

                Task fileWatcherTask = Task.Factory.StartNew(() => {
                    do {
                        try {
                            TailFile(filePath, observer, cancellationTokenSource);
                        }
                        catch (FileNotFoundException) {
                            WaitUntilFileCreated(filePath, cancellationTokenSource);
                        }
                        catch (IOException ioex) {
                            if (ioex.Message.Contains("because it is being used by another process"))
                                Thread.Sleep(250);
                            else
                                throw;
                        }
                        catch (Exception ex) {
                            observer.OnError(ex);
                            throw;
                        }
                    } while (!cancellationTokenSource.IsCancellationRequested);
                }, TaskCreationOptions.LongRunning);

                fileWatcherTask.ContinueWith(
                    t => observer.OnError(new Exception("Error while tailing file", t.Exception)),
                    TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously
                );

                var signalEnd = Disposable.Create(() => {
                    cancellationTokenSource.Cancel();
                    fileWatcherTask.Wait(TimeSpan.FromSeconds(Debugger.IsAttached ? 120 : 2));
                });

                disposable.Add(signalEnd);

                return disposable;
            });
        }

        private static void WaitUntilFileCreated(string filePath, CancellationTokenSource cancellationTokenSource)
        {
            var fileCreated = new ManualResetEventSlim(false);
            cancellationTokenSource.Token.Register(fileCreated.Set);

            while (!fileCreated.Wait(TimeSpan.FromSeconds(1))) {
                if (File.Exists(filePath))
                    fileCreated.Set();
            }
        }

        private static void TailFile(string filePath, IObserver<string> observer, CancellationTokenSource cancellationTokenSource)
        {
            TimeSpan filePollTimeSpan = TimeSpan.FromSeconds(1);
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)) {
                var lastStreamPos = fileStream.Position;

                // Fast-forward to the last line
                if (!cancellationTokenSource.IsCancellationRequested && fileStream.Length != lastStreamPos) {
                    ReadNext(filePath, fileStream, null);
                    if (fileStream.Position != lastStreamPos)
                        lastStreamPos = fileStream.Position;
                }

                do {
                    while (!cancellationTokenSource.IsCancellationRequested && fileStream.Length != lastStreamPos) {
                        ReadNext(filePath, fileStream, observer);
                        if (fileStream.Position == lastStreamPos)
                            break;
                        lastStreamPos = fileStream.Position;
                    }

                    if (!cancellationTokenSource.IsCancellationRequested)
                        Thread.Sleep(filePollTimeSpan);
                } while (!cancellationTokenSource.IsCancellationRequested);
            }
        }

        private static void ReadNext(string filePath, Stream stream, IObserver<string> observer)
        {
            // Reset stream position if file is truncated
            if (stream.Position > stream.Length)
                stream.Position = 0;

            using (var reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (observer != null)
                        observer.OnNext(line);
                }
            }
        }
    }
}
