using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using HeartChamberIdentification.Extensions;

namespace HeartChamberIdentification.Services
{
    public class ImageService
    {
        /// <summary>
        /// Gets the input for Artifician Neural Network. 
        /// </summary>
        /// <param name="path">The path to the directory containing training images.</param>
        /// <returns>Returns a bidimensional array of double representing the pixels gray values of images. 
        /// Each line of the array represents an image.</returns>
        public double[][] GetMlInputFromImages(string path)
        {
            var filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var images = GetFilesFrom(path, filters);
            var unlabeledImages = images.Where(i => !i.Contains("-label"));
            var mlInput = new List<double[]>();

            foreach (var image in unlabeledImages)
            {
                mlInput.Add(GetPixelsFromImage(image));
            }

            return mlInput.ToArray();
        }

        /// <summary>
        /// Gets all pixels from an image.
        /// </summary>
        /// <param name="path">The path to the image for which w want to get its pixels.</param>
        /// <returns>Returns an array of double representing all pixels gray values from the image.</returns>
        public double[] GetPixelsFromImage(string path)
        {
            var bitmapImage = new Bitmap(path);
            var imagePixels = new List<double>();
            var colorMax = int.MinValue;
            var colorMin = int.MaxValue;

            for (var x = 0; x < bitmapImage.Width; x++)
            {
                for (var y = 0; y < bitmapImage.Height; y++)
                {
                    var pixel = bitmapImage.GetPixel(x, y);
                    imagePixels.Add(pixel.R);

                    if (pixel.R > colorMax)
                        colorMax = pixel.R;

                    if (pixel.R < colorMin)
                        colorMin = pixel.R;
                }
            }

            return imagePixels.Normalize(colorMin, colorMax);
        }

        /// <summary>
        /// Gets the output for Artifician Neural Network.
        /// </summary>
        /// <param name="path">The path to the directory containing training images.</param>
        /// <returns>Returns a bidimensional array of double representing the pixels of images.
        /// If pixel is in a chamber, the value in the array is 1 (white), otherwise the value is 0 (black).
        /// Each line of the array represents an image.</returns>
        public double[][] GetMlOutputFromImages(string path)
        {
            var filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var images = GetFilesFrom(path, filters);
            var labeledImages = images.Where(i => i.Contains("-label"));
            var mlOutput = new List<double[]>();

            foreach (var image in labeledImages)
            {
                var bitmapImage = new Bitmap(image);
                var imagePixels = new List<double>();
                for (var x = 0; x < bitmapImage.Width; x++)
                {
                    for (var y = 0; y < bitmapImage.Height; y++)
                    {
                        var pixel = bitmapImage.GetPixel(x, y);
                        imagePixels.Add(IsPartOfContour(pixel) 
                                        ? 1
                                        : 0);
                    }
                }
                mlOutput.Add(imagePixels.ToArray());
            }

            return mlOutput.ToArray();
        }

        /// <summary>
        /// Adds contour to the image.
        /// </summary>
        /// <param name="image">The image on that we want to add contour.</param>
        /// <param name="pixels">The pixels of the new image.</param>
        /// <returns>Returns an inage of type Bitmap.</returns>
        public Bitmap AddContourToImage(Bitmap image, double[] pixels)
        {
            var i = 0;

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    if(pixels[i] >= 0.5)
                        image.SetPixel(x, y, Color.DarkOrange);
                    i++;
                }
            }

            return image;
        }

        private string[] GetFilesFrom(string searchFolder, string[] filters, bool isRecursive = false)
        {
            List<string> filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        public bool IsPartOfContour(Color pixel)
        {
            return pixel.R == 241 && pixel.G == 214 && pixel.B == 145;
        }
    }
}
