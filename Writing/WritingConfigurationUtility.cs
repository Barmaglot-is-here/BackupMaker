using BackupMaker.Configuration;

namespace BackupMaker;
internal static class WritingConfigurationUtility
{
    public static WriteDelegate GetWriteFunction(Config config)
    {
        if (config.EnableOutput)
        {
            if (config.EnableLoging)
                return str =>
                {
                    Loger.Add(str);
                    Console.WriteLine(str);
                };

            return Console.WriteLine;
        }
        else if (config.EnableLoging)
            return Loger.Add;

        return _ => { };
    }
}