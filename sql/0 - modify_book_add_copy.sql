IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AbpEditions] (
    [Id] int NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_AbpEditions] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpAuditLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [BrowserInfo] nvarchar(256) NULL,
    [ClientIpAddress] nvarchar(64) NULL,
    [ClientName] nvarchar(128) NULL,
    [CustomData] nvarchar(2000) NULL,
    [Exception] nvarchar(2000) NULL,
    [ExecutionDuration] int NOT NULL,
    [ExecutionTime] datetime2 NOT NULL,
    [ImpersonatorTenantId] int NULL,
    [ImpersonatorUserId] bigint NULL,
    [MethodName] nvarchar(256) NULL,
    [Parameters] nvarchar(1024) NULL,
    [ServiceName] nvarchar(256) NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_AbpAuditLogs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserAccounts] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EmailAddress] nvarchar(450) NULL,
    [IsDeleted] bit NOT NULL,
    [LastLoginTime] datetime2 NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    [UserLinkId] bigint NULL,
    [UserName] nvarchar(450) NULL,
    CONSTRAINT [PK_AbpUserAccounts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserLoginAttempts] (
    [Id] bigint NOT NULL IDENTITY,
    [BrowserInfo] nvarchar(256) NULL,
    [ClientIpAddress] nvarchar(64) NULL,
    [ClientName] nvarchar(128) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Result] tinyint NOT NULL,
    [TenancyName] nvarchar(64) NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    [UserNameOrEmailAddress] nvarchar(255) NULL,
    CONSTRAINT [PK_AbpUserLoginAttempts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserOrganizationUnits] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [OrganizationUnitId] bigint NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserOrganizationUnits] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpBackgroundJobs] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [IsAbandoned] bit NOT NULL,
    [JobArgs] nvarchar(max) NOT NULL,
    [JobType] nvarchar(512) NOT NULL,
    [LastTryTime] datetime2 NULL,
    [NextTryTime] datetime2 NOT NULL,
    [Priority] tinyint NOT NULL,
    [TryCount] smallint NOT NULL,
    CONSTRAINT [PK_AbpBackgroundJobs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpLanguages] (
    [Id] int NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [Icon] nvarchar(128) NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(10) NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpLanguages] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpLanguageTexts] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Key] nvarchar(256) NOT NULL,
    [LanguageName] nvarchar(10) NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Source] nvarchar(128) NOT NULL,
    [TenantId] int NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AbpLanguageTexts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Data] nvarchar(max) NULL,
    [DataTypeName] nvarchar(512) NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [ExcludedUserIds] nvarchar(max) NULL,
    [NotificationName] nvarchar(96) NOT NULL,
    [Severity] tinyint NOT NULL,
    [TenantIds] nvarchar(max) NULL,
    [UserIds] nvarchar(max) NULL,
    CONSTRAINT [PK_AbpNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpNotificationSubscriptions] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [NotificationName] nvarchar(96) NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpNotificationSubscriptions] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpTenantNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Data] nvarchar(max) NULL,
    [DataTypeName] nvarchar(512) NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [NotificationName] nvarchar(96) NOT NULL,
    [Severity] tinyint NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpTenantNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [State] int NOT NULL,
    [TenantId] int NULL,
    [TenantNotificationId] uniqueidentifier NOT NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpOrganizationUnits] (
    [Id] bigint NOT NULL IDENTITY,
    [Code] nvarchar(95) NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(128) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [ParentId] bigint NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpOrganizationUnits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [AbpOrganizationUnits] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpUsers] (
    [Id] bigint NOT NULL IDENTITY,
    [AccessFailedCount] int NOT NULL,
    [AuthenticationSource] nvarchar(64) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EmailAddress] nvarchar(256) NOT NULL,
    [EmailConfirmationCode] nvarchar(328) NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsEmailConfirmed] bit NOT NULL,
    [IsLockoutEnabled] bit NOT NULL,
    [IsPhoneNumberConfirmed] bit NOT NULL,
    [IsTwoFactorEnabled] bit NOT NULL,
    [LastLoginTime] datetime2 NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [LockoutEndDateUtc] datetime2 NULL,
    [Name] nvarchar(32) NOT NULL,
    [NormalizedEmailAddress] nvarchar(256) NOT NULL,
    [NormalizedUserName] nvarchar(32) NOT NULL,
    [Password] nvarchar(128) NOT NULL,
    [PasswordResetCode] nvarchar(328) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [Surname] nvarchar(32) NOT NULL,
    [TenantId] int NULL,
    [UserName] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_AbpUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUsers_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpUsers_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpUsers_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpFeatures] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [EditionId] int NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpFeatures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpFeatures_AbpEditions_EditionId] FOREIGN KEY ([EditionId]) REFERENCES [AbpEditions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserClaims] (
    [Id] bigint NOT NULL IDENTITY,
    [ClaimType] nvarchar(450) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserClaims_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserLogins] (
    [Id] bigint NOT NULL IDENTITY,
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(256) NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserLogins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserLogins_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserRoles] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [RoleId] int NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserRoles_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserTokens] (
    [Id] bigint NOT NULL IDENTITY,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AbpUserTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserTokens_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpSettings] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(256) NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    [Value] nvarchar(2000) NULL,
    CONSTRAINT [PK_AbpSettings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpSettings_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpRoles] (
    [Id] int NOT NULL IDENTITY,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [IsDefault] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsStatic] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(32) NOT NULL,
    [NormalizedName] nvarchar(32) NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpRoles_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpRoles_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpRoles_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpTenants] (
    [Id] int NOT NULL IDENTITY,
    [ConnectionString] nvarchar(1024) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EditionId] int NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(128) NOT NULL,
    [TenancyName] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_AbpTenants] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpTenants_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpEditions_EditionId] FOREIGN KEY ([EditionId]) REFERENCES [AbpEditions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpPermissions] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [IsGranted] bit NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [TenantId] int NULL,
    [RoleId] int NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_AbpPermissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpPermissions_AbpRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AbpRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AbpPermissions_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpRoleClaims] (
    [Id] bigint NOT NULL IDENTITY,
    [ClaimType] nvarchar(450) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [RoleId] int NOT NULL,
    [TenantId] int NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_AbpRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpRoleClaims_AbpRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpRoles] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AbpFeatures_EditionId_Name] ON [AbpFeatures] ([EditionId], [Name]);

GO

CREATE INDEX [IX_AbpFeatures_TenantId_Name] ON [AbpFeatures] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_ExecutionDuration] ON [AbpAuditLogs] ([TenantId], [ExecutionDuration]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_ExecutionTime] ON [AbpAuditLogs] ([TenantId], [ExecutionTime]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_UserId] ON [AbpAuditLogs] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpPermissions_TenantId_Name] ON [AbpPermissions] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpPermissions_RoleId] ON [AbpPermissions] ([RoleId]);

GO

CREATE INDEX [IX_AbpPermissions_UserId] ON [AbpPermissions] ([UserId]);

GO

CREATE INDEX [IX_AbpRoleClaims_RoleId] ON [AbpRoleClaims] ([RoleId]);

GO

CREATE INDEX [IX_AbpRoleClaims_UserId] ON [AbpRoleClaims] ([UserId]);

GO

CREATE INDEX [IX_AbpRoleClaims_TenantId_ClaimType] ON [AbpRoleClaims] ([TenantId], [ClaimType]);

GO

CREATE INDEX [IX_AbpUserAccounts_EmailAddress] ON [AbpUserAccounts] ([EmailAddress]);

GO

CREATE INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts] ([UserName]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_EmailAddress] ON [AbpUserAccounts] ([TenantId], [EmailAddress]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_UserId] ON [AbpUserAccounts] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts] ([TenantId], [UserName]);

GO

CREATE INDEX [IX_AbpUserClaims_UserId] ON [AbpUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AbpUserClaims_TenantId_ClaimType] ON [AbpUserClaims] ([TenantId], [ClaimType]);

GO

CREATE INDEX [IX_AbpUserLogins_UserId] ON [AbpUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AbpUserLogins_TenantId_UserId] ON [AbpUserLogins] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey] ON [AbpUserLogins] ([TenantId], [LoginProvider], [ProviderKey]);

GO

CREATE INDEX [IX_AbpUserLoginAttempts_UserId_TenantId] ON [AbpUserLoginAttempts] ([UserId], [TenantId]);

GO

CREATE INDEX [IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result] ON [AbpUserLoginAttempts] ([TenancyName], [UserNameOrEmailAddress], [Result]);

GO

CREATE INDEX [IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId] ON [AbpUserOrganizationUnits] ([TenantId], [OrganizationUnitId]);

GO

CREATE INDEX [IX_AbpUserOrganizationUnits_TenantId_UserId] ON [AbpUserOrganizationUnits] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserRoles_UserId] ON [AbpUserRoles] ([UserId]);

GO

CREATE INDEX [IX_AbpUserRoles_TenantId_RoleId] ON [AbpUserRoles] ([TenantId], [RoleId]);

GO

CREATE INDEX [IX_AbpUserRoles_TenantId_UserId] ON [AbpUserRoles] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserTokens_UserId] ON [AbpUserTokens] ([UserId]);

GO

CREATE INDEX [IX_AbpUserTokens_TenantId_UserId] ON [AbpUserTokens] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpBackgroundJobs_IsAbandoned_NextTryTime] ON [AbpBackgroundJobs] ([IsAbandoned], [NextTryTime]);

GO

CREATE INDEX [IX_AbpSettings_UserId] ON [AbpSettings] ([UserId]);

GO

CREATE INDEX [IX_AbpSettings_TenantId_Name] ON [AbpSettings] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpLanguages_TenantId_Name] ON [AbpLanguages] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key] ON [AbpLanguageTexts] ([TenantId], [Source], [LanguageName], [Key]);

GO

CREATE INDEX [IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId] ON [AbpNotificationSubscriptions] ([NotificationName], [EntityTypeName], [EntityId], [UserId]);

GO

CREATE INDEX [IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId] ON [AbpNotificationSubscriptions] ([TenantId], [NotificationName], [EntityTypeName], [EntityId], [UserId]);

GO

CREATE INDEX [IX_AbpTenantNotifications_TenantId] ON [AbpTenantNotifications] ([TenantId]);

GO

CREATE INDEX [IX_AbpUserNotifications_UserId_State_CreationTime] ON [AbpUserNotifications] ([UserId], [State], [CreationTime]);

GO

CREATE INDEX [IX_AbpOrganizationUnits_ParentId] ON [AbpOrganizationUnits] ([ParentId]);

GO

CREATE INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits] ([TenantId], [Code]);

GO

CREATE INDEX [IX_AbpRoles_CreatorUserId] ON [AbpRoles] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpRoles_DeleterUserId] ON [AbpRoles] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpRoles_LastModifierUserId] ON [AbpRoles] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpRoles_TenantId_NormalizedName] ON [AbpRoles] ([TenantId], [NormalizedName]);

GO

CREATE INDEX [IX_AbpUsers_CreatorUserId] ON [AbpUsers] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpUsers_DeleterUserId] ON [AbpUsers] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpUsers_LastModifierUserId] ON [AbpUsers] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpUsers_TenantId_NormalizedEmailAddress] ON [AbpUsers] ([TenantId], [NormalizedEmailAddress]);

GO

CREATE INDEX [IX_AbpUsers_TenantId_NormalizedUserName] ON [AbpUsers] ([TenantId], [NormalizedUserName]);

GO

CREATE INDEX [IX_AbpTenants_CreatorUserId] ON [AbpTenants] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpTenants_DeleterUserId] ON [AbpTenants] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpTenants_EditionId] ON [AbpTenants] ([EditionId]);

GO

CREATE INDEX [IX_AbpTenants_LastModifierUserId] ON [AbpTenants] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpTenants_TenancyName] ON [AbpTenants] ([TenancyName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170424115119_Initial_Migrations', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AbpRoleClaims] DROP CONSTRAINT [FK_AbpRoleClaims_AbpRoles_UserId];

GO

DROP INDEX [IX_AbpRoleClaims_UserId] ON [AbpRoleClaims];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AbpRoleClaims') AND [c].[name] = N'UserId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoleClaims] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AbpRoleClaims] DROP COLUMN [UserId];

GO

ALTER TABLE [AbpLanguages] ADD [IsDisabled] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [AbpRoleClaims] ADD CONSTRAINT [FK_AbpRoleClaims_AbpRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AbpRoles] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170608053244_Upgraded_To_Abp_2_1_0', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AbpRoles] ADD [Description] nvarchar(max) NULL;

GO

ALTER TABLE [AbpRoles] ADD [IsActive] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170621153937_Added_Description_And_IsActive_To_Role', N'2.0.0-rtm-26452');

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AbpRoles') AND [c].[name] = N'IsActive');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoles] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AbpRoles] DROP COLUMN [IsActive];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170703134115_Remove_IsActive_From_Role', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AbpUserOrganizationUnits] ADD [IsDeleted] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170804083601_Upgraded_To_Abp_v2.2.2', N'2.0.0-rtm-26452');

GO

CREATE TABLE [Books] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    [Title] nvarchar(max) NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170912124823_add_book', N'2.0.0-rtm-26452');

GO

ALTER TABLE [Books] DROP CONSTRAINT [PK_Books];

GO

EXEC sp_rename N'Books', N'AppBooks';

GO

ALTER TABLE [AppBooks] ADD [Author] nvarchar(max) NULL;

GO

ALTER TABLE [AppBooks] ADD [Isbn] nvarchar(max) NULL;

GO

ALTER TABLE [AppBooks] ADD [Publish] nvarchar(max) NULL;

GO

ALTER TABLE [AppBooks] ADD CONSTRAINT [PK_AppBooks] PRIMARY KEY ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170917042452_modify-book', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AppBooks] ADD [Count] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170918054505_modify_book_add_count', N'2.0.0-rtm-26452');

GO

CREATE TABLE [AppBorrowRecords] (
    [Id] bigint NOT NULL IDENTITY,
    [BookId] bigint NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AppBorrowRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppBorrowRecords_AppBooks_BookId] FOREIGN KEY ([BookId]) REFERENCES [AppBooks] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AppBorrowRecords_BookId] ON [AppBorrowRecords] ([BookId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170918063351_add_table_BorrowRecord', N'2.0.0-rtm-26452');

GO

CREATE INDEX [IX_AppBorrowRecords_CreatorUserId] ON [AppBorrowRecords] ([CreatorUserId]);

GO

CREATE INDEX [IX_AppBorrowRecords_DeleterUserId] ON [AppBorrowRecords] ([DeleterUserId]);

GO

ALTER TABLE [AppBorrowRecords] ADD CONSTRAINT [FK_AppBorrowRecords_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AppBorrowRecords] ADD CONSTRAINT [FK_AppBorrowRecords_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170918072604_modify_BorrowRecord', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AppBorrowRecords] DROP CONSTRAINT [FK_AppBorrowRecords_AppBooks_BookId];

GO

DROP INDEX [IX_AppBorrowRecords_BookId] ON [AppBorrowRecords];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AppBorrowRecords') AND [c].[name] = N'BookId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AppBorrowRecords] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AppBorrowRecords] ALTER COLUMN [BookId] bigint NOT NULL;
CREATE INDEX [IX_AppBorrowRecords_BookId] ON [AppBorrowRecords] ([BookId]);

GO

ALTER TABLE [AppBorrowRecords] ADD CONSTRAINT [FK_AppBorrowRecords_AppBooks_BookId] FOREIGN KEY ([BookId]) REFERENCES [AppBooks] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170918091433_modify_BorrowRecord_add_bookId', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AppBorrowRecords] ADD [BorrowerUserId] bigint NOT NULL DEFAULT 0;

GO

ALTER TABLE [AppBorrowRecords] ADD [RenewTime] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170918134413_modify_add_borrower', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AppBorrowRecords] DROP CONSTRAINT [FK_AppBorrowRecords_AppBooks_BookId];

GO

DROP INDEX [IX_AppBorrowRecords_BookId] ON [AppBorrowRecords];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AppBorrowRecords') AND [c].[name] = N'BookId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AppBorrowRecords] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AppBorrowRecords] DROP COLUMN [BookId];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AppBooks') AND [c].[name] = N'Count');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AppBooks] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AppBooks] DROP COLUMN [Count];

GO

ALTER TABLE [AppBorrowRecords] ADD [CopyId] bigint NOT NULL DEFAULT 0;

GO

ALTER TABLE [AppBooks] ADD [Location] nvarchar(max) NULL;

GO

CREATE TABLE [AppBookCopys] (
    [Id] bigint NOT NULL IDENTITY,
    [BookId] bigint NOT NULL,
    [BorrowRecordId] bigint NULL,
    CONSTRAINT [PK_AppBookCopys] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppBookCopys_AppBooks_BookId] FOREIGN KEY ([BookId]) REFERENCES [AppBooks] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppBookCopys_AppBorrowRecords_BorrowRecordId] FOREIGN KEY ([BorrowRecordId]) REFERENCES [AppBorrowRecords] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AppBookCopys_BookId] ON [AppBookCopys] ([BookId]);

GO

CREATE UNIQUE INDEX [IX_AppBookCopys_BorrowRecordId] ON [AppBookCopys] ([BorrowRecordId]) WHERE [BorrowRecordId] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171017134752_modify_book_add_copy', N'2.0.0-rtm-26452');

GO

