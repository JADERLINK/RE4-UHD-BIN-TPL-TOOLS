namespace RE4_PS4NS_BIN_TPL_TOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            SHARED_UHD_BIN_TPL.MainAction.MainContinue(args, true, SimpleEndianBinaryIO.Endianness.LittleEndian);
        }
    }
}
