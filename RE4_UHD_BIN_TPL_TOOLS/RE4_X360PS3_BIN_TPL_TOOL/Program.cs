namespace RE4_X360PS3_BIN_TPL_TOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            SHARED_UHD_BIN_TPL.MainAction.MainContinue(args, false, SimpleEndianBinaryIO.Endianness.BigEndian);
        }
    }
}
