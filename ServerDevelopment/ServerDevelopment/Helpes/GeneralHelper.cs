namespace ServerDevelopment.Helpes
{
    public static class GeneralHelper
    {
        public static int CalculatePagesCount(int pageSize, int totalRows)
        {
            if ((totalRows % pageSize) == 0)
            {
                return totalRows / pageSize;
            }
            else
            {
                return totalRows / pageSize + 1;
            }
        }
    }
}
