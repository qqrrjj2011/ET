namespace ET.Server
{
    [FriendOf(typeof(ServerItem))]
    public static class ItemHelper
    {
        public static void RandomQuality(this ServerItem item)
        {
            int rate = RandomGenerator.RandomNumber(0, 10000);
            if (rate< 4000)
            {
                item.Quality = (int)ItemQuality.General;
            }
            else if (rate < 7000)
            {
                item.Quality =  (int)ItemQuality.Good;
            }
            else if (rate < 8500)
            {
                item.Quality = (int)ItemQuality.Excellent;
            }
            else if (rate < 9500)
            {
                item.Quality = (int)ItemQuality.Epic;
            }
            else if (rate < 10000)
            {
                item.Quality = (int)ItemQuality.Legend;
            }
        }
    }
}