ALTER TABLE [Compartments] ADD [DoorPosition] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200309123548_AddedDoorPosition', N'2.2.6-servicing-10079');

GO

