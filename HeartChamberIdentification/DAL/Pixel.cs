using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using MyMachineLearning.Interfaces;

namespace HeartChamberIdentification.DAL
{
    [Table("Pixels")]
    public class Pixel : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public Guid ImageId { get; set; }
        public string ImageName { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public bool IsContour { get; set; }

        public double[] ToInputModel()
        {
            return new double[]
            {
                Red,
                Color.FromArgb((int)Red, (int)Red, (int)Red).GetBrightness()
            };
        }

        public double[] ToOutputModel()
        {
            return new double[]
            {
                Convert.ToInt32(IsContour)
            };
        }
    }
}
