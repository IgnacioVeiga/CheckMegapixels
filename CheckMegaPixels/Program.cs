using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CheckMegaPixels
{
    public class Program
    {
        public static List<string> listImagesToMove = new List<string>();

        #region Counter
        public static int equalOrHigher = 0;
        public static int lessThan = 0;
        public static int incompatible = 0;
        #endregion Counter

        public static void Main(string[] args)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                string[] exts = new string[] { "jpg", "jpeg", "jpe", "jif", "jfif", "jfi", "webp", "gif", "png", "apng", "bmp", "tiff", "tif" };
                string[] arrayImages = new string[] { };

                arrayImages = FilterFiles(path, exts);

                foreach (string imgPath in arrayImages)
                {
                    Bitmap img = CreateBitmap(imgPath);

                    if (img != null)
                    {
                        float megaPixels = float.Parse((img.Height * img.Width / 1000000f).ToString("0.0"));

                        if (megaPixels >= 16)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n" + "Height: " + img.Height + " Width: " + img.Width + " | " + megaPixels + " Megapixels\n" + imgPath);
                            equalOrHigher++;
                            listImagesToMove.Add(imgPath);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(".");
                            lessThan++;
                        }
                        img.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n-------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error\n" + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------------");
                incompatible++;
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nNumber of images of 16 Megapixels or higher: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(equalOrHigher);

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Number of images less than 16 Megapixels: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(lessThan);

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Number of incompatible images: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(incompatible);


                if (equalOrHigher > 0)
                {
                    Console.Write("Move images? (y/n): ");
                    string resp = Console.ReadLine().ToLower();
                    if (resp == "y")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Moving images...");
                        MoveFiles(listImagesToMove);
                    }

                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nEnd of program, press any key to close");
                Console.ReadKey();
            }
        }

        public static Bitmap CreateBitmap(string filename)
        {
            try
            {
                return new Bitmap(filename);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n-------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error\n" + ex.Message);
                Console.WriteLine(filename);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------------");
                incompatible++;
                return null;
            }
        }

        public static string[] FilterFiles(string path, string[] exts)
        {
            try
            {
                return
                    Directory
                    .GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(file => exts.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                    .ToArray();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n-------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error\n" + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------------");
                incompatible++;
                return null;
            }
        }

        public static void MoveFiles(List<string> paths)
        {
            try
            {
                string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/CheckMegapixels/";
                Console.WriteLine("Default path: " + defaultPath);
                Console.Write("Where move? (enter a path or leave blank to use default): ");
                string moveImagesHerePath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(moveImagesHerePath))
                {
                    moveImagesHerePath = defaultPath;
                }
                else
                {
                    if (!Path.EndsInDirectorySeparator(moveImagesHerePath))
                    {
                        moveImagesHerePath += @"\";
                    }
                }

                foreach (var filename in paths)
                {
                    if (File.Exists(filename))
                    {
                        File.Move(filename, moveImagesHerePath + Path.GetFileName(filename));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n-------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error\n" + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------------");
            }
        }
    }
}
