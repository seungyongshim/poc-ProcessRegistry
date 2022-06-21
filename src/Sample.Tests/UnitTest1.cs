using System;
using System.Threading.Tasks;
using Boost.Proto.Actor.DependencyInjection;
using Boost.Proto.Actor.Hosting.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proto;
using Xunit;
using static Sample.Prelude;

namespace Sample.Tests;

public class PreludeSpec
{
    [Fact]
    public async Task AddSuccessAsync()
    {
        var host = Host.CreateDefaultBuilder()
                       .UseProtoActor((sp, o) =>
                       {
                           o.FuncActorSystemStart = root =>
                           {

                               root.SpawnNamed(sp.GetRequiredService<IPropsFactory<HelloActor>>().Create(), "Hello");

                               return root;
                           };
                       }).Build();

        await host.StartAsync();

        var root = host.Services.GetRequiredService<IRootContext>();

        var ret = root.System.ProcessRegistry.Get(new("nonhost","Hello1"));

        await host.StopAsync();
    }
}


public record HelloActor : IActor
{
    public Task ReceiveAsync(IContext context) =>
        Task.CompletedTask;
}
