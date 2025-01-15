namespace Yaba.Models
{
    public static class WhiskyExtensions
    {
        public static WhiskyEntity ToEntity(this Whisky whisky)
        {
            return new WhiskyEntity()
            {

            };
        }

        public static Whisky ToDomain(this WhiskyEntity whiskyEntity)
        {
            return new Whisky()
            {

            };
        }
    }
}
