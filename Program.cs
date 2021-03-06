﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCore.Data;
using EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq.Expressions;

namespace EFCore
{
    public class Program
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });

        public static async Task Main(string[] args)
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Cat, CatViewModel>();
            });

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            var host = new HostBuilder()
                .UseConsoleLifetime()
                .UseSerilog()
                .ConfigureServices(services => {
                    services
                        .AddDbContext<MyAppContext>(options => {
                          //  options.UseLoggerFactory(MyLoggerFactory);
                            options.UseSqlite(Constants.ConnectionString, sqliteOptions => {
                                sqliteOptions.MigrationsAssembly(typeof(MyAppContext).GetTypeInfo().Assembly.GetName().Name);
                            });
                        });
                })
                .Build();

            using (host)
            {
                await host.StartAsync();

                var _logger = host.Services.GetService<ILogger<Program>>();

                using (var context = host.Services.GetService<MyAppContext>())
                {

                    // Delete the SQLite database
                    context.Database.EnsureDeleted();

                    // Create, migrate and seed the SQLite database
                    context.Database.Migrate();

                    var results = context.Cat
                       .OrderBy(x => x.MeowLoudness)
                       .ThenBy(x => x.MeowLoudness)

                       .Select(x => new CatViewModel()
                       {
                           Id = x.Id,
                           Name = x.Name,
                           Breed = new CatBreedViewModel() {
                               Id = x.Breed.Id,
                               BreedName = x.Breed.BreedName,
                               Cats = x.Breed.Cats.Select(y => new CatViewModel()
                               {
                                   Id = y.Id,
                                   MeowLoudness = y.MeowLoudness,
                                   Name = y.Name,
                                   TailLength = y.TailLength
                               }).ToList()
                           },
                           MeowLoudness = x.MeowLoudness,
                           TailLength = x.TailLength
                       })
                       .ToList();

                    Console.WriteLine(results.Count);

                }

                Console.ReadKey();

                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

        }


    }
}
