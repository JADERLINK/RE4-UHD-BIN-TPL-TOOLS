using System;

namespace SHARED_TOOLS
{
    public static class Shared
    {
        private const string VERSION = "V.1.4.1 (2025-09-02)";

        public static string HeaderText()
        {
            return "# github.com/JADERLINK/RE4-UHD-BIN-TPL-TOOLS" + Environment.NewLine +
                   "# youtube.com/@JADERLINK" + Environment.NewLine +
                   "# RE4_PS4NS_BIN_TPL_TOOL by: JADERLINK" + Environment.NewLine +
                   "# Thanks to \"mariokart64n\" and \"CodeMan02Fr\"" + Environment.NewLine +
                   "# Material information by \"Albert\"" + Environment.NewLine +
                  $"# Version {VERSION}";
        }

        public static string HeaderTextSmd()
        {
            return "// RE4_PS4NS_BIN_TPL_TOOL" + Environment.NewLine +
                   "// by: JADERLINK" + Environment.NewLine +
                   "// youtube.com/@JADERLINK" + Environment.NewLine +
                  $"// Version {VERSION}";
        }
    }
}
