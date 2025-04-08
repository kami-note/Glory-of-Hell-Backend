namespace Black_Magic_Backend {
    class Program {
        static void Main() {
            Server server = new Server("127.0.0.1", 7777);
            server.StartAsync().Wait();
        }
    }
}