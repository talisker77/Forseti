﻿using Forseti.Pages;

namespace Forseti.Scripting
{
    public interface IScriptEngine
    {
        void Execute(Page page);
    }
}
