using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using WebAPI.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace _2024TDD04.DAL.Tests.WebAPI
{
    public class AccessesControllerTest
    {
        private IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        private Lazy<IServiceProvider> LazyServiceProvider { get; }
        private IServiceProvider ServiceProvider => LazyServiceProvider.Value;
        private AccessesController Controller => ServiceProvider.GetRequiredService<AccessesController>();
        private RoomAccessContext Context => ServiceProvider.GetRequiredService<RoomAccessContext>();

        public AccessesControllerTest()
        {
            // Configure AutoMapper
            ServiceCollection.AddAutoMapper(cfg =>
            {
            }, typeof(MappingsProfile).Assembly);

            // Configure InMemory DbContext
            ServiceCollection.AddDbContext<IApplicationContext, ApplicationContext>(builder =>
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            // Configure Controller
            ServiceCollection.AddSingleton(provider =>
            {
                var controller = new PlatesController(provider.GetService<IApplicationContext>(), provider.GetService<IMapper>(), NullLogger<PlatesController>.Instance)
                {
                    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { } }
                };
                return controller;
            });

            // Lazy ServiceProvider
            LazyServiceProvider = new Lazy<IServiceProvider>(() => ServiceCollection.BuildServiceProvider(), LazyThreadSafetyMode.ExecutionAndPublication);
        }
    }
}