using System.Data.Entity;

namespace HeartChamberIdentification.DAL
{
    public class PixelDbContex: DbContext
    {
        public PixelDbContex()
        : base("name=HeartChamberIdentificationEntities")
        {
            
        }

        public DbSet<Pixel> Pixels { get; set; }
    }
}
