using System;
using Nautilus.Commands;
using SubnauticaEntityRipper.Data.Exporters;
using SubnauticaEntityRipper.Data.Implementation;
using SubnauticaEntityRipper.Data.Interfaces;
using SubnauticaEntityRipper.Utility;

namespace SubnauticaEntityRipper;

public static class Commands
{
    [ConsoleCommand("exportallentities")]
    public static void ExportAllEntities()
    {
        PerformGenericExport<EntityCoreDataExporter>("ExportAll");
    }
    
    [ConsoleCommand("countentities")]
    public static void CountEntities()
    {
        PerformGenericExport<CountingExporter>("Counted");
    }
    
    [ConsoleCommand("printunusedentities")]
    public static void PrintUnusedEntities()
    {
        PerformGenericExport<UnusedEntitiesExporter>("Unused");
    }

    private static void PerformGenericExport<T>(string prefix) where T : IEntityExporter
    {
        var outputFile = DataExportFolder.GetQuickPathForNewExport(prefix);

        var parser = new StandardBatchParser(false);

        ((IEntityExporter)Activator.CreateInstance(typeof(T))).ExportData(parser, CellsUtils.GetAllCells(true),
            outputFile);
    }
}