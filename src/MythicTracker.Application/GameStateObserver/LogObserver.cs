﻿using System;
using System.Collections.Generic;
using System.IO;

namespace MythicTracker.Application.GameStateObserver
{
    public class LogObserver : IGameStateObserver
    {
        private readonly string _filepath;
        private FileSystemWatcher _watcher;
        private StreamReader _streamReader;

        public LogObserver(string filepath)
        {
            _filepath = filepath;
            _streamReader = CreateConcurrentReader(filepath);
        }

        public event EventHandler<GameStateChangedEventArgs> Notify;

        public void Finish()
        {
            StopLogChangeWatcher();
        }

        public void Start()
        {
            RunLogChangeWatcher();
        }

        private static StreamReader CreateConcurrentReader(string filepath) =>
           new StreamReader(
               new FileStream(
                   filepath,
                   FileMode.Open,
                   FileAccess.Read,
                   FileShare.ReadWrite));

        private void RunLogChangeWatcher()
        {
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher()
            {
                Path = Path.GetDirectoryName(_filepath),
                Filter = Path.GetFileName(_filepath),
                NotifyFilter = NotifyFilters.LastWrite,
            };

            _watcher.Created += OnChanged;
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        protected virtual void OnChanged(object source, FileSystemEventArgs e)
        {
            RunLogStreamReader();
        }

        private void StopLogChangeWatcher()
        {
            _watcher.Dispose();
            _watcher = null;
        }

        private void RunLogStreamReader()
        {
            string line;
            List<string> linesTemp = new List<string>();
            line = _streamReader.ReadLine();
            while (line != null)
            {
                linesTemp.Add(line);
                Notify?.Invoke(this, new GameStateChangedEventArgs(linesTemp));
                break;
            }
        }
    }
}