﻿// ———————————————————————–
// <copyright company="Shane Carvalho">
//      Dynamics CRM Online Management API Client
//      Copyright(C) 2017  Shane Carvalho

//      This program is free software: you can redistribute it and/or modify
//      it under the terms of the GNU General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.

//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//      GNU General Public License for more details.

//      You should have received a copy of the GNU General Public License
//      along with this program.If not, see<http://www.gnu.org/licenses/>.
// </copyright>
// ———————————————————————–

using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Serilog;
using Serilog.Formatting.Json;
using Model = OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient
{
    public class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.Read();
            if(!System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("debugger not attached");
                System.Diagnostics.Debugger.Launch();
            }
            else
            {
                Console.WriteLine("debugger already attached.");
            }
#endif
            

            // initialize the logger using the app config
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole(outputTemplate:"{Timestamp:yyy-MM-dd HH:mm:ss} {Message}{NewLine}")
                .ReadFrom.AppSettings()
                .CreateLogger();

            var operations = new Program();

            // initialize the commandline parser
            var customizedParser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = System.Console.Error;
            });

            var result = customizedParser.ParseArguments<
                GetInstancesOptions,
                CreateInstanceOptions,
                DeleteInstanceOptions,
                GetOperationStatusOptions,
                GetServiceVersions>(args);

            Log.Debug("Input parameters: {@result}", result);

            try
            {
                // Map the command line parameters to the different options 
                result.MapResult(
                    (GetInstancesOptions opts) => operations.Process(opts),
                    (CreateInstanceOptions opts) => operations.Process(opts),
                    (DeleteInstanceOptions opts) => operations.Process(opts),
                    (GetOperationStatusOptions opts) => operations.Process(opts),
                    (GetServiceVersions opts) => operations.Process(opts),
                    errors => 1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error");
            }
        }

        /// <summary>
        /// Processes the Get instances operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetInstancesOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                var instances = service.GetInstances().Result;
                var instancesCount = instances?.Count();

                if (!string.IsNullOrEmpty(opts.FriendlyName))
                {
                    instances = instances
                        .Where(x => x.FriendlyName.Equals(opts.FriendlyName, StringComparison.InvariantCultureIgnoreCase));
                }
                else if (!string.IsNullOrEmpty(opts.UniqueName))
                {
                    instances = instances.Where(x => x.UniqueName.Equals(opts.UniqueName, StringComparison.InvariantCultureIgnoreCase));
                }

                Log.Information("{@instancesCount} instances found.", instancesCount);

                foreach (var i in instances)
                {
                    Log.Information("{@i}", i);
                }
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the create instance operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(CreateInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                Log.Information($"Attempting to retrieve service version ...");

                var availableServiceVersions = service.GetServiceVersion().Result;
                Log.Information($"{availableServiceVersions.Count() } service versions available.");

                // resolve the service version that is being used.
                Model.ServiceVersion serviceVersion = null;

                // ensure that a specific service version name has been specified.
                if (!string.IsNullOrEmpty(opts.ServiceVersionName))
                {
                    serviceVersion = availableServiceVersions
                        .Where(x => x.Name.Equals(opts.ServiceVersionName, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();
                }
                else
                {
                    serviceVersion = availableServiceVersions.FirstOrDefault();
                }

                if (serviceVersion == null)
                {
                    throw new InvalidOperationException("Unable to find any service versions associated with login.");
                }

                Log.Information($"Using service version: {serviceVersion.Name}.");

                var c = new Model.CreateInstance()
                {
                    ServiceVersionId = serviceVersion.Id,
                    Type = opts.Type,
                    BaseLanguage = opts.BaseLanguage,
                    FriendlyName = opts.FriendlyName,
                    DomainName = opts.DomainName,
                    InitialUserEmail = opts.InitialUserEmail,
                    IsValidateOnlyRequest = opts.ValidateOnly
                };

                Log.Information("Creating new instance with parameters ({@c}) ...", c);

                var status = service.CreateInstance(c).Result;

                Log.Information("Result: {@status}", status);
                Log.Information("Operation completed successfully.");
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the delete instance operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(DeleteInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            if (!opts.ValidateOnly && !opts.Confirm)
            {
                Console.Write($"Are you sure to proceed with deletion of the instance?");
                var key = Console.ReadKey();
                if (key.KeyChar != 'Y' && key.KeyChar != 'y')
                {
                    Console.Write(Environment.NewLine);
                    Log.Warning("User aborted deletion of instance. Exiting.");
                    return 1;
                }

                Console.Write(Environment.NewLine);
            }

            Task.Run(() =>
            {
                Guid? instanceId = Guid.Empty;
                if (opts.InstanceId != Guid.Empty)
                {
                    Log.Debug($"Using InstanceId parameter: {opts.InstanceId}");
                    instanceId = opts.InstanceId;
                }
                else if (!string.IsNullOrEmpty(opts.InstanceFriendlyName))
                {
                    Log.Debug($"Attempting to retrieve instance from instance friendly name: {opts.InstanceFriendlyName}");
                    var instances = service.GetInstances().Result;
                    instanceId = instances?
                        .Where(x => x.FriendlyName.Equals(opts.InstanceFriendlyName, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault()?.Id;
                }
                else
                {
                    throw new ArgumentException("Instance Id or instance friendly name not provided.");
                }

                if (!instanceId.HasValue || instanceId == Guid.Empty)
                {
                    throw new InvalidOperationException("Unable to resolve unique instance identifier.");
                }

                var status = service.DeleteInstance(new Model.DeleteInstance()
                {
                    InstanceId = instanceId.Value,
                    IsValidateOnlyRequest = opts.ValidateOnly
                }).Result;

                Log.Information("{@status}", status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the get operation status operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetOperationStatusOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                var status = service.GetOperationStatus(new Model.GetOperationStatus()
                {
                    OperationId = opts.OperationId
                }).Result;

                Log.Information("Result: {@status}");
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the get service version operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetServiceVersions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                service.GetServiceVersion();
            })
            .Wait();

            return 0;
        }
    }
}