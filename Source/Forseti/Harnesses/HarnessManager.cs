﻿using System;
using System.Collections.Generic;
using System.IO;
using Forseti.Pages;
using Forseti.Scripting;
using Forseti.Suites;

namespace Forseti.Harnesses
{
    public class HarnessManager : IHarnessManager
    {
        IScriptEngine _scriptEngine;
        IPageGenerator _pageGenerator;


        public HarnessManager(IScriptEngine scriptEngine, IPageGenerator pageGenerator)
        {
            _scriptEngine = scriptEngine;
            _pageGenerator = pageGenerator;


            var currentDir = Directory.GetCurrentDirectory();

            FileSystemWatcher w = new FileSystemWatcher(currentDir, "*.js");
            w.IncludeSubdirectories = true;
            w.NotifyFilter = NotifyFilters.LastWrite;
            w.Changed += new FileSystemEventHandler(w_Changed);
            w.EnableRaisingEvents = true;
            
        }

        DateTime _lastTrigger = DateTime.Now;
        IEnumerable<Suite> _currentSuites;

        Dictionary<Suite, DateTime> _lastTriggered = new Dictionary<Suite, DateTime>();

        void w_Changed(object sender, FileSystemEventArgs e)
        {
            var name = Path.GetFileName(e.Name).ToLowerInvariant();
            foreach (var suite in _currentSuites)
            {
                var suiteChanged = false;

                var systemFile = Path.GetFileName(suite.SystemFile).ToLowerInvariant();
                if (systemFile == name)
                    suiteChanged = true;
                else
                {
                    foreach (var description in suite.Descriptions)
                    {
                        var suiteDescriptionFile = Path.GetFileName(description.File).ToLowerInvariant();
                        if (suiteDescriptionFile == name)
                            suiteChanged = true;

                    }
                }

                if (suiteChanged)
                {
                    var delta = _lastTriggered.ContainsKey(suite) ?
                        DateTime.Now.Subtract(_lastTriggered[suite]) :
                        TimeSpan.MaxValue;
                    if( delta.Seconds >= 1 ) 
                        Execute(new[] { suite });

                    _lastTriggered[suite] = DateTime.Now;
                }
            }
        }

        


        public Harness Execute(IEnumerable<Suite> suites)
        {
            if( _currentSuites == null ) 
                _currentSuites = suites;


            var harness = new Harness();
            var cases = new List<Case>();

            var timeBefore = DateTime.Now;
            Console.WriteLine("<--- Run Suite(s) --->");
            foreach (var suite in suites)
            {
                foreach (var description in suite.Descriptions)
                    cases.AddRange(description.Cases);

                harness.Cases = cases;

            }
            var page = _pageGenerator.GenerateFrom(harness);
            _scriptEngine.Execute(page);

            var delta = DateTime.Now.Subtract(timeBefore);

            Console.WriteLine("<--- Took {0} seconds --->\n", delta.TotalSeconds);
            return harness;
        }



    }
}
