using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace aether.Controle
{
    public static class AetherLogger
    {
        private static readonly Stopwatch AppTimer = Stopwatch.StartNew();
        private static Process currentProcess = Process.GetCurrentProcess();
        private static readonly object _consoleLock = new object();

        // Linha onde os logs de eventos começam (para não sobrescrever o dashboard)
        private static int logStartRow = 6;

        public static void MonitorResources()
        {
            // Esconde o cursor para evitar o "flicker" (pisca-pisca)
            try { Console.CursorVisible = false; } catch { }

            Task.Run(() =>
            {
                while (true)
                {
                    lock (_consoleLock)
                    {
                        currentProcess.Refresh();
                        long memPrivada = currentProcess.PrivateMemorySize64 / 1024 / 1024;
                        int threads = currentProcess.Threads.Count;
                        int handles = currentProcess.HandleCount;
                        string uptime = AppTimer.Elapsed.ToString(@"dd\.hh\:mm\:ss");

                        // Salva a posição atual para não atrapalhar o log que está sendo escrito
                        int originalLeft = Console.CursorLeft;
                        int originalTop = Console.CursorTop;

                        // Desenha o Dashboard no topo (Linhas 0 a 4)
                        Console.SetCursorPosition(0, 0);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($" >>> AETHER NETWORK ENGINE - MONITORAMENTO EM TEMPO REAL <<< ".PadRight(Console.WindowWidth));
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($" UPTIME: {uptime} | THREADS: {threads} | HANDLES: {handles} ".PadRight(Console.WindowWidth));

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($" MEMÓRIA PRIVADA: {memPrivada}MB | GC GEN: {GC.CollectionCount(0)}/{GC.CollectionCount(1)} ".PadRight(Console.WindowWidth));

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(new string('─', Console.WindowWidth));

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($" [LOG DE EVENTOS RECENTES]");
                        Console.WriteLine(new string('─', Console.WindowWidth));

                        // Restaura a posição se estiver fora da área do dashboard
                        if (originalTop < logStartRow)
                            Console.SetCursorPosition(0, logStartRow);
                        else
                            Console.SetCursorPosition(originalLeft, originalTop);
                    }
                    Thread.Sleep(1000); // Atualiza o Dashboard a cada 1 segundo
                }
            });
        }

        private static void Write(string tag, string message, ConsoleColor color)
        {
            lock (_consoleLock)
            {
                // Garante que o log comece sempre abaixo do dashboard
                if (Console.CursorTop < logStartRow)
                    Console.SetCursorPosition(0, logStartRow);

                string time = DateTime.Now.ToString("HH:mm:ss");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{time}] ");

                Console.ForegroundColor = color;
                Console.Write($"[{tag.PadRight(8)}] ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);

                Console.ResetColor();
            }
        }

        // --- MÉTODOS DE LOG (Mantidos para compatibilidade) ---
        public static void Network(string ip, string msg, bool success = true)
            => Write("NET", $"[{ip}] {msg}", success ? ConsoleColor.Green : ConsoleColor.Red);

        public static void UI(string component, string action)
            => Write("USER_IF", $"<{component}> {action}", ConsoleColor.Blue);

        public static void Disk(string path, string action, long size = 0)
            => Write("DISK_IO", $"{action}: {Path.GetFileName(path)}", ConsoleColor.Yellow);

        public static void Security(string msg, bool alert = false)
            => Write("SEC_AUTH", msg, alert ? ConsoleColor.Red : ConsoleColor.DarkYellow);
    }
}