﻿using Forseti.Resources;
using java.lang;
using java.lang.reflect;
using org.mozilla.javascript;

namespace Forseti
{
    public class ScriptEngine : IScriptEngine
    {
        IResourceManager _resourceManager;

        public ScriptEngine(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        Context GetContext()
        {
            var context = Context.enter();
            context.setOptimizationLevel(-1);

            Scriptable scope = context.initStandardObjects();

            Class myJClass = typeof(SystemConsole);
            Member method = myJClass.getMethod("Print", typeof(string));
            Scriptable function = new FunctionObject("print", method, scope);
            scope.put("print", scope, function);

            var envJs = _resourceManager.GetStringFromAssemblyOf<ScriptEngine>("Forseti.JS.env.js");
            context.evaluateString(scope, envJs, "env.js", 1, null);

            return context;
        }


        public void Execute(Harness execution)
        {
            var context = GetContext();
            
        }
    }
}