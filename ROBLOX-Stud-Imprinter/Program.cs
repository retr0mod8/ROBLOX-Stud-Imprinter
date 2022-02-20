using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ROBLOX_Stud_Imprinter
{
    internal class Program
    {
        private static string FolderPath;
        static string SettingsFile = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\retr0mod8\ROBLOX-Stud-Imprinter\" + @"path.ini";
        static void Main(string[] args)
        {
            if (!File.Exists(@SettingsFile))
            {
                Directory.CreateDirectory(@Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\retr0mod8\ROBLOX-Stud-Imprinter\");
                File.Create(@SettingsFile).Close();
            }
            FolderPath = @File.ReadAllText(@SettingsFile);
            if (!Directory.Exists(@FolderPath))
            {
                while (true)
                {
                    Console.WriteLine("Please enter the path where the images will be saved:");
                    FolderPath = @Console.ReadLine();
                    Console.Clear();
                    if (!Directory.Exists(@FolderPath))
                    {
                        Console.WriteLine("Invalid path! Please only input a directory which exists.");
                    }
                    else
                    {
                        File.WriteAllText(@SettingsFile, @FolderPath);
                        break;
                    }
                        
                }
            }
            while (true)
            {
                Console.WriteLine("Current path is: {0}", @FolderPath);
                Console.WriteLine("Welcome to the Main Menu!");
                Console.WriteLine("Type 'start' to make a new texture");
                Console.WriteLine("Type 'path' to change the output path");
                switch (getChar())
                {
                    case 's':
                        Console.Clear();
                        startImage();
                        break;

                    case 'p':
                        Console.Clear();
                        changePath();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Incorrect input! Please try again!");
                        break;
                }
            }
        }

        static char getChar()
        {
            try
            {
                return @Console.ReadLine()[0];
            }
            catch (Exception)
            {
                return '\n';
            }
        }

        static void changePath()
        {
            while (true)
            {
                Console.WriteLine("Please enter the path where the images will be saved:");
                FolderPath = @Console.ReadLine();
                Console.Clear();
                if (!Directory.Exists(@FolderPath))
                {
                    Console.WriteLine("Invalid path! Please only input a directory which exists.");
                }
                else
                {
                    File.WriteAllText(@SettingsFile, @FolderPath);
                    break;
                }
                    
            }
        }

        static void startImage()
        {
            Bitmap[] BitmapTable = { Properties.Resources.studs, Properties.Resources.inlets, Properties.Resources.glue, Properties.Resources.universal, null };
            string[] BitmapNameTable = { "studs", "inlets", "glue", "universal", "smooth" };
            byte mR;
            byte mG;
            byte mB;
            UInt32 mA;
            while (true)
            {
                Console.WriteLine("Enter R Value:");
                try
                {
                    mR = Convert.ToByte(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Please try again!");
                }
            }
            while (true)
            {
                Console.WriteLine("Enter G Value:");
                try
                {
                    mG = Convert.ToByte(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Please try again!");
                }
            }
            while (true)
            {
                Console.WriteLine("Enter B Value:");
                try
                {
                    mB = Convert.ToByte(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Please try again!");
                }
            }
            while (true)
            {
                Console.WriteLine("Enter Alpha Value: (0 - 100)");
                try
                {
                    mA = Convert.ToUInt32(Console.ReadLine());
                    if (mA > 100)
                        throw new Exception();
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Please try again!");
                }
            }
            Console.Clear();
            while (true)
            {
                bool exit = false;
                float Luminance;
                while (true)
                {
                    Console.WriteLine("Please enter the intensity:");
                    try
                    {
                        Luminance = Convert.ToSingle(Console.ReadLine());
                        if (Luminance > 2)
                            throw new Exception();
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input! Please input a number between 0 and 2");
                    }
                }

                for(int i = 0; i < 5; i++)
                {
                    Bitmap LocalOurs = drawImage(BitmapTable[i], mR, mG, mB, Convert.ToByte(Math.Ceiling(255 * (mA / 100f))), Luminance);
                    if(mA == 100)
                        LocalOurs.Save(@FolderPath + @"\" + BitmapNameTable[i] + "_" + mR + "_" + mG + "_" + mB + ".png");
                    else
                        LocalOurs.Save(@FolderPath + @"\" + BitmapNameTable[i] + "_" + mR + "_" + mG + "_" + mB + "_"+"trans-"+mA+ ".png");
                }
                while (true)
                {
                    Console.WriteLine("Please check your output!");
                    Console.WriteLine("If it looks good, type 'e'");
                    Console.WriteLine("If it looks bad, type 'b' to change the intensity");
                    string input = Console.ReadLine().ToLower();
                    if(input == "e")
                    {
                        exit = true;
                        break;
                    }
                    else if(input == "b")
                    {
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Incorrect input! Try again!");
                    }
                }
                Console.Clear();
                if (exit)
                    break;
                
                
            }
        }

        static Bitmap drawImage(Bitmap Imprint, byte R, byte G, byte B, byte A, float intensity)
        {
            if(Imprint == null)
            {
                Bitmap Bmp = new Bitmap(128, 128);
                using (Graphics gfx = Graphics.FromImage(Bmp))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(A, R, G, B)))
                {
                    gfx.FillRectangle(brush, 0, 0, 128, 128);
                }
                return Bmp;
            }
            Bitmap TemplateImage = Imprint;
            Bitmap ExitImage = new Bitmap(TemplateImage.Width, TemplateImage.Height);
            int NumberOfPixels = TemplateImage.Width * TemplateImage.Height;
            int DeltaX = 0;
            int DeltaY = 0;
            byte localR;
            byte localG;
            byte localB;
            for (int i = 0; i < NumberOfPixels; i++)
            {
                localR = R;
                localG = G;
                localB = B;
                if (DeltaX == TemplateImage.Width)
                {
                    DeltaX = 0;
                    DeltaY++;
                }
                Color ImprintPixel = TemplateImage.GetPixel(DeltaX, DeltaY);

                if (ImprintPixel.R > 126)
                {
                    byte offset = Convert.ToByte(ImprintPixel.R - 126);
                    offset = Convert.ToByte(offset * intensity);
                    if ((localR + offset) > 255)
                        localR = 255;
                    else
                        localR += offset;
                    if ((localG + offset) > 255)
                        localG = 255;
                    else
                        localG += offset;
                    if ((localB + offset) > 255)
                        localB = 255;
                    else
                        localB += offset;

                }
                if (ImprintPixel.R < 126)
                {
                    byte offset = Convert.ToByte(126 - ImprintPixel.R);
                    offset = Convert.ToByte(offset * intensity);
                    if ((localR - offset) < 0)
                        localR = 0;
                    else
                        localR -= offset;
                    if ((localG - offset) < 0)
                        localG = 0;
                    else
                        localG -= offset;
                    if ((localB - offset) < 0)
                        localB = 0;
                    else
                        localB -= offset;
                }
                ExitImage.SetPixel(DeltaX, DeltaY, Color.FromArgb(A, localR, localG, localB));

                DeltaX++;
            }
            return ExitImage;
        }
    }
}
