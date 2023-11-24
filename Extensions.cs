namespace BackupMaker
{
    internal static class Extensions
    {
        public static bool Contains<T>(this T[] array, T? element)
        {
            foreach (var item in array)
                if (EqualityComparer<T>.Default.Equals(item, element))
                    return true;
             
            return false;
        }
    }
}