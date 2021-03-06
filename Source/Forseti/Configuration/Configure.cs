﻿using Forseti.Harnesses;
using Forseti.Registries;
using Forseti.Scripting;
using StructureMap;
using Microsoft.Practices.ServiceLocation;
using StructureMap.ServiceLocatorAdapter;
using System;

namespace Forseti.Configuration
{
    public class Configure : IConfigure
    {
        public IHarnessManager HarnessManager { get; private set; }
        public IContainer Container { get; private set; }
        public IScriptEngine ScriptEngine { get; private set; }


        Configure(IContainer container)
        {
            Container = container;
        }

        public static IConfigure WithStandard()
        {
            var container = GetContainer();
            return With(container);
        }

        
        public static IConfigure With(IContainer container)
        {
            var instance = new Configure(container);
			container.Configure(c=>c.For<IConfigure>().Use (instance));
            return instance;
        }

        static IContainer GetContainer()
        {
            var container = new Container(new MainRegistry());
            ServiceLocator.SetLocatorProvider(()=>new StructureMapServiceLocator(container));
            return container;
        }



        public IConfigure Initialize()
        {
            HarnessManager = Container.GetInstance<IHarnessManager>();
            ScriptEngine = Container.GetInstance<IScriptEngine>();

            return this;
        }


        public T GetInstanceOf<T>()
        {
            return Container.GetInstance<T>();
        }
		
		public object GetInstanceOf(Type type)
		{
			return Container.GetInstance(type);
		}
    }
}
