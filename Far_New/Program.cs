using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;



namespace Far_New
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] drives = Environment.GetLogicalDrives();
            List<FileSystemInfo> directoriesAndFiles = new List<FileSystemInfo>();
            List<FileSystemInfo> current = new List<FileSystemInfo>();
           
            for(int i=0; i<drives.Count(); i++)
            {
                directoriesAndFiles.Add(new DirectoryInfo(drives[i]));
                current.Add(new DirectoryInfo(drives[i]));
            }

            int selected = 0;        
            Display(selected, current);
            bool end = false;
            try
            {
                while (!end)
                {

                    ConsoleKeyInfo keyPressed = Console.ReadKey();
                    DirectoryInfo temp = new DirectoryInfo(current[selected].FullName);
                    DirectoryInfo upd = temp;
                    upd = upd.Parent;
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selected > 0)
                            {
                                selected--;
                            }
                            else
                            {
                                selected = (current.Count - 1);
                            }

                            Display(selected, current);
                            //   DisplayTest(selected, curDir.FullName);
                            break;

                        case ConsoleKey.DownArrow:

                            if (selected < (current.Count - 1))
                            {
                                selected++;
                            }
                            else
                            {
                                selected = 0;
                            }
                            Display(selected, current);
                            break;


                        case ConsoleKey.RightArrow:


                            Show(current[selected]);

                            if (current[selected].GetType() == typeof(DirectoryInfo))
                            {
                                current = temp.GetFileSystemInfos().ToList();
                            }

                            selected = 0;
                            break;

                        case ConsoleKey.LeftArrow:

                            if (temp.Parent.Parent != null)
                            {
                                current = temp.Parent.Parent.GetFileSystemInfos().ToList();
                            }                         
                            else
                            {
                              
                                current = directoriesAndFiles;

                            }

                            Display(0, current);
                            break;

                        case ConsoleKey.F1:
                            DisplayHelp();
                            break;

                        case ConsoleKey.F2:
                            CreateFolder(current[selected]);
                            selected = 0;
                            current = upd.GetFileSystemInfos().ToList();
                            break;

                        case ConsoleKey.F3:
                            createNewFile(current[selected]);
                            selected = 0;
                            current = upd.GetFileSystemInfos().ToList();
                            break;

                        case ConsoleKey.F4:                            
                            DeleteSelected(selected, current);
                            current = upd.GetFileSystemInfos().ToList();
                            break;

                        case ConsoleKey.Escape:
                            end = true;
                            break;

                    }
                    Console.SetCursorPosition(0, Console.CursorTop);
                }
            }
            catch(Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            
        }

        private static void DeleteSelected(int index, List<FileSystemInfo> dir)
        {
            DirectoryInfo updated = new DirectoryInfo(dir[index].FullName);
            updated = updated.Parent;

            File.Delete(dir[index].FullName);

            dir = updated.GetFileSystemInfos().ToList();
            Display(0, dir);
     
            
        }

        private static void createNewFile(FileSystemInfo fileSystemInfo)
        {
            DirectoryInfo createHere = new DirectoryInfo(fileSystemInfo.FullName);
            createHere = createHere.Parent;

            Console.SetCursorPosition(0, 64);
            for (int i = 0; i < 66; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(4, 64);
            Console.Write("Specify file name: ");
            //  string fileName = Path.GetRandomFileName();
            string fileName = Console.ReadLine();

            string path = Path.Combine(createHere.FullName, fileName);
            File.Create(path);

            List<FileSystemInfo> dispFis = createHere.GetFileSystemInfos().ToList();

            int index = 0;
            for (int i = 0; i < dispFis.Count; i++)
            {
                if (dispFis[i].ToString() == fileName)
                {
                    index = i;
                }
            }

            Display(index, dispFis);
      //      return index;
        }

        private static void CreateFolder(FileSystemInfo fsi)
        {

            DirectoryInfo createHere = new DirectoryInfo(fsi.FullName);
            createHere = createHere.Parent;

            Console.SetCursorPosition(0, 64);
            for(int i=0;i<66;i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(4, 64);
            Console.Write("Specify folder name: ");
            string folderName = Console.ReadLine();

            string path = Path.Combine(createHere.FullName, folderName);
            Directory.CreateDirectory(path);
            
            List<FileSystemInfo> dispFis = createHere.GetFileSystemInfos().ToList();
            int index = 0;
            for (int i = 0; i <dispFis.Count;i++)
            {
                if(dispFis[i].ToString() == folderName)
                {
                    index = i;
                }
            }

                Display(index, dispFis);
          //  return index;
        }

        private static void DisplayHelp()
        {
            Console.Clear();
            Console.Write("Hello from my Far! Really glad to see you here.");
        }

        static void DisplayCurrent(FileSystemInfo dir)
        {
          
            Console.SetCursorPosition(0, 61);
            DirectoryInfo inDir = new DirectoryInfo(dir.FullName);
            if(inDir.Parent!=null)
            {
                Console.WriteLine(inDir.Parent.FullName);
            }
            
        }

        static void DisplayOptions()
        {
            Console.SetCursorPosition(4, 64);
            Console.ForegroundColor = ConsoleColor.White;
            string[] options = { "F1-Help", "F2-Create Folder", "F3-Create File", "F4-Delete" };
            for(int i=0;i<options.Length;i++)
            {
                Console.BackgroundColor = ConsoleColor.Red;                
                Console.Write(options[i]);
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.Write("    ");
            }
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(0, 0);
        }

        static void DrawBorders()
        {
            Console.SetCursorPosition(0, 62);
            for(int i=0;i<66;i++)
            {
                Console.Write("_");
            }

            Console.SetCursorPosition(0, 59);
            for (int i = 0; i < 66; i++)
            {
                Console.Write("_");
            }

            Console.SetCursorPosition(0, 0);
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.SetCursorPosition(0, 56);
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }

        static void Display(int index, List<FileSystemInfo> dir)
        {
            Console.SetWindowSize(66, 66);
            
            int top = Console.CursorTop;
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Clear();
            DrawBorders();
            DisplayOptions();           
      /*      FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = dir[index].FullName;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;*/
            //DirectoryInfo check = new DirectoryInfo()

            int line = 0;
            for (int i = 0; i < dir.Count(); i++)
            {
                if (line < 58)
                {
                    if (i == index)
                    {
                        Console.SetCursorPosition(0, index);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;

                        if (dir[index].GetType() == typeof(DirectoryInfo))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("[+] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(/*"[+] " +*/ dir[index]);
                            line++;
                        }
                        else
                        {
                            Console.WriteLine(dir[index]);
                            line++;

                        }

                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                    }


                    else
                    {

                        if (dir[i].GetType() == typeof(DirectoryInfo))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("[+] ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(/*"[+] " +*/ dir[i]);
                            line++;
                        }
                        else
                        {
                            Console.WriteLine(dir[i]);
                            line++;
                        }

                    }
                }
               
               
        
            }
            DisplayCurrent(dir[index]);
            Console.SetCursorPosition(0, top);
        }


        static void Show(FileSystemInfo fsi)
        {

            if (fsi.GetType() == typeof(DirectoryInfo))
            {
                DirectoryInfo dir = new DirectoryInfo(fsi.FullName);
                Display(0, dir.GetFileSystemInfos().ToList());
            }
            else
            {
                if (fsi.ToString().Contains(".jpg") || fsi.ToString().Contains(".jpeg") || fsi.ToString().Contains(".png") || fsi.ToString().Contains(".JPG"))
                {
                      Console.Clear();
                    /*  Console.WriteLine("image!!!!!!!");*/

                    Bitmap bmpSrc = new Bitmap(fsi.FullName, true);
                    ConsoleWriteImage(bmpSrc);
                }
                else
                {
                    System.Diagnostics.Process.Start(fsi.FullName);
                }
            }


        }

        static void ConsoleWritePixel(Color colorValue)
        {
            int[] colors = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };
            Color[] colorTable = colors.Select(x => Color.FromArgb(x)).ToArray(); //fromargb creates a Color structure (a,r,g,b)
            char[] rList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 }; //   fill ratio for ░▒▓█ is 1/4, 2/4, 3/4 and 4/4
            int[] bestHit = new int[] { 0, 0, 4, int.MaxValue }; //ForeColor, BackColor, Symbol, Score

            for (int rChar = rList.Length; rChar > 0; rChar--)
            {
                for (int foregroundColor = 0; foregroundColor < colorTable.Length; foregroundColor++)
                {
                    for (int backgroundColor = 0; backgroundColor < colorTable.Length; backgroundColor++)
                    {
                        int R = (colorTable[foregroundColor].R * rChar + colorTable[backgroundColor].R * (rList.Length - rChar)) / rList.Length;
                        int G = (colorTable[foregroundColor].G * rChar + colorTable[backgroundColor].G * (rList.Length - rChar)) / rList.Length;
                        int B = (colorTable[foregroundColor].B * rChar + colorTable[backgroundColor].B * (rList.Length - rChar)) / rList.Length;
                        int iScore = (colorValue.R - R) * (colorValue.R - R) + (colorValue.G - G) * (colorValue.G - G) + (colorValue.B - B) * (colorValue.B - B);

                        if (!(rChar > 1 && rChar < 4 && iScore > 50000)) // исключает слишком странные комбинации
                        {
                            if (iScore < bestHit[3])
                            {
                                bestHit[3] = iScore; 
                                bestHit[0] = foregroundColor; 
                                bestHit[1] = backgroundColor; 
                                bestHit[2] = rChar;  
                            }
                        }
                    }
                }
            }
            Console.ForegroundColor = (ConsoleColor)bestHit[0];
            Console.BackgroundColor = (ConsoleColor)bestHit[1];
            Console.Write(rList[bestHit[2] - 1]);
        }


        static void ConsoleWriteImage(Bitmap source)
        {
            int sMax = 39;
            decimal percent = Math.Min(decimal.Divide(sMax, source.Width), decimal.Divide(sMax, source.Height));
            Size dSize = new Size((int)(source.Width * percent), (int)(source.Height * percent));
            Bitmap bmpMax = new Bitmap(source, dSize.Width * 2, dSize.Height);
            for (int i = 0; i < dSize.Height; i++)
            {
                for (int j = 0; j < dSize.Width; j++)
                {
                    ConsoleWritePixel(bmpMax.GetPixel(j * 2, i));
                    ConsoleWritePixel(bmpMax.GetPixel(j * 2 + 1, i));
                }
                System.Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
