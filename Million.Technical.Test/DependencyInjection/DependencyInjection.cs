﻿namespace Million.Technical.Test.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void Inject(this WebApplicationBuilder builder)
        {
            BaseInjections.InjectBase(builder);
            CommandsInjections.InjectCommands(builder);
            QueriesInjections.InjectQueries(builder);
        }
    }
}