BEGIN TRANSACTION;

-- Create Federations Table if not exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Federations')
BEGIN
    CREATE TABLE [dbo].[Federations] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Enabled] bit NOT NULL,
        CONSTRAINT [PK_Federations] PRIMARY KEY ([Id])
    )
    WITH 
    (
        SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[FederationsHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FederationContacts')
BEGIN
    CREATE TABLE [dbo].[FederationContacts] (
        [Id] uniqueidentifier NOT NULL,
        [FederationId] uniqueidentifier NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        CONSTRAINT [PK_FederationContacts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FederationContacts_Federations_FederationId] FOREIGN KEY ([FederationId]) REFERENCES [Federations] ([Id]) ON DELETE CASCADE
    )
    WITH 
    (
        SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[FederationContactsHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Members')
BEGIN
    CREATE TABLE [dbo].[Members] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Enabled] bit NOT NULL,
        [FederationId] uniqueidentifier NULL,
        CONSTRAINT [PK_Members] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Members_Federations_FederationId] FOREIGN KEY ([FederationId]) REFERENCES [Federations] ([Id])
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[MembersHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Inboxes')
BEGIN
    CREATE TABLE [dbo].[Inboxes] (
        [Id] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Enabled] bit NOT NULL,
        CONSTRAINT [PK_Inboxes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Inboxes_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[InboxesHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MemberContacts')
BEGIN
    CREATE TABLE [dbo].[MemberContacts] (
        [Id] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_MemberContacts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MemberContacts_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[MemberContactsHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MemberPublicCertificates')
BEGIN
    CREATE TABLE [dbo].[MemberPublicCertificates] (
        [Id] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [Active] bit NOT NULL,
        [PublicCertificatePem] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_MemberPublicCertificates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MemberPublicCertificates_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[MemberPublicCertificatesHistory]) , LEDGER = ON 
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InboxParameters')
BEGIN
    CREATE TABLE [dbo].[InboxParameters] (
        [InboxId] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Value] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_InboxParameters] PRIMARY KEY ([MemberId], [InboxId]),
        CONSTRAINT [FK_InboxParameters_Inboxes_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Inboxes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_InboxParameters_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[InboxParametersHistory]) , LEDGER = ON
    );
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InboxRestrictedMembers')
BEGIN

    CREATE TABLE [dbo].[InboxRestrictedMembers] (
        [InboxId] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [RestrictedMemberId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_InboxRestrictedMembers] PRIMARY KEY ([MemberId], [InboxId]),
        CONSTRAINT [FK_InboxRestrictedMembers_Inboxes_InboxId] FOREIGN KEY ([InboxId]) REFERENCES [Inboxes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_InboxRestrictedMembers_Members_RestrictedMemberId] FOREIGN KEY ([RestrictedMemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION
    )
    WITH 
    (
    SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[InboxRestrictedMembersHistory]) , LEDGER = ON
    );
END;

CREATE INDEX [IX_FederationContacts_FederationId] ON [FederationContacts] ([FederationId]);

CREATE INDEX [IX_Inboxes_MemberId] ON [Inboxes] ([MemberId]);

CREATE INDEX [IX_InboxRestrictedMembers_InboxId] ON [InboxRestrictedMembers] ([InboxId]);

CREATE INDEX [IX_InboxRestrictedMembers_RestrictedMemberId] ON [InboxRestrictedMembers] ([RestrictedMemberId]);

CREATE INDEX [IX_MemberContacts_MemberId] ON [MemberContacts] ([MemberId]);

CREATE INDEX [IX_MemberPublicCertificates_MemberId] ON [MemberPublicCertificates] ([MemberId]);

CREATE INDEX [IX_Members_FederationId] ON [Members] ([FederationId]);

COMMIT;
GO