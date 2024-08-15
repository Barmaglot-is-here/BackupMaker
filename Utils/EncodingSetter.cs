using System.Text;

namespace BackupMaker.Utils;

public static class EncodingSetter
{
    static EncodingSetter() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    public static void Set(int codepage)
    {
        Encoding encoding = Encoding.GetEncoding(codepage);

        Console.OutputEncoding  = encoding;
        Console.InputEncoding   = encoding;
    }
}