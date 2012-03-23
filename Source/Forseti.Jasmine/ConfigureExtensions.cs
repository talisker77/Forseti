﻿namespace Forseti.Configuration
{
    public static class ConfigureExtensions
    {
        public static IConfigure UsingJasmine(this IConfigure configure)
        {
            configure.Container.Configure(
                c => 
                    c   .For<IFramework>()
                        .Singleton()
                        .Use<Jasmine.Framework>());
            return configure;
        }
    }
}
