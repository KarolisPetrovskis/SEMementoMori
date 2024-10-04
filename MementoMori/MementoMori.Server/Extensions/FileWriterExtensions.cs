using MementoMori.Server;
using Microsoft.AspNetCore.Mvc;

public static class FileWriterExtensions
{
    public static FileWriter InitializeFileWriter(this ControllerBase controller)
    {
        return new FileWriter();
    }
}
