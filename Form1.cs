using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace RandomPicture
{
    public partial class Form1 : Form
    {
        private string[] imageFiles;
        private string currentImagePath;

        public Form1()
        {
            InitializeComponent();
            LoadRandomImage();

            // Set PictureBox to resize the image dynamically and maintain aspect ratio
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void LoadRandomImage()
        {
            // Path to the file containing the folder path
            string folderPathFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "imageFolderPath.txt");

            if (!File.Exists(folderPathFile))
            {
                // Define the path to the "My Images" folder
                string myImagesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Create the file and write the path to "My Images" if it doesn't exist
                try
                {
                    File.WriteAllText(folderPathFile, myImagesPath);
                    //MessageBox.Show($"The folder path file was created with the path: {myImagesPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while creating the folder path file: {ex.Message}");
                }
            }

            string folderPath = File.ReadAllText(folderPathFile).Trim();

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("The folder path specified in 'imageFolderPath.txt' does not exist.");
                return;
            }

            // Get all image files in the folder and subfolders
            imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                  .Where(file => file.ToLower().EndsWith(".jpg") ||
                                                 file.ToLower().EndsWith(".jpeg") ||
                                                 file.ToLower().EndsWith(".png") ||
                                                 file.ToLower().EndsWith(".bmp") ||
                                                 file.ToLower().EndsWith(".gif"))
                                  .ToArray();

            if (imageFiles.Length == 0)
            {
                MessageBox.Show("No image files found in the specified folder.");
                return;
            }

            // Initialize the randomizer with the current millisecond count as a seed
            Random random = new Random(DateTime.Now.Millisecond);

            // Select a random image
            currentImagePath = imageFiles[random.Next(imageFiles.Length)];

            // Display the image
            pictureBox.Image = Image.FromFile(currentImagePath);
        }

        private void btnOpenInExplorer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentImagePath))
            {
                // Open the image in the default image viewer
                System.Diagnostics.Process.Start("explorer.exe", currentImagePath);
            }
        }
    }
}