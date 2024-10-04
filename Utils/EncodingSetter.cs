using System.Text;

namespace BackupMaker.Utils;

public static class EncodingSetter
{
    public const int RU_ENCODING = 1251;

    static EncodingSetter() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    public static void Set(int codepage)
    {
        Encoding encoding = Encoding.GetEncoding(codepage);

        Console.OutputEncoding  = encoding;
        Console.InputEncoding   = encoding;
    }
}