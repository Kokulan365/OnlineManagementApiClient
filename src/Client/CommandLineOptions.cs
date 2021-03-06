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
using CommandLine;

namespace OnlineManagementApiClient
{
    /// <summary>
    /// Shared command line options
    /// </summary>
    public abstract class BaseOptions
    {
        [Option(shortName: 's', longName: "serviceurl", Default = "https://admin.services.crm6.dynamics.com", Required = false, HelpText = "A valid service url.")]
        public string ServiceUrl { get; set; }

        [Option(Required = false, HelpText = "The username to connect to the service.")]
        public string Username { get; set; }

        [Option(Required = false, HelpText = "The password associated with username.")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Get instances command line options
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetInstances", HelpText = "Retrieves a Customer Engagement instance in your Office 365 tenant.")]
    public class GetInstancesOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the unique name of a specific instance.
        /// </summary>
        /// <value>
        /// The unique name of the instance.
        /// </value>
        [Option(shortName: 'u', longName: "uniquename", Required = false, HelpText = "Retrieve instance using a unique name.")]
        public string UniqueName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of a specific instance..
        /// </summary>
        /// <value>
        /// The friendly name of the instance.
        /// </value>
        [Option(shortName: 'f', longName: "friendlyname", Required = false, HelpText = "Retrieve instance using a friendly name.")]
        public string FriendlyName { get; set; }
    }

    /// <summary>
    /// Create instance command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("CreateInstance", HelpText = "Provisions (creates) a Customer Engagement instance in your Office 365 tenant.")]
    public class CreateInstanceOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the friendly name of the instance.
        /// </summary>
        /// <value>
        /// The friendly name of the instance.
        /// </value>
        [Option(shortName: 'f', longName: "friendlyname", Required = true, HelpText = "Friendly name for the new instance.")]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        /// <value>
        /// The domain name.
        /// </value>
        [Option(shortName: 'd', longName: "domainname", Required = true, HelpText = "Domain name for the new instance.")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the initial user email.
        /// </summary>
        /// <value>
        /// The initial user email.
        /// </value>
        [Option(shortName: 'e', longName: "initialuseremail", Required = true, HelpText = "Initial user email address.")]
        public string InitialUserEmail { get; set; }

        /// <summary>
        /// Gets or sets the service version name.
        /// </summary>
        /// <value>
        /// The service version name.
        /// </value>
        [Option(shortName: 'v', longName: "serviceversionname", Default = "", Required = false, HelpText = "Service version name. If not provided, the first one will be queried from the service.")]
        public string ServiceVersionName { get; set; }

        /// <summary>
        /// Gets or sets the service instance type.
        /// </summary>
        /// <value>
        /// The service instance type.
        /// </value>
        [Option(shortName: 't', longName: "type", Default = 2, HelpText = "The service instance type. Valid options are: 1 = Production, 2 = Sandbox. Defaults to Sandbox.")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the base language.
        /// </summary>
        /// <value>
        /// The base language.
        /// </value>
        [Option(shortName: 'l', longName: "baselanguage", Default = "1033", Required = false, HelpText = "Base language Id. Defaults to English (1033).")]
        public string BaseLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is validation request only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validation request; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'v', longName: "validateonly", Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnly { get; set; }
    }

    /// <summary>
    /// Delete instance command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("DeleteInstance", HelpText = "Deletes a Customer Engagement instance in your Office 365 tenant.")]
    public class DeleteInstanceOptions : BaseOptions
    {
        [Option(shortName: 'f', longName: "friendlyname", Required = false, HelpText = "The friendly name of the instance to delete.")]
        public string InstanceFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the unique instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        [Option(shortName: 'i', longName: "instanceid", Required = false, HelpText = "The unique identifier of the instance to delete.")]
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is validation request only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validation request; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'v', longName: "validateonly", Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnly { get; set; }

        /// <summary>
        /// Gets or sets a value confirming the continuation of the delete process.
        /// </summary>
        /// <value>
        ///   <c>true</c> if confirm; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'c', longName: "confirm", Default = false, HelpText = "Confirm the deletion of the instance.")]
        public bool Confirm { get; set; }
    }

    /// <summary>
    /// Get Operation Status command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetOperation", HelpText = "Retrieves status of an operation in your Customer Engagement instance.")]
    public class GetOperationStatusOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the unique operation identifier.
        /// </summary>
        /// <value>
        /// The operation identifier.
        /// </value>
        [Option(shortName: 'o', longName: "operationid", Required = true, HelpText = "Operation Id.")]
        public Guid OperationId { get; set; }
    }

    /// <summary>
    /// Get Service Versions command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetServiceVersions", HelpText = "Retrieves information about all the supported releases for Customer Engagement.")]
    public class GetServiceVersions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the service version name.
        /// </summary>
        /// <value>
        /// The service verison name.
        /// </value>
        [Option(shortName: 'n', longName: "name", Default = "", Required = false, HelpText = "Service version name.")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Get backup instances command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetBackups", HelpText = "Retrieves all backups of a Customer Engagement instance.")]
    public class GetInstanceBackupsOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        [Option(shortName: 'i', longName: "instanceid", Required = true, HelpText = "The unique identifier of the instance.")]
        public Guid InstanceId { get; set; }
    }

    /// <summary>
    /// Create backup command line options.
    /// </summary>
    [Verb("CreateBackup", HelpText = "Backs up a Customer Engagement instance.")]
    public class CreateInstanceBackupOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        [Option(shortName: 'i', longName: "instanceid", Required = true, HelpText = "The unique identifier of the instance to backup.")]
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [Option(shortName: 'l', longName: "label", Required = true, HelpText = "Label to help identify this backup for future restoration.")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is azure backup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is azure backup; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'a', longName: "isazurebackup", Required = false, HelpText = "The unique identifier of the instance to backup.")]
        public bool IsAzureBackup { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [Option(shortName: 'n', longName: "notes", Required = false, HelpText = "Notes to help identify this backup for future restoration.")]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Restore backup command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("RestoreBackup", HelpText = "Restores a Customer Engagement instance in your Office 365 tenant.")]
    public class RestoreInstanceBackupOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the source instance identifier.
        /// </summary>
        /// <value>
        /// The source instance identifier.
        /// </value>
        [Option(shortName: 's', longName: "sourceinstanceid", Required = true, HelpText = "The unique identifier of the source instance.")]
        public Guid SourceInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the target instance identifier.
        /// </summary>
        /// <value>
        /// The source instance identifier.
        /// </value>
        [Option(shortName: 't', longName: "targetinstanceid", Required = true, HelpText = "The unique identifier of the target instance.")]
        public Guid TargetInstanceId { get; set; }
        
        /// <summary>
        /// Gets or sets the instance backup identifier.
        /// </summary>
        /// <value>
        /// The instance backup identifier.
        /// </value>
        [Option(shortName: 'b', longName: "instancebackupid", Required = false, HelpText = "The unique identifier of the instance backup id.")]
        public Guid InstanceBackupId { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [Option(shortName: 'l', longName: "label", Required = false, HelpText = "Label to help identify a backup for restoration.")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        [Option(shortName: 'c', longName: "createdon", Required = false, HelpText = "The created on date time of an existing instance backup to use.")]
        public DateTime CreatedOn { get; set; }
    }
}
