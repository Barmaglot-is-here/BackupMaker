using System.Text;

namespace BackupMaker.Utils
{
    public static class EncodingSetter
    {
        public enum Type
        {
            Input,
            Output,
        }

        static EncodingSetter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static void Set(int codepage)
        {
            Encoding encoding = Encoding.GetEncoding(codepage);

            Console.OutputEncoding = encoding;
            Console.InputEncoding = encoding;
        }

        public static void Set(int codepage, Type type)
        {
            Encoding encoding = Encoding.GetEncoding(codepage);

            if (type == Type.Output)
                Console.OutputEncoding = encoding;
            else
                Console.InputEncoding = encoding;
        }
    }
}