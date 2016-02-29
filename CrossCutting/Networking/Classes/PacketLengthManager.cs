namespace Aegis.CrossCutting.Network.Classes
{
    public class PacketLengthManager
    {
        public static int? GetPacketInformation(PACKET_COMMAND command)
        {
            if (!Command.HasMethod(command))
            {
                throw new BufferException("CClient::OnPacket()", string.Format("{0} (0x{0:X4}) packet is unknown", (ushort) command), null);
            }

            return Command.GetPacketInfo(command);
        }
    }
}