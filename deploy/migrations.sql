IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Nom] nvarchar(200) NOT NULL,
        [Type] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE TABLE [Utilisateurs] (
        [Id] uniqueidentifier NOT NULL,
        [Nom] nvarchar(200) NOT NULL,
        [Email] nvarchar(320) NOT NULL,
        [DateCreation] datetime2 NOT NULL,
        CONSTRAINT [PK_Utilisateurs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE TABLE [Comptes] (
        [Id] uniqueidentifier NOT NULL,
        [Nom] nvarchar(200) NOT NULL,
        [UtilisateurId] uniqueidentifier NOT NULL,
        [DateCreation] datetime2 NOT NULL,
        CONSTRAINT [PK_Comptes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comptes_Utilisateurs_UtilisateurId] FOREIGN KEY ([UtilisateurId]) REFERENCES [Utilisateurs] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE TABLE [Transactions] (
        [Id] uniqueidentifier NOT NULL,
        [Montant] decimal(18,2) NOT NULL,
        [Type] nvarchar(20) NOT NULL,
        [Date] datetime2 NOT NULL,
        [Description] nvarchar(500) NULL,
        [CompteId] uniqueidentifier NOT NULL,
        [CategorieId] uniqueidentifier NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Transactions_Categories_CategorieId] FOREIGN KEY ([CategorieId]) REFERENCES [Categories] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_Transactions_Comptes_CompteId] FOREIGN KEY ([CompteId]) REFERENCES [Comptes] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Comptes_UtilisateurId] ON [Comptes] ([UtilisateurId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_CategorieId] ON [Transactions] ([CategorieId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_CompteId] ON [Transactions] ([CompteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Transactions_CompteId_Date] ON [Transactions] ([CompteId], [Date]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Utilisateurs_Email] ON [Utilisateurs] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805115159_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805115159_InitialCreate', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805130136_AddMotDePasseHash'
)
BEGIN
    ALTER TABLE [Utilisateurs] ADD [MotDePasseHash] nvarchar(500) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805130136_AddMotDePasseHash'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805130136_AddMotDePasseHash', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805152806_AddPrenomToUtilisateur'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Utilisateurs]') AND [c].[name] = N'Nom');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Utilisateurs] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Utilisateurs] ALTER COLUMN [Nom] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805152806_AddPrenomToUtilisateur'
)
BEGIN
    ALTER TABLE [Utilisateurs] ADD [Prenom] nvarchar(50) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805152806_AddPrenomToUtilisateur'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805152806_AddPrenomToUtilisateur', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [Transactions] DROP CONSTRAINT [FK_Transactions_Comptes_CompteId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [Comptes] DROP CONSTRAINT [FK_Comptes_Utilisateurs_UtilisateurId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'[Comptes]', N'SourcesRevenu', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'[Transactions].[CompteId]', N'SourceRevenuId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'[Transactions].[IX_Transactions_CompteId_Date]', N'IX_Transactions_SourceRevenuId_Date', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'[Transactions].[IX_Transactions_CompteId]', N'IX_Transactions_SourceRevenuId', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'[SourcesRevenu].[IX_Comptes_UtilisateurId]', N'IX_SourcesRevenu_UtilisateurId', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    EXEC sp_rename N'dbo.PK_Comptes', N'PK_SourcesRevenu', N'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD [Type] nvarchar(20) NOT NULL DEFAULT N'Autre';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD [EstActif] bit NOT NULL DEFAULT CAST(1 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD CONSTRAINT [FK_SourcesRevenu_Utilisateurs_UtilisateurId] FOREIGN KEY ([UtilisateurId]) REFERENCES [Utilisateurs] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    ALTER TABLE [Transactions] ADD CONSTRAINT [FK_Transactions_SourcesRevenu_SourceRevenuId] FOREIGN KEY ([SourceRevenuId]) REFERENCES [SourcesRevenu] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815115414_RenameCompteToSourceRevenu'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260815115414_RenameCompteToSourceRevenu', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    CREATE TABLE [TypesSourceRevenu] (
        [Id] uniqueidentifier NOT NULL,
        [Nom] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_TypesSourceRevenu] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nom') AND [object_id] = OBJECT_ID(N'[TypesSourceRevenu]'))
        SET IDENTITY_INSERT [TypesSourceRevenu] ON;
    EXEC(N'INSERT INTO [TypesSourceRevenu] ([Id], [Nom])
    VALUES (''11111111-1111-1111-1111-111111111111'', N''Salaire''),
    (''22222222-2222-2222-2222-222222222222'', N''Freelance''),
    (''33333333-3333-3333-3333-333333333333'', N''Investissement''),
    (''44444444-4444-4444-4444-444444444444'', N''Location''),
    (''55555555-5555-5555-5555-555555555555'', N''Autre'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nom') AND [object_id] = OBJECT_ID(N'[TypesSourceRevenu]'))
        SET IDENTITY_INSERT [TypesSourceRevenu] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD [TypeId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    UPDATE SourcesRevenu
    SET TypeId = CASE Type
        WHEN 'Salaire' THEN '11111111-1111-1111-1111-111111111111'
        WHEN 'Freelance' THEN '22222222-2222-2222-2222-222222222222'
        WHEN 'Investissement' THEN '33333333-3333-3333-3333-333333333333'
        WHEN 'Location' THEN '44444444-4444-4444-4444-444444444444'
        ELSE '55555555-5555-5555-5555-555555555555'
    END;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SourcesRevenu]') AND [c].[name] = N'TypeId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [SourcesRevenu] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [SourcesRevenu] ALTER COLUMN [TypeId] uniqueidentifier NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SourcesRevenu]') AND [c].[name] = N'Type');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [SourcesRevenu] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [SourcesRevenu] DROP COLUMN [Type];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    CREATE INDEX [IX_SourcesRevenu_TypeId] ON [SourcesRevenu] ([TypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD CONSTRAINT [FK_SourcesRevenu_TypesSourceRevenu_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [TypesSourceRevenu] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260815132752_TypeSourceRevenuAsReference'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260815132752_TypeSourceRevenuAsReference', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823125816_AddDevisesReference'
)
BEGIN
    CREATE TABLE [Devises] (
        [Id] uniqueidentifier NOT NULL,
        [Nom] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Devises] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823125816_AddDevisesReference'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nom') AND [object_id] = OBJECT_ID(N'[Devises]'))
        SET IDENTITY_INSERT [Devises] ON;
    EXEC(N'INSERT INTO [Devises] ([Id], [Nom])
    VALUES (''66666666-6666-6666-6666-666666666666'', N''Franc CFA''),
    (''77777777-7777-7777-7777-777777777777'', N''Euro''),
    (''88888888-8888-8888-8888-888888888888'', N''Dollar US'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nom') AND [object_id] = OBJECT_ID(N'[Devises]'))
        SET IDENTITY_INSERT [Devises] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823125816_AddDevisesReference'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260823125816_AddDevisesReference', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823131104_AddDeviseToSourceRevenu'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD [DeviseId] uniqueidentifier NOT NULL DEFAULT '66666666-6666-6666-6666-666666666666';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823131104_AddDeviseToSourceRevenu'
)
BEGIN
    CREATE INDEX [IX_SourcesRevenu_DeviseId] ON [SourcesRevenu] ([DeviseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823131104_AddDeviseToSourceRevenu'
)
BEGIN
    ALTER TABLE [SourcesRevenu] ADD CONSTRAINT [FK_SourcesRevenu_Devises_DeviseId] FOREIGN KEY ([DeviseId]) REFERENCES [Devises] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823131104_AddDeviseToSourceRevenu'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260823131104_AddDeviseToSourceRevenu', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    CREATE TABLE [RepartitionsSourceRevenu] (
        [Id] uniqueidentifier NOT NULL,
        [TransactionId] uniqueidentifier NOT NULL,
        [SourceRevenuId] uniqueidentifier NOT NULL,
        [Montant] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_RepartitionsSourceRevenu] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RepartitionsSourceRevenu_SourcesRevenu_SourceRevenuId] FOREIGN KEY ([SourceRevenuId]) REFERENCES [SourcesRevenu] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RepartitionsSourceRevenu_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    INSERT INTO RepartitionsSourceRevenu (Id, TransactionId, SourceRevenuId, Montant)
    SELECT NEWID(), Id, SourceRevenuId, Montant
    FROM Transactions;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    ALTER TABLE [Transactions] DROP CONSTRAINT [FK_Transactions_SourcesRevenu_SourceRevenuId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    DROP INDEX [IX_Transactions_SourceRevenuId] ON [Transactions];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    DROP INDEX [IX_Transactions_SourceRevenuId_Date] ON [Transactions];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transactions]') AND [c].[name] = N'Montant');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Transactions] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [Transactions] DROP COLUMN [Montant];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transactions]') AND [c].[name] = N'SourceRevenuId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Transactions] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [Transactions] DROP COLUMN [SourceRevenuId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    CREATE INDEX [IX_Transactions_Date] ON [Transactions] ([Date]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    CREATE INDEX [IX_RepartitionsSourceRevenu_SourceRevenuId] ON [RepartitionsSourceRevenu] ([SourceRevenuId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    CREATE INDEX [IX_RepartitionsSourceRevenu_TransactionId] ON [RepartitionsSourceRevenu] ([TransactionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260823134703_SplitTransactionAcrossSourcesRevenu'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260823134703_SplitTransactionAcrossSourcesRevenu', N'10.0.10');
END;

COMMIT;
GO

