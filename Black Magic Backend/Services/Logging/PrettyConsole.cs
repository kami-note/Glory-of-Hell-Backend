using System;

public static class PrettyConsole
{
    public static void LogInfo(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{timestamp}] [INFO] {message}");
        Console.ResetColor();
    }

    public static void LogWarning(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{timestamp}] [WARNING] {message}");
        Console.ResetColor();
    }

    public static void LogError(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{timestamp}] [ERROR] {message}");
        Console.ResetColor();
    }

    public static void LogConnection(string playerIp)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"[{timestamp}] 🟢 [CONNECTION] Jogador conectado de {playerIp}");
        Console.ResetColor();
    }

    public static void LogPacketReceived(string packetType)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{timestamp}] [PACKET RECEIVED] {packetType}");
        Console.ResetColor();
    }
}