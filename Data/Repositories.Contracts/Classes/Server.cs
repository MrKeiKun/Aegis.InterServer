namespace Aegis.Data.Repositories.Contracts.Classes
{
    public class Server
    {
        public int SID { get; set; }
        public int Type { get; set; }
        public string IP { get; set; }
        public short PrivatePort { get; set; }
        public short Port { get; set; }
        public string SvrName { get; set; }
        public int DestinationOneSid { get; set; }
    }
}